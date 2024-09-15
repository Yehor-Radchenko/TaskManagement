namespace TaskManagement.Tests.Services;

using Xunit;
using Moq;
using FluentAssertions;
using TaskManagement.BLL.Services;
using TaskManagement.Common.Dto.User;
using TaskManagement.Common.Exceptions;
using TaskManagement.DAL.Models;
using TaskManagement.DAL.UoW;
using AutoMapper;
using System;
using System.Threading.Tasks;
using TaskManagement.BLL.Services.IService;
using TaskManagement.DAL.Repositories.IRepository;
using System.Linq.Expressions;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userRepoMock = new Mock<IGenericRepository<User>>();

        _unitOfWorkMock.Setup(u => u.GetRepository<User>()).Returns(_userRepoMock.Object);
        _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterAsync_ThrowsArgumentNullException_WhenDtoIsNull()
    {
        // Arrange
        UserRegistrationDto dto = null!;

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.RegisterAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task RegisterAsync_ThrowsConflictException_WhenUserExists()
    {
        // Arrange
        var dto = new UserRegistrationDto { Email = "test@example.com", Username = "testuser", Password = "password" };
        _userRepoMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(true);

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.RegisterAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }


    [Fact]
    public async System.Threading.Tasks.Task RegisterAsync_CreatesUserSuccessfully()
    {
        // Arrange
        var dto = new UserRegistrationDto { Email = "test@example.com", Username = "testuser", Password = "password" };
        var user = new User { Email = dto.Email, Username = dto.Username };
        _mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);

        _userRepoMock.Setup(repo => repo.ExistsAsync(It.Is<Expression<Func<User, bool>>>(expr =>
            expr.Compile()(new User { Email = dto.Email, Username = dto.Username }))))
            .ReturnsAsync(false);

        // Act
        var result = await _userService.RegisterAsync(dto);

        // Assert
        result.Should().BeTrue();
        _userRepoMock.Verify(repo => repo.Add(It.Is<User>(u => u.Email == dto.Email && u.Username == dto.Username)), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
    }

    [Fact]
    public async System.Threading.Tasks.Task ChangePasswordAsync_ThrowsArgumentNullException_WhenDtoIsNull()
    {
        // Arrange
        ChangePasswordDto dto = null!;

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.ChangePasswordAsync(Guid.NewGuid(), dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task FindUserAsync_ThrowsArgumentNullException_WhenLoginIsNull()
    {
        // Arrange
        string login = null!;

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.FindUserAsync(login);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task ChangePasswordAsync_ThrowsKeyNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new ChangePasswordDto { Password = "newpassword" };
        _userRepoMock.Setup(repo => repo.GetAsync(It.Is<Expression<Func<User, bool>>>(expr => expr.ToString().Contains(userId.ToString()))))
                     .ReturnsAsync((User)null!);

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.ChangePasswordAsync(userId, dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async System.Threading.Tasks.Task FindUserAsync_ThrowsNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var login = "nonexistentuser";
        _userRepoMock.Setup(repo => repo.GetAsync(It.Is<Expression<Func<User, bool>>>(expr => expr.ToString().Contains(login))))
                     .ReturnsAsync((User)null!);

        // Act
        Func<System.Threading.Tasks.Task> act = async () => await _userService.FindUserAsync(login);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
