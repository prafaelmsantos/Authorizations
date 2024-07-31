namespace Authorizations.Persistence.Mapping
{
    public class IdentityUserTokenMap : IEntityTypeConfiguration<IdentityUserToken<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<long>> entity)
        {
            entity.ToTable("identity_user_token");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired(true);

            entity.Property(x => x.LoginProvider)
                .HasColumnName("login_provider")
                .IsRequired(true);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired(true);

            entity.Property(x => x.Value)
                .HasColumnName("value")
                .IsRequired(true);

        }
    }
}
