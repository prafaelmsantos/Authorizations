namespace Authorizations.Persistence.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.ToTable("roles");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired(true);

            entity.Property(x => x.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false)
                .IsRequired(true);

            entity.Property(x => x.IsReadOnly)
               .HasColumnName("is_read_only")
               .HasDefaultValue(false)
               .IsRequired(true);

            /* ------------------- IdentityRole ------------------- */

            entity.Property(x => x.NormalizedName)
               .HasColumnName("normalized_name")
               .IsRequired(false);

            entity.Property(x => x.ConcurrencyStamp)
               .HasColumnName("concurrency_stamp")
               .IsRequired(false);

            /* ---------------------------------------------------- */
        }
    }
}
