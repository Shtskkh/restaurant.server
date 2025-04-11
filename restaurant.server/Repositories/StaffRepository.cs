using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<List<Staff>> GetAll();
    Task<Staff?> GetById(int id);
    Task<Staff?> GetByLogin(string login);
    Task<List<Staff>> GetByPosition(int idPosition);
    Task Add(Staff staff);
    Task Update(Staff staff);
    Task Delete(int id);
}

public class StaffRepository(RestaurantContext context) : IStaffRepository
{
    public async Task<List<Staff>> GetAll()
    {
        return await context.Staff.AsNoTracking().ToListAsync();
    }

    public async Task<Staff?> GetById(int id)
    {
        return await context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.IdEmployee == id);
    }

    public async Task<Staff?> GetByLogin(string login)
    {
        return await context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Login == login);
    }

    public async Task<List<Staff>> GetByPosition(int idPosition)
    {
        return await context.Staff.AsNoTracking().Where(s => s.IdPosition == idPosition).ToListAsync();
    }

    public async Task Add(Staff staff)
    {
        try
        {
            context.Staff.Add(staff);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить сотрудника.");
        }
    }

    public async Task Update(Staff staff)
    {
        var existingStaff = await GetById(staff.IdEmployee);
        if (existingStaff == null)
            throw new Exception("Сотрудник не найден.");
        
        context.Staff.Entry(existingStaff).CurrentValues.SetValues(staff);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var staff = await GetById(id);
        if (staff != null)
        {
            context.Staff.Remove(staff);
            await context.SaveChangesAsync();
        }
    }
}