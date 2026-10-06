using Booking.Api.Extensions;
using Booking.Application.DTOs.Rooms;
using Booking.Application.Interfaces;
using Booking.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Controllers;

[ApiController]
[Route("api")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
	private const string HotelOwnerOrAdminRoles = "HotelOwner,Admin";

	[HttpPost("hotels/{hotelId:guid}/rooms")]
	[Authorize(Roles = HotelOwnerOrAdminRoles)]
	public async Task<IActionResult> Create([FromRoute]Guid hotelId, [FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
	{
		if (User.GetUserId() is not { } actorId)
			return Unauthorized();

		var result = await roomService.CreateAsync(actorId, User.IsInRole(ApplicationRoles.Admin), hotelId, request, cancellationToken);

		return result.IsSuccess
				? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
				: result.ToProblem();
	}

	[HttpGet("hotels/{hotelId:guid}/rooms")]
	public async Task<IActionResult> GetAll([FromRoute] Guid hotelId, CancellationToken cancellationToken)
	{
		var result = await roomService.GetByHotelAsync(hotelId, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpGet("rooms/{id:guid}")]
	public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var result = await roomService.GetByIdAsync(id, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPut("rooms/{id:guid}")]
	[Authorize(Roles = HotelOwnerOrAdminRoles)]
	public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoomRequest request, CancellationToken cancellationToken)
	{
		if (User.GetUserId() is not { } actorId)
			return Unauthorized();

		var result = await roomService.UpdateAsync(actorId, User.IsInRole(ApplicationRoles.Admin), id, request, cancellationToken);

		return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
	}

	[HttpPatch("rooms/{id:guid}/toggle-activation")]
	[Authorize(Roles = HotelOwnerOrAdminRoles)]
	public async Task<IActionResult> ToggleActivation([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		if (User.GetUserId() is not { } actorId)
			return Unauthorized();

		var result = await roomService.ToggleActivationAsync(actorId, User.IsInRole(ApplicationRoles.Admin), id, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}
}
