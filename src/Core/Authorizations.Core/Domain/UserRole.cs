namespace Authorizations.Core.Domain
{
    public class UserRole : IdentityUserRole<long>
    {
        public virtual User User { get; private set; } = null!;
        public virtual Role Role { get; private set; } = null!;

        public UserRole() { }

        public UserRole(long userId, long roleId)
        {
            userId.Throw(() => throw new Exception(DomainResources.UserIdNeedsToBeSpecifiedException))
              .IfNegativeOrZero();

            roleId.Throw(() => throw new Exception(DomainResources.RoleIdNeedsToBeSpecifiedException))
              .IfNegativeOrZero();

            UserId = userId;
            RoleId = roleId;
        }
    }
}
