namespace Authorizations.Persistence.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> entity)
        {
            entity.ToTable("user_roles");

            entity.HasKey(user => new { user.UserId, user.RoleId });

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired(true);

            entity.Property(x => x.RoleId)
                .HasColumnName("role_id")
                .IsRequired(true);
        }
    }
}
