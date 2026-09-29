using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.Errors;

public static class StatusCodes
{
	public readonly static int Status401Unauthorized= 401;
	public readonly static int Status404NotFound = 404;
	public readonly static int Status409Conflict = 409;
	public readonly static int Status400BadRequest = 400;
	public readonly static int Status403Forbidden = 403;
	
}
