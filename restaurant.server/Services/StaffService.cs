using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
    Task<StaffModel?> GetById(int id);
}

public class StaffService(IStaffRepository staffRepository) : IStaffService
{
    public async Task<List<StaffModel>> GetAll()
    {
        var staff = await staffRepository.GetAll();
        
        var staffModels = staff.Select(s => new StaffModel
        {
            IdEmployee = s.IdEmployee,
            Position = s.IdPositionNavigation.Title,
            LastName = s.LastName,
            FirstName = s.FirstName,
            MiddleName = s.MiddleName,
            PhoneNumber = s.PhoneNumber
        });
        
        return staffModels.ToList();
    }

    public async Task<StaffModel?> GetById(int id)
    {
        var staff = await staffRepository.GetById(id);
        if (staff == null)
            return null;

        var staffModel = new StaffModel
        {
            IdEmployee = staff.IdEmployee,
            Position = staff.IdPositionNavigation.Title,
            LastName = staff.LastName,
            FirstName = staff.FirstName,
            MiddleName = staff.MiddleName,
            PhoneNumber = staff.PhoneNumber
        };
        
        return staffModel;
    }
    
}