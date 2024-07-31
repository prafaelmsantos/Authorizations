namespace Authorizations.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<User, Role, long,
                                                   IdentityUserClaim<long>,
                                                   UserRole,
                                                   IdentityUserLogin<long>,
                                                   IdentityRoleClaim<long>,
                                                   IdentityUserToken<long>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Necessario para o IdentityDbContext

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());

            modelBuilder.ApplyConfiguration(new IdentityRoleClaimMap());
            modelBuilder.ApplyConfiguration(new IdentityUserClaimMap());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginMap());
            modelBuilder.ApplyConfiguration(new IdentityUserTokenMap());

            modelBuilder.AddInitialSeed();
        }
    }
}
