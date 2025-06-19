using Microsoft.Extensions.Logging;
using Moq;
using restaurant.server.DTOs;
using restaurant.server.Models;
using restaurant.server.Repositories;
using restaurant.server.Services;
using restaurant.server.Utils;

public class StaffServiceTests
{
    private readonly Mock<ILogger<StaffService>> _loggerMock;
    private readonly Mock<IPositionsRepository> _positionsRepositoryMock;
    private readonly Mock<IStaffRepository> _staffRepositoryMock;
    private readonly StaffService _staffService;

    public StaffServiceTests()
    {
        _staffRepositoryMock = new Mock<IStaffRepository>();
        _positionsRepositoryMock = new Mock<IPositionsRepository>();
        _loggerMock = new Mock<ILogger<StaffService>>();
        _staffService = new StaffService(_staffRepositoryMock.Object, _positionsRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsListOfStaff()
    {
        // Arrange
        var staffList = new List<StaffModel>
        {
            new()
            {
                IdEmployee = 1, Position = "Manager", LastName = "Smith", FirstName = "John",
                PhoneNumber = "+1234567890"
            },
            new()
            {
                IdEmployee = 2, Position = "Waiter", LastName = "Doe", FirstName = "Jane", PhoneNumber = "+0987654321"
            }
        };
        _staffRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(staffList);

        // Act
        var result = await _staffService.GetAll();

        // Assert
        Assert.Equal(staffList, result);
        _staffRepositoryMock.Verify(repo => repo.GetAll(), Times.Once());
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsStaffModel()
    {
        // Arrange
        var id = 1;
        var staffModel = new StaffModel
        {
            IdEmployee = id,
            Position = "Manager",
            LastName = "Smith",
            FirstName = "John",
            PhoneNumber = "+1234567890"
        };
        _staffRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(staffModel);

        // Act
        var result = await _staffService.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(staffModel, result);
        _staffRepositoryMock.Verify(repo => repo.GetById(id), Times.Once());
    }

    [Fact]
    public async Task GetById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var id = 999;
        _staffRepositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync((StaffModel)null);

        // Act
        var result = await _staffService.GetById(id);

        // Assert
        Assert.Null(result);
        _staffRepositoryMock.Verify(repo => repo.GetById(id), Times.Once());
    }

    [Fact]
    public async Task GetAllPositions_ReturnsListOfPositions()
    {
        // Arrange
        var positions = new List<Position>
        {
            new() { IdPosition = 1, Title = "Manager", Staff = new List<Staff>() },
            new() { IdPosition = 2, Title = "Waiter", Staff = new List<Staff>() }
        };
        _positionsRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(positions);

        // Act
        var result = await _staffService.GetAllPositions();

        // Assert
        Assert.Equal(positions, result);
        _positionsRepositoryMock.Verify(repo => repo.GetAll(), Times.Once());
    }

    [Fact]
    public async Task Add_ValidDtoWithNewPosition_ReturnsSuccess()
    {
        // Arrange
        var dto = new CreateStaffModel
        {
            LastName = "Smith",
            FirstName = "John",
            PhoneNumber = "+1234567890",
            Login = "jsmith",
            Password = "P@ssw0rd123",
            Position = "Manager"
        };
        var position = new Position { IdPosition = 1, Title = "Manager", Staff = new List<Staff>() };
        var staff = new Staff
        {
            IdEmployee = 1,
            IdPosition = 1,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            PhoneNumber = dto.PhoneNumber,
            Login = dto.Login,
            Password = dto.Password,
            IdPositionNavigation = position,
            Orders = new List<Order>()
        };

        _staffRepositoryMock.Setup(repo => repo.GetLoginInfo(dto.Login)).ReturnsAsync((Staff)null);
        _positionsRepositoryMock.Setup(repo => repo.GetByTitle(dto.Position)).ReturnsAsync((Position)null);
        _positionsRepositoryMock.Setup(repo => repo.Add(dto.Position))
            .ReturnsAsync(RepositoryResult<Position>.Success(position));
        _staffRepositoryMock.Setup(repo => repo.Add(It.Is<Staff>(s =>
                s.LastName == dto.LastName &&
                s.FirstName == dto.FirstName &&
                s.PhoneNumber == dto.PhoneNumber &&
                s.Login == dto.Login &&
                s.Password == dto.Password &&
                s.IdPosition == position.IdPosition)))
            .ReturnsAsync(RepositoryResult<Staff>.Success(staff));

        // Act
        var result = await _staffService.Add(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(staff.IdEmployee, result.Data);
        Assert.Null(result.ErrorMessage);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(dto.Login), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.GetByTitle(dto.Position), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.Add(dto.Position), Times.Once());
        _staffRepositoryMock.Verify(repo => repo.Add(It.IsAny<Staff>()), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Information, $"Сотрудник успешно создан: {dto.Login}, ID: {staff.IdEmployee}",
            Times.Once());
    }

    [Fact]
    public async Task Add_ExistingLogin_ReturnsFailure()
    {
        // Arrange
        var dto = new CreateStaffModel
        {
            LastName = "Smith",
            FirstName = "John",
            PhoneNumber = "+1234567890",
            Login = "jsmith",
            Password = "P@ssw0rd123",
            Position = "Manager"
        };
        var existingStaff = new Staff { IdEmployee = 1, Login = dto.Login };

        _staffRepositoryMock.Setup(repo => repo.GetLoginInfo(dto.Login)).ReturnsAsync(existingStaff);

        // Act
        var result = await _staffService.Add(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Пользователь с таким логином уже существует.", result.ErrorMessage);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(dto.Login), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.GetByTitle(It.IsAny<string>()), Times.Never());
        _positionsRepositoryMock.Verify(repo => repo.Add(It.IsAny<string>()), Times.Never());
        _staffRepositoryMock.Verify(repo => repo.Add(It.IsAny<Staff>()), Times.Never());
        _loggerMock.VerifyLog(LogLevel.Warning, $"Попытка создать сотрудника с уже существующим логином: {dto.Login}",
            Times.Once());
    }

    [Fact]
    public async Task Add_PositionCreationFails_ReturnsFailure()
    {
        // Arrange
        var dto = new CreateStaffModel
        {
            LastName = "Smith",
            FirstName = "John",
            PhoneNumber = "+1234567890",
            Login = "jsmith",
            Password = "P@ssw0rd123",
            Position = "Manager"
        };

        _staffRepositoryMock.Setup(repo => repo.GetLoginInfo(dto.Login)).ReturnsAsync((Staff)null);
        _positionsRepositoryMock.Setup(repo => repo.GetByTitle(dto.Position)).ReturnsAsync((Position)null);
        _positionsRepositoryMock.Setup(repo => repo.Add(dto.Position))
            .ReturnsAsync(RepositoryResult<Position>.Fail("Failed to create position"));

        // Act
        var result = await _staffService.Add(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create position", result.ErrorMessage);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(dto.Login), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.GetByTitle(dto.Position), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.Add(dto.Position), Times.Once());
        _staffRepositoryMock.Verify(repo => repo.Add(It.IsAny<Staff>()), Times.Never());
        _loggerMock.VerifyLog(LogLevel.Error, $"Не удалось создать должность {dto.Position}: Failed to create position",
            Times.Once());
    }

    [Fact]
    public async Task Add_StaffCreationFails_ReturnsFailure()
    {
        // Arrange
        var dto = new CreateStaffModel
        {
            LastName = "Smith",
            FirstName = "John",
            PhoneNumber = "+1234567890",
            Login = "jsmith",
            Password = "P@ssw0rd123",
            Position = "Manager"
        };
        var position = new Position { IdPosition = 1, Title = "Manager", Staff = new List<Staff>() };

        _staffRepositoryMock.Setup(repo => repo.GetLoginInfo(dto.Login)).ReturnsAsync((Staff)null);
        _positionsRepositoryMock.Setup(repo => repo.GetByTitle(dto.Position)).ReturnsAsync(position);
        _staffRepositoryMock.Setup(repo => repo.Add(It.IsAny<Staff>()))
            .ReturnsAsync(RepositoryResult<Staff>.Fail("Failed to create staff"));

        // Act
        var result = await _staffService.Add(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to create staff", result.ErrorMessage);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(dto.Login), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.GetByTitle(dto.Position), Times.Once());
        _positionsRepositoryMock.Verify(repo => repo.Add(It.IsAny<string>()), Times.Never());
        _staffRepositoryMock.Verify(repo => repo.Add(It.IsAny<Staff>()), Times.Once());
        _loggerMock.VerifyLog(LogLevel.Error, "Не удалось создать сотрудника: Failed to create staff", Times.Once());
    }
}

public static class LoggerExtensions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times)
    {
        loggerMock.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(message)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            times);
    }
}