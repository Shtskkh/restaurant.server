using restaurant.server.DTOs;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IOrdersService
{
    Task<List<OrderModel>> GetAll();
    Task<OrderModel?> GetById(int id);
}

public class OrdersService(
    OrdersRepository ordersRepository,
    TablesRepository tablesRepository,
    StatusesRepository statusesRepository,
    StaffService staffService
) : IOrdersService
{
    public async Task<List<OrderModel>> GetAll()
    {
        var orders = await ordersRepository.GetAll();
        var statuses = await statusesRepository.GetAll();
        var tables = await tablesRepository.GetAll();

        var orderModels = from o in orders
            join s in statuses on o.IdStatus equals s.IdStatus
            join t in tables on o.IdTable equals t.IdTable
            select new OrderModel
            {
                IdOrder = o.IdOrder,
                Date = o.Date,
                TableNumber = t.Number,
                Status = s.Title,
            };

        return orderModels.ToList();
    }

    public async Task<OrderModel?> GetById(int id)
    {
        var order = await ordersRepository.GetById(id);
        if (order == null) return null;
        var status = await statusesRepository.GetById(order.IdStatus);
        var table = await tablesRepository.GetById(order.IdTable);
        var employee = await staffService.GetById(order.IdEmployee);
        var orderModel = new OrderModel
        {
            IdOrder = order.IdOrder,
            Date = order.Date,
            TableNumber = table.Number,
            Status = status.Title,
            Employee = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}"
        };
        
        return orderModel;
    }
}