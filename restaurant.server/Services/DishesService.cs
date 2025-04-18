using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IDishesService
{
    Task<List<DishModel>> GetAll();
    Task<DishModel?> GetById(int id);
}

public class DishesService(IDishesRepository dishesRepository) : IDishesService
{
    public async Task<List<DishModel>> GetAll()
    {
        var dishes = await dishesRepository.GetAll();
        return dishes.ToList();
    }

    public async Task<DishModel?> GetById(int id)
    {
        var dish = await dishesRepository.GetById(id);
        return dish;
    }
}