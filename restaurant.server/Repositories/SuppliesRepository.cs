using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.DTOs;

namespace restaurant.server.Repositories;

public interface ISuppliesRepository
{
    Task<List<SupplyModel>> GetAllAsync();
}

public class SuppliesRepository(RestaurantContext context) : ISuppliesRepository
{
    public async Task<List<SupplyModel>> GetAllAsync()
    {
        var suppliesModels =
            from supply in context.Supplies.AsNoTracking()
            join supplier in context.Suppliers.AsNoTracking()
                on supply.IdSupplier equals supplier.IdSupplier
            join product in context.Products.AsNoTracking()
                on supply.IdProduct equals product.IdProduct
            join unit in context.MeasureUnits.AsNoTracking()
                on supply.IdUnit equals unit.IdUnit
            select new SupplyModel
            {
                Product = product.Title,
                Count = supply.Count,
                Date = supply.Date,
                Unit = unit.Title,
                Supplier = supplier.Title
            };

        return await suppliesModels.ToListAsync();
    }
}