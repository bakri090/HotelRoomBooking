using Booking.Api.Extensions;
using Booking.Application.DTOs.Hotels;
using Booking.Application.Interfaces;
using Booking.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(IHotelService hotelService) : ControllerBase
{
    private const string HotelOwnerOrAdminRoles = "HotelOwner,Admin";

    private readonly IHotelService _hotelService = hotelService;

    [HttpPost]
    [Authorize(Roles = ApplicationRoles.HotelOwner)]
    public async Task<IActionResult> Create([FromBody] CreateHotelRequest request, CancellationToken cancellationToken)
    {
		if (!User.TryGetUserId(out var ownerId))
			return Unauthorized();

		var result = await _hotelService.CreateAsync(ownerId, request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetHotelsRequest request, CancellationToken cancellationToken)
    {
        var result = await _hotelService.GetAllAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _hotelService.GetByIdAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = HotelOwnerOrAdminRoles)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotelRequest request, CancellationToken cancellationToken)
    {

		if (!User.TryGetUserId(out var actorId))
			return Unauthorized();
		
        var isAdmin = User.IsInRole(ApplicationRoles.Admin);

        var result = await _hotelService.UpdateAsync(actorId, isAdmin, id, request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

	[HttpPatch("{id:guid}/toggle-activation")]
	[Authorize(Roles = HotelOwnerOrAdminRoles)]
	public async Task<IActionResult> ToggleActivation(Guid id, CancellationToken cancellationToken)
	{
		if (!User.TryGetUserId(out var actorId))
			return Unauthorized();

		var isAdmin = User.IsInRole(ApplicationRoles.Admin);

		var result = await _hotelService.ToggleActivationAsync(actorId, isAdmin, id, cancellationToken);

		return result.IsSuccess ? NoContent() : result.ToProblem();
	}
}