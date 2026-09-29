using Booking.Domain.Entities;
using Booking.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(h => h.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(h => h.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(h => h.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.StarRating)
            .IsRequired();

        builder.HasIndex(h => h.City);
        builder.HasIndex(h => h.Country);
        builder.HasIndex(h => h.OwnerId);
        builder.HasIndex(h => h.IsActive);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(h => h.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t =>
            t.HasCheckConstraint("CK_Hotels_CheckOutAfterCheckIn",
                "CheckInTime IS NULL OR CheckOutTime IS NULL OR CheckOutTime > CheckInTime"));
    }
}
