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
        return await dishesRepository.GetAll();
    }

    public async Task<DishModel?> GetById(int id)
    {
        var dishModel = await dishesRepository.GetById(id);

        if (dishModel == null)
            return null;

        var products = await dishesRepository.GetProductsInDish(dishModel.IdDish);
        dishModel.Products = products;

        return dishModel;
    }
}