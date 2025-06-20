using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IDishesService
{
    Task<List<DishModel>> GetAllAsync();
    Task<DishModel?> GetByIdAsync(int id);
}

public class DishesService(IDishesRepository dishesRepository) : IDishesService
{
    public async Task<List<DishModel>> GetAllAsync()
    {
        return await dishesRepository.GetAllAsync();
    }

    public async Task<DishModel?> GetByIdAsync(int id)
    {
        var dishModel = await dishesRepository.GetByIdAsync(id);

        if (dishModel == null)
            return null;

        var products = await dishesRepository.GetProductsInDishAsync(dishModel.IdDish);
        dishModel.Products = products;

        return dishModel;
    }
}