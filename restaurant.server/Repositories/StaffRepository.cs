using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<IEnumerable<Staff>> GetAll();
    Task<Staff?> GetById(int id);
    Task<Staff?> GetByLogin(string login);
}

public class StaffRepository(RestaurantContext context) : IStaffRepository
{
    public async Task<IEnumerable<Staff>> GetAll()
    {
        return await context.Staff.AsNoTracking().Include(s => s.IdPositionNavigation).ToListAsync();
    }

    public async Task<Staff?> GetById(int id)
    {
        return await context.Staff.AsNoTracking().Include(s => s.IdPositionNavigation)
            .FirstOrDefaultAsync(s => s.IdEmployee == id);
    }

    public async Task<Staff?> GetByLogin(string login)
    {
        return await context.Staff.AsNoTracking().Include(s => s.IdPositionNavigation)
            .FirstOrDefaultAsync(s => s.Login == login);
    }
}