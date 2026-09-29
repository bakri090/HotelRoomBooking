using Microsoft.AspNetCore.Identity;

namespace Booking.Infrastructure.Identity;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roles = [ApplicationRoles.Admin, ApplicationRoles.Customer, ApplicationRoles.HotelOwner];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
        }
    }
}
