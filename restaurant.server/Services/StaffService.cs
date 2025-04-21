using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
    Task<StaffModel?> GetById(int id);
}

public class StaffService(StaffRepository staffRepository) : IStaffService
{
    public async Task<List<StaffModel>> GetAll()
    {
        return await staffRepository.GetAll();
    }

    public async Task<StaffModel?> GetById(int id)
    {
        var staffModel = await staffRepository.GetById(id);
        return staffModel ?? null;
    }
}