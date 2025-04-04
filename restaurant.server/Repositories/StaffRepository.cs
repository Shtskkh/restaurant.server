﻿using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<List<Staff>> GetAll();
    Task<Staff?> GetById(int id);
    Task<Staff?> GetByLogin(string login);
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

    public async Task Add(Staff staff)
    {
        context.Staff.Add(staff);
        await context.SaveChangesAsync();
    }

    public async Task Update(Staff staff)
    {
        context.Entry(staff).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var staff = await context.Staff.FirstOrDefaultAsync(s => s.IdPosition == id);
        if (staff != null)
        {
            context.Staff.Remove(staff);
            await context.SaveChangesAsync();
        }
    }
}