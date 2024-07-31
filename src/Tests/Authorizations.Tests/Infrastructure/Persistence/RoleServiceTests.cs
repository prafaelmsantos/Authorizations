namespace Authorizations.Tests.Infrastructure.Persistence
{
    public class RoleServiceTests : BaseClassTests
    {
        #region Private variables

        private readonly Mock<IRoleRepository> _roleRepositoryMock;
        private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IRoleService _roleService;

        #endregion

        #region Constructors

        public RoleServiceTests(ITestOutputHelper output) : base(output)
        {
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
            _userRoleRepositoryMock = new Mock<IUserRoleRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _roleService = new RoleService(Mapper, _roleRepositoryMock.Object, _userRoleRepositoryMock.Object, _userRepositoryMock.Object);
        }

        #endregion

        #region GetAllRolesAsync

        [Fact]
        public async Task GetAllRolesAsync_GetAll_Successfully()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.IsReadOnly = false;
            dto.IsDefault = false;
            dto.Id = 0;

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryable(dto)));

            // Act
            List<RoleDTO> result = await _roleService.GetAllRolesAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(RoleBuilder.RoleDTOList(dto));

            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllRolesAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.GetAllRolesAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetAllRolesAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }
        #endregion

        #region GetRoleByIdAsync

        [Fact]
        public async Task GetRoleByIdAsync_ValidRole_Successfully()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(dto));

            // Act
            RoleDTO result = await _roleService.GetRoleByIdAsync(dto.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.IsReadOnly.Should().BeFalse();
            result.IsDefault.Should().BeFalse();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetRoleByIdAsync_RoleNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.GetRoleByIdAsync(It.IsAny<long>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetRoleByIdAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task GetRoleByIdAsync_FindByIdAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.GetRoleByIdAsync(It.IsAny<long>())).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.GetRoleByIdAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region AddRoleAsync

        [Fact]
        public async Task AddRoleAsync_ValidRole_Successfully()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Role>())).ReturnsAsync(RoleBuilder.Role(dto));

            // Act
            RoleDTO result = await _roleService.AddRoleAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.IsReadOnly.Should().BeFalse();
            result.IsDefault.Should().BeFalse();

            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddRoleAsync_RoleAlreadyExists_ThrowsExceptionAsync()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryable(dto)));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.AddRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddRoleAsyncException} {DomainResources.RoleAlreadyExistsException}");
        }

        [Fact]
        public async Task AddRoleAsync_AddAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Role>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.AddRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.AddRoleAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateRoleAsync

        [Fact]
        public async Task UpdateRoleAsync_ValidRole_Successfully()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();
            Role role = RoleBuilder.Role(dto);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Role>())).ReturnsAsync(role);

            // Act
            RoleDTO result = await _roleService.UpdateRoleAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.IsReadOnly.Should().BeFalse();
            result.IsDefault.Should().BeFalse();

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.UpdateRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateRoleAsyncException} {DomainResources.RoleNotFoundException}");
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleAlreadyExistsException_ThrowsExceptionAsync()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(RoleBuilder.Role(dto));

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryable(dto)));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.UpdateRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateRoleAsyncException} {DomainResources.RoleAlreadyExistsException}");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task UpdateRoleAsync_RoleIsReadOnlyException_ThrowsExceptionAsync(bool isDefault, bool isReadOnly)
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.IsDefault = isDefault;
            dto.IsReadOnly = isReadOnly;

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(RoleBuilder.FullRole(dto));

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.UpdateRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateRoleAsyncException} {DomainResources.UpdateDefaultRoleException}");
        }

        [Fact]
        public async Task UpdateMarkAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            RoleDTO dto = RoleBuilder.RoleDTO();

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(RoleBuilder.Role(dto));

            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            _roleRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _roleService.UpdateRoleAsync(dto)).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResources.UpdateRoleAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region DeleteRolesAsync

        [Fact]
        public async Task DeleteRolesAsync_ValidRole_Successfully()
        {
            // Arrange
            UserDTO userDTO = UserBuilder.UserDTO();
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;

            userRole.RoleId = roleDTO.Id;

            RoleDTO defaultRoleDTO = RoleBuilder.RoleDTO();
            defaultRoleDTO.IsDefault = true;
            defaultRoleDTO.IsReadOnly = false;

            Role role = RoleBuilder.FullRole(roleDTO);
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(null, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(defaultRoleDTO)));
            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(UserBuilder.User(userDTO));
            _userRoleRepositoryMock.Setup(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(UserBuilder.User(userDTO));

            _roleRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<Role>())).ReturnsAsync(true);

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<Role>()), Times.Once);

            _userRoleRepositoryMock.Verify(repo => repo.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>()), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteMarksAsync_InvalidRole_RoleNotFoundException()
        {
            // Arrange
            string errorMessage = DomainResources.RoleNotFoundException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(() => null!);

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Never);

            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Never);

            _roleRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<Role>()), Times.Never);

            _userRoleRepositoryMock.Verify(repo => repo.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>()), Times.Never);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Never);

            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public async Task DeleteMarksAsync_RoleIsDefaultOrIsReadOnly_DeleteDefaultRoleAsyncException(bool isDefault, bool isReadOnly)
        {
            // Arrange
            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = isDefault;
            roleDTO.IsReadOnly = isReadOnly;

            Role role = RoleBuilder.FullRole(roleDTO);

            string errorMessage = DomainResources.DeleteDefaultRoleAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(role);

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteMarksAsync_FindByIdAsync_DeleteRolesAsyncException()
        {
            // Arrange
            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long>() { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteRolesAsync_RemoveRoleAsync_DeleteRolesAsyncException()
        {
            // Arrange
            UserDTO userDTO = UserBuilder.UserDTO();
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;
            Role role = RoleBuilder.FullRole(roleDTO);

            userRole.RoleId = roleDTO.Id;

            RoleDTO defaultRoleDTO = RoleBuilder.RoleDTO();
            defaultRoleDTO.IsDefault = true;
            defaultRoleDTO.IsReadOnly = false;

            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(defaultRoleDTO)));
            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(UserBuilder.User(userDTO));
            _userRoleRepositoryMock.Setup(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(UserBuilder.User(userDTO));

            _roleRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<Role>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<Role>()), Times.Once);

            _userRoleRepositoryMock.Verify(repo => repo.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>()), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteRolesAsync_UpdateUserWithDefaultRole_DefaultRoleNotFound_DeleteRolesAsyncException()
        {
            // Arrange
            UserDTO userDTO = UserBuilder.UserDTO();
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;

            Role role = RoleBuilder.FullRole(roleDTO);

            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.IQueryableEmpty()));

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteRolesAsync_UpdateUserWithDefaultRole_UserNotFound_DeleteRolesAsyncException()
        {
            // Arrange
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;

            Role role = RoleBuilder.FullRole(roleDTO);

            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(roleDTO)));
            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))!.ReturnsAsync(() => null!);


            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteRolesAsync_UpdateUserWithDefaultRole_RemoveRangeAsync_DeleteRolesAsyncException()
        {
            // Arrange
            UserDTO userDTO = UserBuilder.UserDTO();
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;

            userRole.RoleId = roleDTO.Id;

            RoleDTO defaultRoleDTO = RoleBuilder.RoleDTO();
            defaultRoleDTO.IsDefault = true;
            defaultRoleDTO.IsReadOnly = false;

            Role role = RoleBuilder.FullRole(roleDTO);
            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(defaultRoleDTO)));
            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(UserBuilder.User(userDTO));
            _userRoleRepositoryMock.Setup(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<Role>()), Times.Never);

            _userRoleRepositoryMock.Verify(repo => repo.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>()), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteRolesAsync_UpdateUserWithDefaultRole_UpdateAsync_DeleteRolesAsyncException()
        {
            // Arrange
            UserDTO userDTO = UserBuilder.UserDTO();
            UserRole userRole = UserRoleBuilder.UserRole();

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            roleDTO.IsDefault = false;
            roleDTO.IsReadOnly = false;

            userRole.RoleId = roleDTO.Id;

            RoleDTO defaultRoleDTO = RoleBuilder.RoleDTO();
            defaultRoleDTO.IsDefault = true;
            defaultRoleDTO.IsReadOnly = false;

            Role role = RoleBuilder.FullRole(roleDTO);
            string errorMessage = DomainResources.DeleteRolesAsyncException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseBuilder.InternalBaseResponseDTOList(errorMessage, role.Id);

            _roleRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(role);

            //UpdateUserWithDefaultRole
            _userRoleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<UserRole>(UserRoleBuilder.IQueryable(userRole.UserId, userRole.RoleId)));
            _roleRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Role>(RoleBuilder.FullIQueryable(defaultRoleDTO)));
            _userRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(UserBuilder.User(userDTO));
            _userRoleRepositoryMock.Setup(x => x.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>())).ThrowsAsync(new Exception());

            _roleRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<Role>())).ReturnsAsync(true);

            // Act
            List<InternalBaseResponseDTO> results = await _roleService.DeleteRolesAsync(new List<long> { roleDTO.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _roleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _roleRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _userRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);

            _userRoleRepositoryMock.Verify(repo => repo.RemoveRangeAsync(It.IsAny<IEnumerable<UserRole>>()), Times.Once);
            _userRoleRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);

            _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);

            _roleRepositoryMock.VerifyNoOtherCalls();
        }

        #endregion
    }
}
