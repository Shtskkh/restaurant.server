using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IDishesService
{
    Task<List<DishModel>> GetAll();
}

public class DishesService(IDishesRepository dishesRepository) : IDishesService
{
    public async Task<List<DishModel>> GetAll()
    {
        var dishes = await dishesRepository.GetAll();
        return dishes.ToList();
    }
}