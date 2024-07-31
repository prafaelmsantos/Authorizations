namespace Authorizations.Tests.Core.Builders
{
    public class UserRoleBuilder
    {
        private static readonly Faker data = new("en");

        public static UserRole UserRole()
        {
            return new(data.Random.Long(1), data.Random.Long(1));
        }
        public static UserRole UserRole(long userId, long roleId)
        {
            return new(userId, roleId);
        }
        public static List<UserRole> UserRoleList(long userId, long roleId)
        {
            return new List<UserRole>() { UserRole(userId, roleId) };
        }
        public static IQueryable<UserRole> IQueryable(long userId, long roleId)
        {
            return UserRoleList(userId, roleId).AsQueryable();
        }
        public static IQueryable<UserRole> IQueryableEmpty()
        {
            return new List<UserRole>().AsQueryable();
        }
    }
}
