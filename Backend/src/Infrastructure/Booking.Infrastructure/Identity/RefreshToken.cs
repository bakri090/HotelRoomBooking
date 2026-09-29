using Booking.Domain.Common;
using Booking.Infrastructure.Identity;

namespace Booking.Infrastructure.Identity;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? CreatedByID { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsValid => RevokedAt == null && !IsExpired;

    public ApplicationUser CreatedBy { get; set; } = null!;
}
