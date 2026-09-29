using Booking.Domain.Entities;
using Booking.Domain.Enums;
using Booking.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure.Data;

public static class DatabaseSeeder
{
    private const string SeedOwnerEmail = "owner1@booking.com";
    private const string SeedOwnerPassword = "SeedPassw0rd!123";

    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
		using var scope = services.CreateScope();

		var dbContext = scope.ServiceProvider
				.GetRequiredService<ApplicationDbContext>();

		// Seed data here
	

        await dbContext.Database.MigrateAsync(cancellationToken);

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await RoleSeeder.SeedRolesAsync(roleManager);

        var owner = await EnsureSeedOwnerAsync(userManager);

        if (await dbContext.Hotels.AnyAsync(cancellationToken))
            return;

        var (hotels, rooms) = BuildHotels(owner.Id);

        dbContext.Hotels.AddRange(hotels);
        dbContext.Rooms.AddRange(rooms);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<ApplicationUser> EnsureSeedOwnerAsync(UserManager<ApplicationUser> userManager)
    {
        var owner = await userManager.FindByEmailAsync(SeedOwnerEmail);

        if (owner is null)
        {
            owner = new ApplicationUser
            {
                UserName = SeedOwnerEmail,
                Email = SeedOwnerEmail,
                FirstName = "khalid",
                LastName = "ali"
            };

            var result = await userManager.CreateAsync(owner, SeedOwnerPassword);

            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to create seed owner: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        if (!await userManager.IsInRoleAsync(owner, ApplicationRoles.HotelOwner))
        {
            var roleResult = await userManager.AddToRoleAsync(owner, ApplicationRoles.HotelOwner);

            if (!roleResult.Succeeded)
                throw new InvalidOperationException($"Failed to assign HotelOwner role to seed owner: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }

        return owner;
    }

    private static (List<Hotel> Hotels, List<Room> Rooms) BuildHotels(Guid ownerId)
    {
var seedHotels = new (string Name, string Description, string Address, string City, string Country, int StarRating)[]
        {
            ("Grand Cairo Palace", "A luxurious five-star hotel in the heart of Cairo with panoramic Nile views.", "15 Tahrir Square", "Cairo", "Egypt", 5),
            ("Nile View Suites", "Modern suites overlooking the Nile with a rooftop restaurant.", "8 Corniche El Nil", "Cairo", "Egypt", 4),
            ("Pyramids Oasis Resort", "Resort with direct views of the Great Pyramids and private pools.", "1 Pyramids Road", "Giza", "Egypt", 5),
            ("Sphinx City Lodge", "Budget-friendly lodge near Giza with friendly service.", "12 Al Haram Street", "Giza", "Egypt", 2),
            ("Alexandria Corniche Hotel", "Seafront hotel on the famous Corniche with Mediterranean views.", "Corniche Road", "Alexandria", "Egypt", 4),
            ("Sharm Sunset Beach Resort", "All-inclusive beach resort in Naama Bay, Sharm El-Sheikh.", "Naama Bay", "Sharm El-Sheikh", "Egypt", 5),
            ("Red Sea Pearl", "Diving-friendly hotel steps away from the Red Sea coastline.", "El Mamsha", "Hurghada", "Egypt", 4),
            ("El Gouna Lagoon Villas", "Villas and suites along the lagoons of El Gouna.", "Downtown El Gouna", "Hurghada", "Egypt", 5),
            ("Luxor Temple Inn", "Charming hotel within walking distance of Karnak Temple.", "Karnak Street", "Luxor", "Egypt", 3),
            ("Aswan Nile Dreams", "Relaxing hotel on Elephantine Island in Aswan.", "Elephantine Island", "Aswan", "Egypt", 4),
            ("Aswan South Retreat", "Serene retreat near the High Dam with garden views.", "Abu Simbel Road", "Aswan", "Egypt", 3),
            ("Marina Siwa Camp", "Eco-camp nestled amid Siwa's palm groves and salt lakes.", "Siwa Oasis", "Siwa", "Egypt", 3),
            ("Delta Business Hotel", "Practical hotel for business travellers in the Nile Delta.", "El Geish Street", "Tanta", "Egypt", 3),
            ("Dahab Blue House", "Colourful guesthouse by the Dahab boardwalk.", "Lighthouse Area", "Dahab", "Egypt", 3),
            ("Marsa Alam Coral Bay", "Eco-friendly resort near pristine coral reefs.", "Abu Dabbab Bay", "Marsa Alam", "Egypt", 4)
        };

        var hotels = new List<Hotel>();
        var rooms = new List<Room>();

        foreach (var (name, description, address, city, country, starRating) in seedHotels)
        {
            hotels.Add(new Hotel
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                Name = name,
                Description = description,
                Address = address,
                City = city,
                Country = country,
                StarRating = starRating,
                CheckInTime = new TimeOnly(14, 0),
                CheckOutTime = new TimeOnly(18, 0),
                IsActive = true
            });
        }

        var roomsByHotel = new (int Index, string RoomNumber, RoomType RoomType, string Description)[][]
        {
            new[] { (0, "101", RoomType.Standard, "Comfortable room with city view"), (0, "102", RoomType.Deluxe, "Spacious room with a balcony"), (0, "201", RoomType.Suite, "Suite with separate living area") },
            new[] { (1, "101", RoomType.Standard, "Room with a Nile river view"), (1, "102", RoomType.Deluxe, "Deluxe room with river view"), (1, "201", RoomType.Suite, "Executive suite overlooking the Nile") },
            new[] { (2, "101", RoomType.Deluxe, "Room with a pyramid view"), (2, "102", RoomType.Family, "Family room with two bedrooms"), (2, "201", RoomType.Suite, "Presidential suite with private pool") },
            new[] { (3, "201", RoomType.Economy, "Simple budget-friendly room"), (3, "202", RoomType.Standard, "Standard room with garden view"), (3, "203", RoomType.Standard, "Standard room on the upper floor") },
            new[] { (4, "301", RoomType.Deluxe, "Seafront room with balcony"), (4, "302", RoomType.Suite, "Suite with a sea view terrace"), (4, "303", RoomType.Family, "Family suite near the beach") },
            new[] { (5, "101", RoomType.Standard, "Garden view room"), (5, "102", RoomType.Deluxe, "Beachfront deluxe room"), (5, "201", RoomType.Suite, "Suite with private jacuzzi") },
            new[] { (6, "101", RoomType.Standard, "Cozy room with sea breeze"), (6, "102", RoomType.Deluxe, "Deluxe room with ocean view"), (6, "201", RoomType.Family, "Family room close to the pool") },
            new[] { (7, "101", RoomType.Suite, "Lagoon view villa suite"), (7, "102", RoomType.Deluxe, "Deluxe room on the lagoon"), (7, "201", RoomType.Family, "Family villa suite") },
            new[] { (8, "201", RoomType.Standard, "Room with temple views"), (8, "202", RoomType.Standard, "Twin room for two guests"), (8, "203", RoomType.Deluxe, "Deluxe room with a terrace") },
            new[] { (9, "101", RoomType.Standard, "Room overlooking the Nile"), (9, "102", RoomType.Deluxe, "Deluxe room with island view"), (9, "201", RoomType.Suite, "Riverfront suite") },
            new[] { (10, "201", RoomType.Economy, "Compact economical room"), (10, "202", RoomType.Standard, "Standard room with garden view"), (10, "203", RoomType.Standard, "Quiet room facing the courtyard") },
            new[] { (11, "101", RoomType.Standard, "Eco-friendly palm grove room"), (11, "102", RoomType.Deluxe, "Cabin with lake view"), (11, "201", RoomType.Family, "Family cabin with two beds") },
            new[] { (12, "301", RoomType.Standard, "City view room"), (12, "302", RoomType.Standard, "Quiet room on the executive floor"), (12, "303", RoomType.Deluxe, "Executive deluxe room") },
            new[] { (13, "101", RoomType.Economy, "Budget room for divers"), (13, "102", RoomType.Standard, "Room with boardwalk view"), (13, "201", RoomType.Deluxe, "Deluxe room with sea view") },
            new[] { (14, "101", RoomType.Standard, "Room near the reef"), (14, "102", RoomType.Deluxe, "Deluxe ocean view room"), (14, "201", RoomType.Suite, "Eco suite with panoramic views") }
        };

        for (var i = 0; i < hotels.Count; i++)
        {
            var hotel = hotels[i];
            var basePrice = 30m + (hotel.StarRating * 45m);

            foreach (var (_, roomNumber, roomType, description) in roomsByHotel[i])
            {
                rooms.Add(new Room
                {
                    Id = Guid.NewGuid(),
                    HotelId = hotel.Id,
                    RoomNumber = roomNumber,
                    RoomType = roomType,
                    PricePerNight = basePrice + ((int)roomType * 20m),
                    Capacity = roomType == RoomType.Family ? 5 : roomType == RoomType.Suite ? 4 : 2,
                    Description = description,
                    IsAvailable = true,
                    IsActive = true
                });
            }
        }

        return (hotels, rooms);
    }
}