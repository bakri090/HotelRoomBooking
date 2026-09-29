using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Infrastructure.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoomNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.PricePerNight)
            .HasPrecision(18, 2);

        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.HasOne(r => r.Hotel)
            .WithMany()
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.HotelId);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => new { r.HotelId, r.RoomNumber }).IsUnique();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Rooms_PricePerNight_Positive", "PricePerNight > 0");
            t.HasCheckConstraint("CK_Rooms_Capacity_Positive", "Capacity > 0");
        });
    }
}