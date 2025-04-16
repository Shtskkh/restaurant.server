using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<IEnumerable<Staff>> GetAll();
}

public class StaffRepository(RestaurantContext context) : IStaffRepository
{
    public async Task<IEnumerable<Staff>> GetAll()
    {
        return await context.Staff.AsNoTracking().Include(s => s.IdPositionNavigation).ToListAsync();
    }
}