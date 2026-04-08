using Microsoft.AspNetCore.Mvc;
using InnoClinic.Officies.Shared.Results;

namespace Presentation.Extensions;

public static class ResultExtension
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert success result to problem details");
        }

        return CreateProblemDetails(result.ErrorType, result.ErrorMessage);
    }

    public static IResult ToProblemDetails<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert success result to problem details");
        }

        return CreateProblemDetails(result.ErrorType, result.ErrorMessage);
    }

    private static IResult CreateProblemDetails(ErrorType errorType, string? message)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = message
        };

        return errorType switch
        {
            ErrorType.Validation => Results.Problem(
                detail: message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation Error"),

            ErrorType.NotFound => Results.Problem(
                detail: message,
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found"),

            ErrorType.Conflict => Results.Problem(
                detail: message,
                statusCode: StatusCodes.Status409Conflict,
                title: "Conflict"),

            _ => Results.Problem(
                detail: message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Server Error")
        };
    }
}
