using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public class StaffService(StaffRepository staffRepository)
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