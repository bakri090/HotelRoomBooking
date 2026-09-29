using Booking.Api.Extensions;
using Booking.Application.DTOs.Rooms;
using Booking.Application.Interfaces;
using Booking.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking_API.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    private const string HotelOwnerOrAdminRoles = "HotelOwner,Admin";

    private readonly IRoomService _roomService = roomService;

    [HttpPost("/api/hotels/{hotelId:guid}/rooms")]
    [Authorize(Roles = HotelOwnerOrAdminRoles)]
    public async Task<IActionResult> Create(Guid hotelId, [FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var actorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
        var isAdmin = User.IsInRole(ApplicationRoles.Admin);

        var result = await _roomService.CreateAsync(actorId, isAdmin, hotelId, request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("/api/hotels/{hotelId:guid}/rooms")]
    public async Task<IActionResult> GetAll(Guid hotelId, CancellationToken cancellationToken)
    {
        var result = await _roomService.GetByHotelAsync(hotelId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roomService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = HotelOwnerOrAdminRoles)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequest request, CancellationToken cancellationToken)
    {
        var actorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
        var isAdmin = User.IsInRole(ApplicationRoles.Admin);

        var result = await _roomService.UpdateAsync(actorId, isAdmin, id, request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = HotelOwnerOrAdminRoles)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var actorId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString());
        var isAdmin = User.IsInRole(ApplicationRoles.Admin);

        var result = await _roomService.DeactivateAsync(actorId, isAdmin, id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}