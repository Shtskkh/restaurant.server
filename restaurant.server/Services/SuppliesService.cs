using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface ISuppliesService
{
    Task<List<SupplyModel>> GetAll();
}

public class SuppliesService(ISuppliesRepository suppliesRepository) : ISuppliesService
{
    public async Task<List<SupplyModel>> GetAll()
    {
        return await suppliesRepository.GetAll();
    }
}