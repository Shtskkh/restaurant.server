using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrderService
{
    Task<List<OrderModel>> GetAll();
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
}