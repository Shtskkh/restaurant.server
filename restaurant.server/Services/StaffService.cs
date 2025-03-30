using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
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
}