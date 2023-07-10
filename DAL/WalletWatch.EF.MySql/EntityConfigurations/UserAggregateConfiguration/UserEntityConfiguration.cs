using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WalletWatch.Domain.Entities.UserAggregate;

namespace WalletWatch.EF.MySql.EntityConfigurations.UserAggregateConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasMaxLength(256)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(e => e.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.UserName)
                .HasMaxLength(256)
                .HasDefaultValue(Guid.NewGuid().ToString())
                .IsRequired();

            builder.Property(e => e.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(e => e.SubscriptionStatus)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .IsRequired();

            builder.Property(e => e.SecurityStamp)
                .HasMaxLength(256);

            builder.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(256);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasDefaultValue(false);

            builder.Property(e => e.TwoFactorEnabled)
                .HasDefaultValue(false);

            builder.Property(e => e.LockoutEnabled)
                .HasDefaultValue(false);

            builder.Property(e => e.AccessFailedCount)
                .HasDefaultValue(0);
        }
    }
}
