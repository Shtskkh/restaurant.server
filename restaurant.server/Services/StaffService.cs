using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;
using restaurant.server.Utils;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAllAsync();
    Task<StaffModel?> GetByIdAsync(int id);
    Task<List<Position>> GetAllPositionsAsync();
    Task<ServiceResult<int>> AddAsync(AddStaffModel dto);
}

public class StaffService(
    IStaffRepository staffRepository,
    IPositionsRepository positionsRepository,
    ILogger<StaffService> logger) : IStaffService
{
    public async Task<List<StaffModel>> GetAllAsync()
    {
        return await staffRepository.GetAllAsync();
    }

    public async Task<StaffModel?> GetByIdAsync(int id)
    {
        var staffModel = await staffRepository.GetByIdAsync(id);
        return staffModel ?? null;
    }

    public async Task<List<Position>> GetAllPositionsAsync()
    {
        return await positionsRepository.GetAllAsync();
    }

    public async Task<ServiceResult<int>> AddAsync(AddStaffModel dto)
    {
        logger.LogDebug("Starting to add employee with login: {login}...", dto.Login);

        if (await staffRepository.GetLoginInfoAsync(dto.Login) != null)
        {
            logger.LogWarning("An attempt to create an employee with an existing login: {login}", dto.Login);
            return ServiceResult<int>.Fail("The user with this username already exists.");
        }

        var position = await positionsRepository.GetByTitleAsync(dto.Position);
        if (position == null)
        {
            logger.LogInformation("Position {position} not found, creating a new one.", dto.Position);
            var positionResult = await positionsRepository.AddAsync(dto.Position);

            if (!positionResult.IsSuccess)
            {
                logger.LogError("Couldn't create a position with title {position}: {error}.", dto.Position,
                    positionResult.ErrorMessage);
                return ServiceResult<int>.Fail(positionResult.ErrorMessage);
            }

            position = positionResult.Data;
        }

        var staff = new Staff
        {
            IdPosition = position!.IdPosition,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            PhoneNumber = dto.PhoneNumber,
            Login = dto.Login,
            Password = dto.Password
        };

        var staffResult = await staffRepository.AddAsync(staff);
        if (!staffResult.IsSuccess)
        {
            logger.LogError("Couldn't create an employee: {error}", staffResult.ErrorMessage);
            return ServiceResult<int>.Fail(staffResult.ErrorMessage);
        }

        logger.LogInformation("The employee with the login: {login} has been successfully created, ID: {IdEmployee}",
            dto.Login,
            staffResult.Data.IdEmployee);

        return ServiceResult<int>.Success(staffResult.Data.IdEmployee);
    }
}