using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface ITablesRepository
{
    Task<RepositoryResult<List<Table>>> GetAllAsync();
    Task<RepositoryResult<Table>> GetByIdAsync(int id);
    Task<RepositoryResult<Table>> GetByNumberAsync(int number);
}

public class TablesRepository(RestaurantContext context, ILogger<TablesRepository> logger) : ITablesRepository
{
    public async Task<RepositoryResult<List<Table>>> GetAllAsync()
    {
        try
        {
            var tables = await context.Tables.AsNoTracking().ToListAsync();
            return RepositoryResult<List<Table>>.Success(tables);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting tables.");
            return RepositoryResult<List<Table>>.Fail("Error getting tables: " + e.Message);
        }
    }

    public async Task<RepositoryResult<Table>> GetByIdAsync(int id)
    {
        try
        {
            var table = await context.Tables.AsNoTracking().FirstOrDefaultAsync(t => t.IdTable == id);
            if (table != null) return RepositoryResult<Table>.Success(table);

            logger.LogError("Table with ID: {id} not found.", id);
            return RepositoryResult<Table>.Fail($"Table with ID: {id} not found.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting table.");
            return RepositoryResult<Table>.Fail("Error: " + e.Message);
        }
    }

    public async Task<RepositoryResult<Table>> GetByNumberAsync(int number)
    {
        try
        {
            var table = await context.Tables.AsNoTracking().FirstOrDefaultAsync(t => t.Number == number);
            if (table != null) return RepositoryResult<Table>.Success(table);

            logger.LogError("Table with number: {number} not found.", number);
            return RepositoryResult<Table>.Fail($"Table with number: {number} not found.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting table with number: {number}.", number);
            return RepositoryResult<Table>.Fail("Error: " + e.Message);
        }
    }
}