using Moq;
using restaurant.server.Models;
using restaurant.server.Repositories;
using restaurant.server.Services;

namespace restaurant.server.test;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IStaffRepository> _staffRepositoryMock;

    public AuthServiceTests()
    {
        _staffRepositoryMock = new Mock<IStaffRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _authService = new AuthService(_staffRepositoryMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Authenticate_ValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        var login = "testuser";
        var password = "testpass";
        var expectedToken = "jwt_token";
        var staff = new Staff
        {
            IdEmployee = 1,
            IdPosition = 1,
            LastName = "Doe",
            FirstName = "John",
            PhoneNumber = "1234567890",
            Login = login,
            Password = password
        };

        _staffRepositoryMock
            .Setup(repo => repo.GetLoginInfo(login))
            .ReturnsAsync(staff);
        _jwtServiceMock
            .Setup(jwt => jwt.GenerateJwtToken())
            .Returns(expectedToken);

        // Act
        var result = await _authService.Authenticate(login, password);

        // Assert
        Assert.Equal(expectedToken, result);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(login), Times.Once());
        _jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(), Times.Once());
    }

    [Fact]
    public async Task Authenticate_NullStaff_ReturnsNull()
    {
        // Arrange
        var login = "testuser";
        var password = "testpass";

        _staffRepositoryMock
            .Setup(repo => repo.GetLoginInfo(login))
            .ReturnsAsync((Staff)null);

        // Act
        var result = await _authService.Authenticate(login, password);

        // Assert
        Assert.Null(result);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(login), Times.Once());
        _jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(), Times.Never());
    }

    [Fact]
    public async Task Authenticate_WrongPassword_ReturnsNull()
    {
        // Arrange
        var login = "testuser";
        var password = "wrongpass";
        var staff = new Staff
        {
            IdEmployee = 1,
            IdPosition = 1,
            LastName = "Doe",
            FirstName = "John",
            PhoneNumber = "1234567890",
            Login = login,
            Password = "correctpass"
        };

        _staffRepositoryMock
            .Setup(repo => repo.GetLoginInfo(login))
            .ReturnsAsync(staff);

        // Act
        var result = await _authService.Authenticate(login, password);

        // Assert
        Assert.Null(result);
        _staffRepositoryMock.Verify(repo => repo.GetLoginInfo(login), Times.Once());
        _jwtServiceMock.Verify(jwt => jwt.GenerateJwtToken(), Times.Never());
    }

    [Theory]
    [InlineData(null, "password")]
    [InlineData("", "password")]
    [InlineData("login", null)]
    [InlineData("login", "")]
    public async Task Authenticate_NullOrEmptyInputs_ReturnsNull(string login, string password)
    {
        // Act
        var result = await _authService.Authenticate(login, password);

        // Assert
        Assert.Null(result);
    }
}