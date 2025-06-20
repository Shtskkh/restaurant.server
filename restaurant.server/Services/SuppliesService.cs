using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface ISuppliesService
{
    Task<List<SupplyModel>> GetAllAsync();
}

public class SuppliesService(ISuppliesRepository suppliesRepository) : ISuppliesService
{
    public async Task<List<SupplyModel>> GetAllAsync()
    {
        return await suppliesRepository.GetAllAsync();
    }
}