namespace Authorizations.Tests.Infrastructure.Persistence
{
    public class UserServiceTests : BaseClassTests
    {
        #region Private variables

        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<INotificationsGrpcClientService> _notificationsGrpcClientServiceMock;
        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public UserServiceTests(ITestOutputHelper output) : base(output)
        {
            _userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                Array.Empty<IUserValidator<User>>(),
                Array.Empty<IPasswordValidator<User>>(),
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);


            _signInManagerMock = new Mock<SignInManager<User>>(
                _userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<User>>().Object);

            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
            _notificationsGrpcClientServiceMock = new Mock<INotificationsGrpcClientService>(MockBehavior.Strict);

            _userService = new UserService(
                Mapper,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _userRepositoryMock.Object,
                _roleRepositoryMock.Object,
                _notificationsGrpcClientServiceMock.Object
                );
        }

        #endregion

        #region GetAllUsersAsync

        [Fact]
        public async Task GetAllUsersAsync_GetAll_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            dto.Image = null;
            dto.Password = null;
            dto.Token = null;
            dto.IsDefault = false;
            dto.DarkMode = false;


            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            // Act
            List<UserDTO> result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(UserBuilder.UserDTOList(dto));

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllUsersAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.GetAllUsersAsync()).Should()
                .ThrowAsync<Exception>();
        }
        #endregion

        #region GetUserByIdAsync

        [Fact]
        public async Task GetUserByIdAsync_ValidUser_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            // Act
            UserDTO result = await _userService.GetUserByIdAsync(dto.Id);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEmpty();

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUserByIdAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.GetUserByIdAsync(It.IsAny<long>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetUserByIdAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task GetUserByIdAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.GetUserByIdAsync(It.IsAny<long>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetUserByIdAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region GetUserByEmailAsync

        [Fact]
        public async Task GetUserByEmailAsync_ValidUser_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            // Act
            UserDTO result = await _userService.GetUserByEmailAsync(dto.Email);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEmpty();

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetUserByEmailAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetAll())
               .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.GetUserByEmailAsync(It.IsAny<string>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetUserByEmailAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task GetUserByEmailAsync_FindByIdAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.GetUserByEmailAsync(It.IsAny<string>())).Should()
               .ThrowAsync<Exception>()
               .WithMessage($"{DomainResources.GetUserByEmailAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region LoginUserAsync

        [Fact]
        public async Task LoginUserAsync_SignInResultSuccess_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            User user = UserBuilder.User(dto);

            _userManagerMock.Setup(m => m.Users)
                       .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(user)));

            _signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                           .ReturnsAsync(SignInResult.Success);

            // Act
            bool result = await _userService.LoginUserAsync(UserBuilder.UserLoginDTO(dto));

            // Assert
            result.Should().BeTrue();

            _userManagerMock.Verify(repo => repo.Users, Times.Once);
            _signInManagerMock.Verify(repo => repo.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_SignInResultFailed_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            User user = UserBuilder.User(dto);

            _userManagerMock.Setup(m => m.Users)
                       .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(user)));

            _signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                           .ReturnsAsync(SignInResult.Failed);

            // Act
            bool result = await _userService.LoginUserAsync(UserBuilder.UserLoginDTO(dto));

            // Assert
            result.Should().BeFalse();

            _userManagerMock.Verify(repo => repo.Users, Times.Once);
            _signInManagerMock.Verify(repo => repo.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public async Task LoginUserAsync_GetUsersNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _userManagerMock.Setup(m => m.Users).Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.LoginUserAsync(It.IsAny<UserLoginDTO>()))
                .Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task LoginUserAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange
            _userManagerMock.Setup(m => m.Users).Returns(UserBuilder.IQueryableEmpty());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.LoginUserAsync(It.IsAny<UserLoginDTO>()))
                .Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task LoginUserAsync_CheckPasswordSignInAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            User user = UserBuilder.User(dto);

            _userManagerMock.Setup(m => m.Users)
                       .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(user)));

            _signInManagerMock.Setup(m => m.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
                           .Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.LoginUserAsync(It.IsAny<UserLoginDTO>()))
                .Should()
                .ThrowAsync<Exception>();
        }
        #endregion

        #region AddUserAsync

        [Fact]
        public async Task AddUserAsync_ValidUser_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendWelcomeEmailAsync(It.IsAny<SendWelcomeEmailRequestGrpc>())).Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.AddUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();

            _userManagerMock.Verify(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendWelcomeEmailAsync(It.IsAny<SendWelcomeEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddUserAsync_UserAlreadyExists_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {DomainResources.UserAlreadyExistsException}");
        }

        [Fact]
        public async Task AddUserAsync_UserAlreadyExistsNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task AddUserAsync_UpdateUserRoleAsync_RoleNotFoundException()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task AddUserAsync_UpdateUserRoleAsync_FindByIdAsync_RoleNotFoundException()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!
                .ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task AddUserAsync_CreateAsyncFailed_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            Role role = RoleBuilder.Role();

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!
                .ReturnsAsync(role);

            _userManagerMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task AddUserAsync_CreateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task AddUserAsync_SendWelcomeEmailAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendWelcomeEmailAsync(It.IsAny<SendWelcomeEmailRequestGrpc>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.AddUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateUserAsync

        [Fact]
        public async Task UpdateUserAsync_UpdateUser_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>())).Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.UpdateUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().Be(dto.Image);
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Exactly(2));
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Never);
            _roleRepositoryMock.VerifyNoOtherCalls();

            _userManagerMock.Verify(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Once);
            _userManagerMock.Verify(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateDefaultRole_Successfully()
        {
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = true;
            roleDTO.IsReadOnly = true;
            dto.IsDefault = true;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.FullIQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(x => x.GetAll())
               .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(roleDTO)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>())).Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.UpdateUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().Be(dto.Image);
            result.IsDefault.Should().BeTrue();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Exactly(2));
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Never);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();

            _userManagerMock.Verify(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Once);
            _userManagerMock.Verify(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateUserAndPasswordIsNull_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            dto.Password = null;
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>())).Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.UpdateUserAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().Be(dto.Image);
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Exactly(2));
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Never);
            _roleRepositoryMock.VerifyNoOtherCalls();

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(It.IsAny<UserDTO>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task UpdateUserAsync_UserGetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(It.IsAny<UserDTO>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserAsync_UserAlreadyExistsException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            UserDTO userDTO = UserBuilder.UserDTO();
            userDTO.Email = dto.Email;

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.FullIQueryable(userDTO)));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {DomainResources.UserAlreadyExistsException}");
        }

        [Fact]
        public async Task UpdateUserAsync_UserExistsGetAllAnyNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateUserAsync_RolesEmptyRoleNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task UpdateUserAsync_RoleNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task UpdateUserAsync_GeneratePasswordResetTokenAsync_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateUserPasswordAsyncFailed_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);
            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {DomainResources.UpdateUserPasswordAsyncException}");
        }

        [Fact]
        public async Task UpdateUserAsync_ResetPasswordAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);
            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = true;
            roleDTO.IsReadOnly = true;
            dto.IsDefault = true;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.FullIQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(x => x.GetAll())
               .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(roleDTO)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserAsync_SendEmailToUpdatedUserAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.Id = 0;
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            dto.Id = 0;
            dto.IsDefault = false;
            dto.Roles = RoleBuilder.RoleDTOList(roleDTO);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)))
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(roleDTO));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendUserProfileUpdatedEmailAsync(It.IsAny<SendUserProfileUpdatedEmailRequestGrpc>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateUserModeAsync

        [Fact]
        public async Task UpdateUserModeAsync_UpdateMode_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            UserDTO result = await _userService.UpdateUserModeAsync(dto.Id, dto.DarkMode);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserModeAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserModeAsync(dto.Id, dto.DarkMode)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserModeAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task UpdateUserModeAsync_FindByIdAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserModeAsync(dto.Id, dto.DarkMode)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserModeAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserModeAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserModeAsync(dto.Id, dto.DarkMode)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserModeAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateUserImageAsync

        [Fact]
        public async Task UpdateUserImageAsync_UpdateImage_Successfully()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            UserDTO result = await _userService.UpdateUserImageAsync(dto.Id, dto.Image);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().Be(dto.Image);
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserImageAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserImageAsync(dto.Id, dto.Image)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserImageAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task UpdateUserImageAsync_FindByIdAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserImageAsync(dto.Id, dto.Image)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserImageAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserImageAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserImageAsync(dto.Id, dto.Image)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserImageAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateUserPasswordAsync

        [Fact]
        public async Task UpdateUserPasswordAsync_UpdatePassword_Successfully()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendPasswordChangedEmailAsync(It.IsAny<SendPasswordChangedEmailRequestGrpc>()))
                .Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.UpdateUserPasswordAsync(userLoginDTO);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _userManagerMock.Verify(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Once);
            _userManagerMock.Verify(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendPasswordChangedEmailAsync(It.IsAny<SendPasswordChangedEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);

            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_GeneratePasswordResetTokenAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_ResetPasswordAsyncFailed_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserPasswordAsyncException} {DomainResources.UpdateUserPasswordAsyncException}");
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_ResetPasswordAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_SendPasswordChangedEmailAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            UserLoginDTO userLoginDTO = UserBuilder.UserLoginDTO(dto);
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendPasswordChangedEmailAsync(It.IsAny<SendPasswordChangedEmailRequestGrpc>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.UpdateUserPasswordAsync(userLoginDTO)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateUserPasswordAsync

        [Fact]
        public async Task ResetUserPasswordAsync_ResetPassword_Successfully()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(dto.Token!);

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendPasswordResetEmailAsync(It.IsAny<SendPasswordResetEmailRequestGrpc>()))
                .Returns(Task.CompletedTask);

            // Act
            UserDTO result = await _userService.ResetUserPasswordAsync(dto.Email);

            // Assert
            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().BeFalse();
            result.Roles.Should().BeEquivalentTo(dto.Roles);

            _userRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();

            _userManagerMock.Verify(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Once);
            _userManagerMock.Verify(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            _notificationsGrpcClientServiceMock.Verify(repo => repo.SendPasswordResetEmailAsync(It.IsAny<SendPasswordResetEmailRequestGrpc>()), Times.Once);
            _notificationsGrpcClientServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ResetUserPasswordAsync_UserNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.ResetUserPasswordAsync(dto.Email)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.ResetUserPasswordAsyncException} {DomainResources.UserNotFoundException}");
        }

        [Fact]
        public async Task ResetUserPasswordAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();

            _userRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.ResetUserPasswordAsync(dto.Email)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.ResetUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task ResetUserPasswordAsync_UpdateUserPasswordAsyncException_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.ResetUserPasswordAsync(dto.Email)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.ResetUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task ResetUserPasswordAsync_ResetPasswordAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.ResetUserPasswordAsync(dto.Email)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.ResetUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task ResetUserPasswordAsync_SendEmailToUserResetPasswordAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            _userRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<User>(UserBuilder.IQueryable(dto)));

            _userManagerMock.Setup(repo => repo.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            _userManagerMock.Setup(repo => repo.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _notificationsGrpcClientServiceMock.Setup(repo => repo.SendPasswordResetEmailAsync(It.IsAny<SendPasswordResetEmailRequestGrpc>()))
                .Throws(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _userService.ResetUserPasswordAsync(dto.Email)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.ResetUserPasswordAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region DeleteUsersAsync

        [Fact]
        public async Task DeleteUsersAsync_ValidMark_Successfully()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            User user = UserBuilder.User(dto);
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(null, user.Id);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            List<InternalBaseResponseDTO> results = await _userService.DeleteUsersAsync(new List<long> { dto.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteUsersAsync_InvalidUser_UserNotFoundException()
        {
            // Arrange
            string errorMessage = DomainResources.UserNotFoundException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(() => null!);

            // Act
            List<InternalBaseResponseDTO> results = await _userService.DeleteUsersAsync(new List<long> { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteUsersAsync_FindByIdAsync_DeleteUsersAsyncException()
        {
            // Arrange
            string errorMessage = DomainResources.DeleteUsersAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _userService.DeleteUsersAsync(new List<long> { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteUsersAsync_RemoveAsync_DeleteUsersAsyncException()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = 0;
            User user = UserBuilder.User(dto);
            string errorMessage = DomainResources.DeleteUsersAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, user.Id);

            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            _userRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<User>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _userService.DeleteUsersAsync(new List<long> { dto.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        #endregion
    }
}
