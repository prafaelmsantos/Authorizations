namespace Authorizations.Core.Domain
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string? Image { get; private set; }
        public bool DarkMode { get; private set; } = false;
        public bool IsDefault { get; private set; } = false;

        public virtual ICollection<Role> Roles { get; private set; }

        public User()
        {
            Roles = new List<Role>();
        }

        public User(long id, string email, string? phoneNumber, string firstName, string lastName, bool isDefault)
        {
            id.Throw(() => throw new Exception(DomainResources.UserIdNeedsToBeSpecifiedException))
            .IfNegativeOrZero();

            email.ThrowIfNull(() => throw new Exception(DomainResources.UserEmailNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            phoneNumber?.Throw(() => throw new Exception(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            firstName.ThrowIfNull(() => throw new Exception(DomainResources.UserFirstNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            lastName.ThrowIfNull(() => throw new Exception(DomainResources.UserLastNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            Id = id;
            UserName = email;
            NormalizedUserName = email.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
            EmailConfirmed = true;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = true;
            FirstName = firstName;
            LastName = lastName;
            IsDefault = isDefault;

            Roles = new List<Role>();
        }

        public User(string email, string? phoneNumber, string firstName, string lastName)
        {
            email.ThrowIfNull(() => throw new Exception(DomainResources.UserEmailNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            phoneNumber?.Throw(() => throw new Exception(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            firstName.ThrowIfNull(() => throw new Exception(DomainResources.UserFirstNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            lastName.ThrowIfNull(() => throw new Exception(DomainResources.UserLastNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            UserName = email;
            NormalizedUserName = email.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
            EmailConfirmed = true;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumber != null;
            FirstName = firstName;
            LastName = lastName;
            IsDefault = false;

            Roles = new List<Role>();
        }

        public void Update(string email, string? phoneNumber, string firstName, string lastName, string? image)
        {
            email.ThrowIfNull(() => throw new Exception(DomainResources.UserEmailNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            phoneNumber?.Throw(() => throw new Exception(DomainResources.UserPhoneNumberNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            firstName.ThrowIfNull(() => throw new Exception(DomainResources.UserFirstNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            lastName.ThrowIfNull(() => throw new Exception(DomainResources.UserLastNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            image?.Throw(() => throw new Exception(DomainResources.UserImageNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            UserName = email;
            NormalizedUserName = email.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Image = image;

            Roles = new List<Role>();
        }

        public void SetDarkMode(bool darkMode)
        {
            DarkMode = darkMode;
        }

        public void SetImage(string? image)
        {
            image?.Throw(() => throw new Exception(DomainResources.UserImageNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            Image = image;
        }

        public void SetRoles(List<Role> roles)
        {
            Roles.Clear();
            Roles = roles;
        }

        public void SetPasswordHash(string passwordHash)
        {
            passwordHash.ThrowIfNull(() => throw new Exception(DomainResources.UserPasswordHashNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            PasswordHash = passwordHash;
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
