using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;

namespace restaurant.server.Repositories;

public interface IDishesRepository
{
    Task<List<DishModel>> GetAllAsync();
    Task<DishModel?> GetByIdAsync(int idDish);
    Task<List<ProductInDishModel>> GetProductsInDishAsync(int idDish);
}

public class DishesRepository(RestaurantContext context) : IDishesRepository
{
    public async Task<List<DishModel>> GetAllAsync()
    {
        var dishesModels =
            from dish in context.Dishes.AsNoTracking()
            join unit in context.MeasureUnits.AsNoTracking()
                on dish.IdUnit equals unit.IdUnit
            select new DishModel
            {
                IdDish = dish.IdDish,
                Title = dish.Title,
                Cost = dish.Cost,
                Availability = dish.Availability,
                WeightVolume = dish.WeightVolume,
                Unit = unit.Title
            };

        return await dishesModels.ToListAsync();
    }

    public async Task<DishModel?> GetByIdAsync(int idDish)
    {
        var dishModels =
            from dish in context.Dishes.AsNoTracking()
            where dish.IdDish == idDish
            join unit in context.MeasureUnits.AsNoTracking()
                on dish.IdUnit equals unit.IdUnit
            select new DishModel
            {
                IdDish = dish.IdDish,
                Title = dish.Title,
                Cost = dish.Cost,
                Availability = dish.Availability,
                WeightVolume = dish.WeightVolume,
                Unit = unit.Title
            };

        return await dishModels.FirstOrDefaultAsync();
    }

    public async Task<List<ProductInDishModel>> GetProductsInDishAsync(int idDish)
    {
        var productsModels =
            from productInDish in context.ProductsInDishes.AsNoTracking()
            where productInDish.IdDish == idDish
            join product in context.Products.AsNoTracking()
                on productInDish.IdProduct equals product.IdProduct
            join unit in context.MeasureUnits.AsNoTracking()
                on productInDish.IdUnit equals unit.IdUnit
            select new ProductInDishModel
            {
                IdProduct = productInDish.IdProduct,
                Title = product.Title,
                Count = productInDish.Count,
                Unit = unit.Title
            };

        return await productsModels.ToListAsync();
    }
}