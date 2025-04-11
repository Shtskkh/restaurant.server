using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IDishesRepository
{
    Task<List<Dish>> GetAll();
    Task<Dish?> GetById(int id);
    Task<Dish?> GetByTitle(string title);
    Task Add(Dish dish);
    Task Update(Dish dish);
    Task Delete(int id);
}

public class DishesRepository(RestaurantContext context) : IDishesRepository
{
    public async Task<List<Dish>> GetAll()
    {
        return await context.Dishes.AsNoTracking().ToListAsync();
    }

    public async Task<Dish?> GetById(int id)
    {
        return await context.Dishes.AsNoTracking().FirstOrDefaultAsync(d => d.IdDish == id);
    }

    public async Task<Dish?> GetByTitle(string title)
    {
        return await context.Dishes.AsNoTracking().FirstOrDefaultAsync(d => d.Title == title);
    }
    
    public async Task Add(Dish dish)
    {
        try
        {
            context.Dishes.Add(dish);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить продукт.");
        }
    }

    public async Task Update(Dish dish)
    {
        context.Dishes.Entry(dish).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var dish = await GetById(id);
        if (dish != null)
        {
            context.Dishes.Remove(dish);
            await context.SaveChangesAsync();
        }
    }
    
}