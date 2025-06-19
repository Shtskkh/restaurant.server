using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface IPositionsRepository
{
    Task<List<Position>> GetAll();
    Task<Position?> GetById(int id);
    Task<Position?> GetByTitle(string title);
    Task<RepositoryResult<Position>> Add(string positionTitle);
}

public class PositionsRepository(RestaurantContext context, ILogger<PositionsRepository> logger) : IPositionsRepository
{
    public async Task<List<Position>> GetAll()
    {
        return await context.Positions.AsNoTracking().ToListAsync();
    }

    public async Task<Position?> GetById(int id)
    {
        return await context.Positions.AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPosition == id);
    }

    public async Task<Position?> GetByTitle(string title)
    {
        return await context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Title == title);
    }

    public async Task<RepositoryResult<Position>> Add(string positionTitle)
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
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Ошибка базы данных при добавлении новой должности: {Title}.", positionTitle);
            return RepositoryResult<Position>.Fail("Ошибка базы данных: " + ex.InnerException?.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Непредвиденная ошибка при добавлении новой должности: {Title}.", positionTitle);
            return RepositoryResult<Position>.Fail("Произошла ошибка: " + ex.Message);
        }
    }
}