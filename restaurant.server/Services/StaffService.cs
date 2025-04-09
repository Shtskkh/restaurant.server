using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;

namespace restaurant.server.Services;

public interface IStaffService
{
    Task<List<StaffModel>> GetAll();
    Task<StaffModel?> GetById(int id);
    Task<StaffModel?> GetByLogin(string login);
    Task<List<StaffModel>> GetByPosition(string pos);
}

public class StaffService(IStaffRepository staffRepository, IPositionsRepository positionsRepository) : IStaffService
{
    public async Task<List<StaffModel>> GetAll()
    {
        var staff = await staffRepository.GetAll();
        var positions = await positionsRepository.GetAll();
        var staffModels = from s in staff
            join p in positions on s.IdPosition equals p.IdPosition
            select new StaffModel
            {
                IdEmployee = s.IdEmployee,
                IdPosition = s.IdPosition,
                Position = p!.Title,
                LastName = s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                PhoneNumber = s.PhoneNumber,
            };

        return staffModels.ToList();
    }

    public async Task<StaffModel?> GetById(int id)
    {
        var staff = await staffRepository.GetById(id);
        if (staff == null) return null;
        var position = await positionsRepository.GetById(staff.IdPosition);
        var staffModel = new StaffModel
        {
            IdEmployee = staff.IdEmployee,
            IdPosition = staff.IdPosition,
            Position = position!.Title,
            LastName = staff.LastName,
            FirstName = staff.FirstName,
            MiddleName = staff.MiddleName,
            PhoneNumber = staff.PhoneNumber,
        };

        return staffModel;
    }

    public async Task<StaffModel?> GetByLogin(string login)
    {
        var staff = await staffRepository.GetByLogin(login);
        if (staff == null) return null;
        var position = await positionsRepository.GetById(staff.IdPosition);
        var staffModel = new StaffModel
        {
            IdEmployee = staff.IdEmployee,
            IdPosition = staff.IdPosition,
            Position = position!.Title,
            LastName = staff.LastName,
            FirstName = staff.FirstName,
            MiddleName = staff.MiddleName,
            PhoneNumber = staff.PhoneNumber,
        };

        return staffModel;
    }

    public async Task<List<StaffModel>> GetByPosition(string pos)
    {
        var position = await positionsRepository.GetByTitle(pos);
        var staff = await staffRepository.GetByPosition(position.IdPosition);
        var staffModels = from s in staff
            select new StaffModel
            {
                IdEmployee = s.IdEmployee,
                IdPosition = s.IdPosition,
                Position = position.Title,
                LastName = s.LastName,
                FirstName = s.FirstName,
                MiddleName = s.MiddleName,
                PhoneNumber = s.PhoneNumber,
            };
        return staffModels.ToList();
    }
}