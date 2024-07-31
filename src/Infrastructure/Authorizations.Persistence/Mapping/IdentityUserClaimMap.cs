namespace Authorizations.Persistence.Mapping
{
    public class IdentityUserClaimMap : IEntityTypeConfiguration<IdentityUserClaim<long>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<long>> entity)
        {
            entity.ToTable("identity_user_claim");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.UserId)
                .HasColumnName("user_id")
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
