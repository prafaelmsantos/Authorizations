namespace Authorizations.Persistence.Mapping.Seed
{
    public static class InitialSeed
    {
        public static void AddInitialSeed(this ModelBuilder modelBuilder)
        {
            Role role = new(1, "Administrador", true, true);
            modelBuilder.Entity<Role>().HasData(role, new(2, "Colaborador", true), new(3, "Comercial"));

            User user = new(1, "automoreiraportugal@gmail.com", "231472555", "Auto", "Moreira", true);
            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<UserRole>().HasData(new UserRole(user.Id, role.Id));
        }
    }
}
