using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<List<Staff>> GetAllStaff();
    Task<Staff?> GetStaffById(int id);
    Task<Staff?> GetStaffByLogin(string username);
    Task AddStaff(Staff staff);
    Task UpdateStaff(Staff staff);
    Task DeleteStaff(int id);
}

public class StaffRepository(RestaurantContext context) : IStaffRepository
{
    
    public async Task<List<Staff>> GetAllStaff()
    {
        return await context.Staff.ToListAsync();
    }

    public async Task<Staff?> GetStaffById(int id)
    {
        var staff = await context.Staff.FindAsync(id);
        return staff ?? null;
    }

    public async Task<Staff?> GetStaffByLogin(string login)
    {
        var staff = await context.Staff.FirstOrDefaultAsync(x => x.Login == login);
        return staff ?? null;
    }

    public async Task AddStaff(Staff staff)
    {
        context.Staff.Add(staff);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStaff(Staff staff)
    {
        context.Entry(staff).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteStaff(int id)
    {
        var staff = await context.Staff.FindAsync(id);
        if (staff != null)
        {
            context.Staff.Remove(staff);
            await context.SaveChangesAsync();
        }
    }
}