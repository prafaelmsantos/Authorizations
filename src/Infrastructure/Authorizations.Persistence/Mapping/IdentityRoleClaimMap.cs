namespace Authorizations.Persistence.Mapping
{
    public class IdentityRoleClaimMap : IEntityTypeConfiguration<IdentityRoleClaim<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<long>> entity)
        {
            entity.ToTable("identity_role_claim");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.RoleId)
                .HasColumnName("role_id")
                .IsRequired(true);

            entity.Property(x => x.ClaimType)
                .HasColumnName("claim_type")
                .IsRequired(true);

            entity.Property(x => x.ClaimValue)
                .HasColumnName("claim_value")
                .IsRequired(true);
        }
    }
}
