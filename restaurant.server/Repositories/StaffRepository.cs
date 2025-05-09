﻿using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IStaffRepository
{
    Task<List<StaffModel>> GetAll();
    Task<StaffModel?> GetById(int idEmployee);
    Task<Staff?> GetLoginInfo(string login);
}

public class StaffRepository(RestaurantContext context) : IStaffRepository
{
    public async Task<List<StaffModel>> GetAll()
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

    public async Task<StaffModel?> GetById(int idEmployee)
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

    public async Task<Staff?> GetLoginInfo(string login)
    {
        return await context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Login == login);
    }
}