using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface IPositionsRepository
{
    Task<List<Position>> GetAllAsync();
    Task<Position?> GetByIdAsync(int id);
    Task<Position?> GetByTitleAsync(string title);
    Task<RepositoryResult<Position>> AddAsync(string positionTitle);
}

public class PositionsRepository(RestaurantContext context, ILogger<PositionsRepository> logger) : IPositionsRepository
{
    public async Task<List<Position>> GetAllAsync()
    {
        return await context.Positions.AsNoTracking().ToListAsync();
    }

    public async Task<Position?> GetByIdAsync(int id)
    {
        return await context.Positions.AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPosition == id);
    }

    public async Task<Position?> GetByTitleAsync(string title)
    {
        return await context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Title == title);
    }

    public async Task<RepositoryResult<Position>> AddAsync(string positionTitle)
    {
        try
        {
            var newPosition = new Position
            {
                Title = positionTitle
            };

            await context.Positions.AddAsync(newPosition);
            await context.SaveChangesAsync();
            return RepositoryResult<Position>.Success(newPosition);
        }
        catch (DbUpdateException e)
        {
            logger.LogError(e, "Database error when adding a new position: {Title}.", positionTitle);
            return RepositoryResult<Position>.Fail("Database error: " + e.InnerException?.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error when adding a new position: {Title}.", positionTitle);
            return RepositoryResult<Position>.Fail("Error: " + e.Message);
        }
    }
}