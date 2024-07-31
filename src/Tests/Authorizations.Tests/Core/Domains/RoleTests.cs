namespace Authorizations.Tests.Core.Domains
{
    public class RoleTests : BaseClassTests
    {
        public RoleTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();

            // Act
            Role role = RoleBuilder.Role(dto);

            // Assert
            role.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            role.NormalizedName.Should().Be(dto.Name.ToUpper()).And.NotBeNullOrWhiteSpace();
            role.IsReadOnly.Should().BeFalse();
            role.IsDefault.Should().BeFalse();
            role.Users.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithValidFullParameters_InitializesProperties()
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();

            // Act
            Role role = RoleBuilder.FullRole(dto);

            // Assert
            role.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            role.NormalizedName.Should().Be(dto.Name.ToUpper()).And.NotBeNullOrWhiteSpace();
            role.IsReadOnly.Should().Be(dto.IsReadOnly);
            role.IsDefault.Should().Be(dto.IsDefault);
            role.Users.Should().BeEmpty();
        }

        [Fact]
        public void TestMap_InitializesProperties()
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.IsReadOnly = false;
            dto.IsDefault = false;
            Role role = RoleBuilder.Role(dto);

            // Act
            RoleDTO result = Mapper.Map<RoleDTO>(role);

            // Assert
            result.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            result.IsReadOnly.Should().Be(dto.IsReadOnly);
            result.IsDefault.Should().Be(dto.IsDefault);
        }

        [Fact]
        public void TestMap_InitializesFullProperties()
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            Role role = RoleBuilder.FullRole(dto);

            // Act
            RoleDTO result = Mapper.Map<RoleDTO>(role);

            // Assert
            result.Id.Should().Be(dto.Id).And.BeGreaterThan(0);
            result.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            result.IsReadOnly.Should().Be(dto.IsReadOnly);
            result.IsDefault.Should().Be(dto.IsDefault);
            role.Users.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void FullConstructor_WithInvalidId_ThrowsArgumentException(int id)
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.Id = id;

            // Act & Assert
            FluentActions.Invoking(() => RoleBuilder.FullRole(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.RoleIdNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void FullConstructor_WithInvalidName_ThrowsArgumentException(string? name)
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.Name = name!;

            // Act & Assert
            FluentActions.Invoking(() => RoleBuilder.FullRole(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.RoleNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ThrowsArgumentException(string? name)
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            dto.Name = name!;

            // Act & Assert
            FluentActions.Invoking(() => RoleBuilder.Role(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.RoleNameNeedsToBeSpecifiedException);
        }

        [Fact]
        public void SetName_WithValidParameters()
        {
            // Arrange
            RoleDTO dto = RoleBuilder.RoleDTO();
            Role role = RoleBuilder.Role(dto);

            // Act
            role.SetName(dto.Name);

            // Assert
            role.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SetName_WithInvalidName_ThrowsArgumentException(string? name)
        {
            // Arrange
            Role role = RoleBuilder.Role();

            // Act & Assert
            FluentActions.Invoking(() => role.SetName(name!)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.RoleNameNeedsToBeSpecifiedException);
        }
    }
}
