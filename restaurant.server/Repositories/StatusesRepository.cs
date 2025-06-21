using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Models;
using restaurant.server.Utils;

namespace restaurant.server.Repositories;

public interface IStatusesRepository
{
    Task<RepositoryResult<List<Status>>> GetAllAsync();
    Task<RepositoryResult<Status>> GetByIdAsync(int id);
    Task<RepositoryResult<Status>> GetByTitleAsync(string status);
}

public class StatusesRepository(RestaurantContext context, ILogger<IStatusesRepository> logger) : IStatusesRepository
{
    public async Task<RepositoryResult<List<Status>>> GetAllAsync()
    {
        try
        {
            var statuses = await context.Statuses.AsNoTracking().ToListAsync();
            return RepositoryResult<List<Status>>.Success(statuses);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting statuses.");
            return RepositoryResult<List<Status>>.Fail("Error: " + ex.Message);
        }
    }

    public async Task<RepositoryResult<Status>> GetByIdAsync(int id)
    {
        try
        {
            var status = await context.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.IdStatus == id);
            if (status != null) return RepositoryResult<Status>.Success(status);

            logger.LogError("Status with ID: {Id} not found.", id);
            return RepositoryResult<Status>.Fail($"Status with ID: {id} not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting status.");
            return RepositoryResult<Status>.Fail("Error: " + ex.Message);
        }
    }

    public async Task<RepositoryResult<Status>> GetByTitleAsync(string title)
    {
        try
        {
            var status = await context.Statuses.AsNoTracking().FirstOrDefaultAsync(s => s.Title == title);
            if (status != null) return RepositoryResult<Status>.Success(status);

            logger.LogError("Status with title: {title} not found.", title);
            return RepositoryResult<Status>.Fail($"Status with title: {title} not found.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting status.");
            return RepositoryResult<Status>.Fail("Error: " + ex.Message);
        }
    }
}