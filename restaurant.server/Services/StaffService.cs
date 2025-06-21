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
        logger.LogDebug("Начало создания сотрудника с логином: {Login}", dto.Login);

        if (await staffRepository.GetLoginInfoAsync(dto.Login) != null)
        {
            logger.LogWarning("Попытка создать сотрудника с уже существующим логином: {Login}", dto.Login);
            return ServiceResult<int>.Fail("Пользователь с таким логином уже существует.");
        }

        var position = await positionsRepository.GetByTitleAsync(dto.Position);
        if (position == null)
        {
            logger.LogInformation("Должность {Position} не найдена, создаём новую", dto.Position);
            var positionResult = await positionsRepository.AddAsync(dto.Position);

            if (!positionResult.IsSuccess)
            {
                logger.LogError("Не удалось создать должность {Position}: {Error}", dto.Position,
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
            logger.LogError("Не удалось создать сотрудника: {Error}", staffResult.ErrorMessage);
            return ServiceResult<int>.Fail(staffResult.ErrorMessage);
        }

        logger.LogInformation("Сотрудник успешно создан: {Login}, ID: {IdEmployee}", dto.Login,
            staffResult.Data.IdEmployee);

        return ServiceResult<int>.Success(staffResult.Data.IdEmployee);
    }
}