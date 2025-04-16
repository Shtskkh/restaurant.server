using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
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
}