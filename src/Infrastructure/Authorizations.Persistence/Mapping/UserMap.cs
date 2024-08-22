namespace Authorizations.Persistence.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);

            /* ------------------- IdentityUser ------------------- */

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.UserName)
               .HasColumnName("user_name")
               .IsRequired(true);

            entity.Property(x => x.NormalizedUserName)
               .HasColumnName("normalized_user_name")
               .IsRequired(false);

            entity.Property(x => x.Email)
               .HasColumnName("email")
               .IsRequired(true);

            entity.Property(x => x.NormalizedEmail)
               .HasColumnName("normalized_email")
               .IsRequired(false);

            entity.Property(x => x.EmailConfirmed)
                .HasColumnName("email_confirmed")
                .HasDefaultValue(false)
                .IsRequired(true);

            entity.Property(x => x.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired(false);

            entity.Property(x => x.SecurityStamp)
               .HasColumnName("security_stamp")
               .IsRequired(false);

            entity.Property(x => x.ConcurrencyStamp)
               .HasColumnName("concurrency_stamp")
               .IsRequired(false);

            entity.Property(x => x.PhoneNumber)
               .HasColumnName("phone_number")
               .IsRequired(true);

            entity.Property(x => x.PhoneNumberConfirmed)
               .HasColumnName("phone_number_confirmed")
               .HasDefaultValue(false)
               .IsRequired(true);

            entity.Property(x => x.TwoFactorEnabled)
               .HasColumnName("two_factor_enabled")
               .HasDefaultValue(false)
               .IsRequired(true);

            entity.Property(x => x.LockoutEnd)
               .HasColumnName("lockout_end")
               .IsRequired(false);

            entity.Property(x => x.LockoutEnabled)
               .HasColumnName("lockout_enabled")
               .HasDefaultValue(false)
               .IsRequired(true);

            entity.Property(x => x.AccessFailedCount)
               .HasColumnName("access_failed_count")
               .IsRequired(true);

            /* ---------------------------------------------------- */

            entity.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired(true);

            entity.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired(true);

            entity.Property(x => x.Image)
                .HasColumnName("image")
                .IsRequired(false);

            entity.Property(x => x.DarkMode)
                .HasColumnName("dark_mode")
                .HasDefaultValue(false)
                .IsRequired(true);

            entity.Property(x => x.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false)
                .IsRequired(true);


            entity.HasMany(x => x.Roles)
               .WithMany(x => x.Users)
               .UsingEntity<UserRole>(
                   x => x.HasOne(x => x.Role)
                   .WithMany().HasForeignKey(x => x.RoleId),
                   x => x.HasOne(x => x.User)
                  .WithMany().HasForeignKey(x => x.UserId));

        }
    }
}
