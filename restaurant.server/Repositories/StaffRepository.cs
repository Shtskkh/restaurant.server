using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<List<StaffModel>> GetAllAsync();
    Task<StaffModel?> GetByIdAsync(int idEmployee);
    Task<Staff?> GetLoginInfoAsync(string login);
    Task<RepositoryResult<Staff>> AddAsync(Staff newStaff);
    Task<RepositoryResult<StaffModel>> GetByLoginAsync(string login);
}

// TODO: Переписать все методы с использованием RepositoryResult
public class StaffRepository(RestaurantContext context, ILogger<StaffRepository> logger) : IStaffRepository
{
    public async Task<List<StaffModel>> GetAllAsync()
    {
        var staffModels =
            from s in context.Staff.AsNoTracking()
            join p in context.Positions.AsNoTracking()
                on s.IdPosition equals p.IdPosition
            select new StaffModel
            {
                IdEmployee = s.IdEmployee,
                Position = p.Title,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MiddleName = s.MiddleName,
                PhoneNumber = s.PhoneNumber
            };

        return await staffModels.ToListAsync();
    }

    public async Task<StaffModel?> GetByIdAsync(int idEmployee)
    {
        var staffModel =
            from s in context.Staff.AsNoTracking()
            where s.IdEmployee == idEmployee
            join p in context.Positions.AsNoTracking()
                on s.IdPosition equals p.IdPosition
            select new StaffModel
            {
                IdEmployee = s.IdEmployee,
                Position = p.Title,
                FirstName = s.FirstName,
                LastName = s.LastName,
                MiddleName = s.MiddleName,
                PhoneNumber = s.PhoneNumber
            };

        return await staffModel.FirstOrDefaultAsync();
    }

    public async Task<Staff?> GetLoginInfoAsync(string login)
    {
        return await context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Login == login);
    }

    public async Task<RepositoryResult<Staff>> AddAsync(Staff newStaff)
    {
        try
        {
            await context.Staff.AddAsync(newStaff);
            await context.SaveChangesAsync();
            logger.LogInformation("Сотрудник успешно добавлен.");
            return RepositoryResult<Staff>.Success(newStaff);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Ошибка базы данных при добавлении сотрудника: {Login}", newStaff.Login);
            return RepositoryResult<Staff>.Fail("Ошибка базы данных: " + e.InnerException?.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Непредвиденная ошибка при добавлении сотрудника: {Login}", newStaff.Login);
            return RepositoryResult<Staff>.Fail("Произошла ошибка: " + e.Message);
        }
    }

    public async Task<RepositoryResult<StaffModel>> GetByLoginAsync(string login)
    {
        try
        {
            var staffModel = await (
                from s in context.Staff.AsNoTracking()
                where s.Login == login
                join p in context.Positions.AsNoTracking()
                    on s.IdPosition equals p.IdPosition
                select new StaffModel
                {
                    IdEmployee = s.IdEmployee,
                    Position = p.Title,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    MiddleName = s.MiddleName,
                    PhoneNumber = s.PhoneNumber
                }).FirstOrDefaultAsync();

            if (staffModel != null) return RepositoryResult<StaffModel>.Success(staffModel);

            logger.LogError("Employee with login: {Login} not found.", login);
            return RepositoryResult<StaffModel>.Fail($"Employee with login: {login} not found.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting employee with login: {Login}.", login);
            return RepositoryResult<StaffModel>.Fail("Error: " + e.Message);
        }
    }
}