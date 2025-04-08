using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
    Task<StaffModel?> GetById(int id);
    Task<StaffModel?> GetByLogin(string login);
}

public class StaffService(IStaffRepository staffRepository, IPositionsRepository positionsRepository) : IStaffService
{
    public async Task<List<StaffModel>> GetAll()
    {
        var allStaff = await staffRepository.GetAll();
        var allPositions = await positionsRepository.GetAll();
        var allStaffDto = allStaff
            .Join(allPositions,
                s => s.IdPosition,
                p => p.IdPosition,
                (s, p) => new StaffModel
                {
                    IdEmployee = s.IdEmployee,
                    IdPosition = s.IdPosition,
                    Position = p.Title,
                    LastName = s.LastName,
                    FirstName = s.FirstName,
                    MiddleName = s.MiddleName,
                    PhoneNumber = s.PhoneNumber,
                });

        return allStaffDto.ToList();
    }

    public async Task<StaffModel?> GetById(int id)
    {
        var staff = await staffRepository.GetById(id);
        if (staff == null) return null;
        var position = await positionsRepository.GetById(staff.IdPosition);
        var staffDto = new StaffModel
        {
            IdEmployee = staff.IdEmployee,
            IdPosition = staff.IdPosition,
            Position = position!.Title,
            LastName = staff.LastName,
            FirstName = staff.FirstName,
            MiddleName = staff.MiddleName,
            PhoneNumber = staff.PhoneNumber,
        };
        
        return staffDto;
    }
    
    public async Task<StaffModel?> GetByLogin(string login)
    {
        var staff = await staffRepository.GetByLogin(login);
        if (staff == null) return null;
        var position = await positionsRepository.GetById(staff.IdPosition);
        var staffDto = new StaffModel
        {
            IdEmployee = staff.IdEmployee,
            IdPosition = staff.IdPosition,
            Position = position!.Title,
            LastName = staff.LastName,
            FirstName = staff.FirstName,
            MiddleName = staff.MiddleName,
            PhoneNumber = staff.PhoneNumber,
        };
        
        return staffDto;
    }
}