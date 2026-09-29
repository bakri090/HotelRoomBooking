using global::Booking.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Api.Extensions;

public static class ResultExtensions
{
	public static ObjectResult ToProblem(this Result result)
	{
		if (result.IsSuccess)
			throw new InvalidOperationException("Cannot convert success result to problem");

		var problemDetails = new ProblemDetails
		{
			Status = result.Error.StatusCode,
			Title = result.Error.Code,
			Detail = result.Error.Description
		};

		problemDetails.Extensions["errors"] = new[] { result.Error };

		return new ObjectResult(problemDetails)
		{
			StatusCode = result.Error.StatusCode
		};
	}
}