namespace Authorizations.Persistence.Mapping
{
    public class IdentityUserLoginMap : IEntityTypeConfiguration<IdentityUserLogin<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<long>> entity)
        {
            entity.ToTable("identity_user_login");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired(true);

            entity.Property(x => x.LoginProvider)
                .HasColumnName("login_provider")
                .IsRequired(true);

            entity.Property(x => x.ProviderKey)
                .HasColumnName("provider_key")
                .IsRequired(true);

            entity.Property(x => x.ProviderDisplayName)
                .HasColumnName("provider_display_name")
                .IsRequired(true);

        }
    }
}
