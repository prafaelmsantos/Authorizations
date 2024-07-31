namespace Authorizations.Tests.Core.Domains
{
    public class UserTests : BaseClassTests
    {
        public UserTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();

            // Act
            User user = UserBuilder.User(dto);

            // Assert
            user.UserName.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedUserName.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedEmail.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.EmailConfirmed.Should().BeTrue();
            user.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            user.PhoneNumberConfirmed.Should().BeTrue();
            user.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            user.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            user.Image.Should().BeNull();
            user.IsDefault.Should().BeFalse();
            user.Roles.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithValidFullParameters_InitializesProperties()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();

            // Act
            User user = UserBuilder.FullUser(dto);

            // Assert
            user.UserName.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedUserName.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedEmail.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.EmailConfirmed.Should().BeTrue();
            user.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            user.PhoneNumberConfirmed.Should().BeTrue();
            user.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            user.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            user.Image.Should().BeNull();
            user.IsDefault.Should().Be(dto.IsDefault);
            user.Roles.Should().BeEmpty();
        }

        [Fact]
        public void TestMap_InitializesProperties()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.FullUser(dto);

            // Act
            UserDTO result = Mapper.Map<UserDTO>(user);

            // Assert
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().Be(dto.IsDefault);
            result.Roles.Should().BeEmpty();
        }

        [Fact]
        public void TestMap_InitializesFullProperties()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.FullUser(dto);

            // Act
            UserDTO result = Mapper.Map<UserDTO>(user);

            // Assert
            result.Id.Should().Be(dto.Id).And.BeGreaterThan(0);
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            result.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            result.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            result.Image.Should().BeNull();
            result.IsDefault.Should().Be(dto.IsDefault);
            result.Roles.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void FullConstructor_WithInvalidId_ThrowsArgumentException(int id)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.Id = id;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.FullUser(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserIdNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void FullConstructor_WithInvalidEmail_ThrowsArgumentException(string? email)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.Email = email!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.FullUser(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserEmailNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void FullConstructor_WithInvalidPhoneNumber_ThrowsArgumentException(string? phoneNumber)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.PhoneNumber = phoneNumber;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.FullUser(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void FullConstructor_WithInvalidFirstName_ThrowsArgumentException(string? firstName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.FirstName = firstName!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.FullUser(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserFirstNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void FullConstructor_WithInvalidLastName_ThrowsArgumentException(string? lastName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.LastName = lastName!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.FullUser(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserLastNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidEmail_ThrowsArgumentException(string? email)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.Email = email!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.User(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserEmailNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_WithInvalidPhoneNumber_ThrowsArgumentException(string? phoneNumber)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.PhoneNumber = phoneNumber;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.User(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidFirstName_ThrowsArgumentException(string? firstName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.FirstName = firstName!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.User(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserFirstNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidLastName_ThrowsArgumentException(string? lastName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            dto.LastName = lastName!;

            // Act & Assert
            FluentActions.Invoking(() => UserBuilder.User(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserLastNameNeedsToBeSpecifiedException);
        }

        [Fact]
        public void Update_WithValidParameters()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.FullUser(dto);

            // Act
            user.Update(dto.Email, dto.PhoneNumber, dto.FirstName, dto.LastName, dto.Image);

            // Assert
            user.UserName.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedUserName.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            user.NormalizedEmail.Should().Be(dto.Email.ToUpper()).And.NotBeNullOrWhiteSpace();
            user.EmailConfirmed.Should().BeTrue();
            user.PhoneNumber.Should().Be(dto.PhoneNumber).And.NotBeEmpty();
            user.PhoneNumberConfirmed.Should().BeTrue();
            user.FirstName.Should().Be(dto.FirstName).And.NotBeNullOrWhiteSpace();
            user.LastName.Should().Be(dto.LastName).And.NotBeNullOrWhiteSpace();
            user.Image.Should().Be(dto.Image);
            user.IsDefault.Should().Be(dto.IsDefault);
            user.Roles.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Update_WithInvalidEmail_ThrowsArgumentException(string? email)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.Update(email!, user.PhoneNumber, user.FirstName, user.LastName, user.Image)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserEmailNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_WithInvalidPhoneNumber_ThrowsArgumentException(string? phoneNumber)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.Update(user.Email, phoneNumber, user.FirstName, user.LastName, user.Image)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Update_WithInvalidFirstName_ThrowsArgumentException(string? firstName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.Update(user.Email, user.PhoneNumber, firstName!, user.LastName, user.Image)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserFirstNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Update_WithInvalidLastName_ThrowsArgumentException(string? lastName)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.Update(user.Email, user.PhoneNumber, user.FirstName, lastName!, user.Image)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserLastNameNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Update_WithInvalidImage_ThrowsArgumentException(string? image)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.Update(user.Email, user.PhoneNumber, user.FirstName, user.LastName, image)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserImageNeedsToBeSpecifiedException);
        }

        [Fact]
        public void SetDarkMode_WithValidParameters()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act
            user.SetDarkMode(dto.DarkMode);

            // Assert
            user.DarkMode.Should().Be(dto.DarkMode);
        }

        [Fact]
        public void SetImage_WithValidParameters()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act
            user.SetImage(dto.Image!);

            // Assert
            user.Image.Should().Be(dto.Image);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void SetImage_WithInvalidImage_ThrowsArgumentException(string? image)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.SetImage(image!)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserImageNeedsToBeSpecifiedException);
        }

        [Fact]
        public void SetRoles_WithValidParameters()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            RoleDTO roleDTO = RoleBuilder.RoleDTO();
            List<Role> roles = RoleBuilder.RoleList(roleDTO);

            // Act
            user.SetRoles(roles);

            // Assert
            user.Roles.Should().BeEquivalentTo(roles);
        }

        [Fact]
        public void SetPasswordHash_WithValidParameters()
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act
            user.SetPasswordHash(dto.Password!);

            // Assert
            user.PasswordHash.Should().Be(dto.Password).And.NotBeNullOrWhiteSpace();
            user.SecurityStamp.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void SetPasswordHash_WithInvalidPasswordHash_ThrowsArgumentException(string? password)
        {
            // Arrange
            UserDTO dto = UserBuilder.UserDTO();
            User user = UserBuilder.User(dto);

            // Act & Assert
            FluentActions.Invoking(() => user.SetPasswordHash(password!)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResources.UserPasswordHashNeedsToBeSpecifiedException);
        }
    }
}
