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
    Task<RepositoryResult<Staff>> AddAsync(Staff staff);
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

    public async Task<RepositoryResult<Staff>> AddAsync(Staff staff)
    {
        logger.LogInformation("Adding new staff with login: {login}... ", staff.Login);
        try
        {
            await context.Staff.AddAsync(staff);
            await context.SaveChangesAsync();
            return RepositoryResult<Staff>.Success(staff);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Database error when adding employee: {login}", staff.Login);
            return RepositoryResult<Staff>.Fail("Database error: " + e.InnerException?.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error when adding an employee: {login}", staff.Login);
            return RepositoryResult<Staff>.Fail("Error: " + e.Message);
        }
    }

    public async Task<RepositoryResult<StaffModel>> GetByLoginAsync(string login)
    {
        logger.LogInformation("Getting employee with login: {login}...", login);
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
            logger.LogError(e, "Unexpected error when getting employee with login: {Login}.", login);
            return RepositoryResult<StaffModel>.Fail("Error: " + e.Message);
        }
    }
}