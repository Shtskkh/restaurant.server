using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product?> GetByTitle(string title);
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(int id);
}

public class ProductsRepository(RestaurantContext context) : IProductRepository
{
    public async Task<List<Product>> GetAll()
    {
        return await context.Products.AsNoTracking().ToListAsync();
    }

    public async Task<Product?> GetById(int id)
    {
        return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.IdProduct == id);
    }

    public async Task<Product?> GetByTitle(string title)
    {
        return await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Title == title);
    }

    public async Task Add(Product product)
    {
        try
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить продукт.");
        }
    }

    public async Task Update(Product product)
    {
        var existingProduct = await GetById(product.IdProduct);
        if (existingProduct == null)
            throw new Exception("Продукт не найден.");
        
        context.Products.Entry(existingProduct).CurrentValues.SetValues(product);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var product = await GetById(id);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
    
}