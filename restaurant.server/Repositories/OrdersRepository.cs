﻿using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;

namespace restaurant.server.Repositories;

public interface IOrdersRepository
{
    Task<List<Order>> GetAll();
    Task<Order?> GetById(int id);
    Task Add(Order order);
    Task Update(Order order);
    Task Delete(int id);
}

public class OrdersRepository(RestaurantContext context) : IOrdersRepository
{
    public async Task<List<Order>> GetAll()
    {
        return await context.Orders.AsNoTracking().ToListAsync();
    }

    public async Task<Order?> GetById(int id)
    {
        return await context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.IdOrder == id);
    }

    public async Task Add(Order order)
    {
        try
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Не удалось добавить заказ");
        }
    }

    public async Task Update(Order order)
    {
        var existingOrder = await GetById(order.IdOrder);
        if (existingOrder == null)
            throw new Exception("Заказ не найден.");
        
        context.Orders.Entry(order).CurrentValues.SetValues(order);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var order = await GetById(id);
        if (order != null)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
}