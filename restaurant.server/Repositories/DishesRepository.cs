using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;

namespace restaurant.server.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<DishModel>> GetAll();
}

public class DishesRepository(RestaurantContext context) : IDishesRepository
{
    public async Task<IEnumerable<DishModel>> GetAll()
    {
        var dishes =
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
        
        return await dishes.ToListAsync();
    }
}