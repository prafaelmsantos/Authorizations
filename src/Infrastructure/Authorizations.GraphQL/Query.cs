namespace Authorizations.GraphQL
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:All members as static")]
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers([Service] IUserRepository repo)
        {
            return repo.GetAll().Include(x => x.Roles);
        }

        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Role> GetRoles([Service] IRoleRepository repo)
        {
            return repo.GetAll();
        }
    }
}
