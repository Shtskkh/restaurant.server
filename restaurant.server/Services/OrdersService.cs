using restaurant.server.Controllers;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrderService
{
    Task<List<OrderModel>> GetAll();
    Task<OrderModel?> GetById(int id);
    Task<List<DishInOrderModel>> GetDishesInOrder(int id);
}

public class OrdersService(IOrdersRepository ordersRepository) : IOrderService
{
    public async Task<List<OrderModel>> GetAll()
    {
        var orders = await ordersRepository.GetAll();
        var orderModels = orders.Select(o => new OrderModel
        {
            IdOrder = o.IdOrder,
            Date = o.Date,
            TableNumber = o.IdTableNavigation.Number,
            Status = o.IdStatusNavigation.Title,
            Employee =
                $"{o.IdEmployeeNavigation.LastName} {o.IdEmployeeNavigation.FirstName} {o.IdEmployeeNavigation.MiddleName}"
        });

        return orderModels.ToList();
    }

    public async Task<OrderModel?> GetById(int id)
    {
        var order = await ordersRepository.GetById(id);
        if (order == null)
            return null;
        
        var dishesInOrder = await GetDishesInOrder(id);
        var orderModel = new OrderModel
        {
            IdOrder = order.IdOrder,
            Date = order.Date,
            TableNumber = order.IdTableNavigation.Number,
            Status = order.IdStatusNavigation.Title,
            Employee =
                $"{order.IdEmployeeNavigation.LastName} {order.IdEmployeeNavigation.FirstName} {order.IdEmployeeNavigation.MiddleName}",
            Dishes = dishesInOrder
        };

        return orderModel;
    }

    public async Task<List<DishInOrderModel>> GetDishesInOrder(int id)
    {
        var dishesInOrder = await ordersRepository.GetDishesInOrder(id);
        
        var dishesInOrderModels = dishesInOrder.Select(d => new DishInOrderModel
        {
            IdDish = d.IdDish,
            Title = d.IdDishNavigation.Title,
            Count = d.Count,
            Comment = d.Comment,
            Status = d.IdStatusNavigation.Title,
            TotalCost = d.Count * d.IdDishNavigation.Cost
        });
        
        return dishesInOrderModels.ToList();
    }
}