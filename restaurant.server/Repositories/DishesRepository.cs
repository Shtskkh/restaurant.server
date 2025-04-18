using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;

namespace restaurant.server.Repositories;

public interface IDishesRepository
{
    Task<IEnumerable<DishModel>> GetAll();
    Task<DishModel?> GetById(int id);
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

    public async Task<DishModel?> GetById(int id)
    {
        var dish =
            from d in context.Dishes.AsNoTracking()
            where d.IdDish == id
            join unit in context.MeasureUnits.AsNoTracking()
                on d.IdUnit equals unit.IdUnit
            select new DishModel
            {
                IdDish = d.IdDish,
                Title = d.Title,
                Cost = d.Cost,
                Availability = d.Availability,
                WeightVolume = d.WeightVolume,
                Unit = unit.Title
            };

        return await dish.FirstOrDefaultAsync();
    }
}