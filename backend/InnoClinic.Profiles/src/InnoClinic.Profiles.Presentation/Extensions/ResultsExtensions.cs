using Microsoft.AspNetCore.Mvc;
using Shared.Results;

namespace InnoClinic.Profiles.Presentation.Extensions;

public static class ResultExtensions
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
            ErrorType.Validation => Results.BadRequest(new ProblemDetails 
            { 
                Title = "Validation Error", 
                Status = StatusCodes.Status400BadRequest,
                Detail = message 
            }),

            ErrorType.NotFound => Results.NotFound(new ProblemDetails 
            { 
                Title = "Not Found", 
                Status = StatusCodes.Status404NotFound,
                Detail = message 
            }),

            ErrorType.Conflict => Results.Conflict(new ProblemDetails 
            { 
                Title = "Conflict", 
                Status = StatusCodes.Status409Conflict,
                Detail = message 
            }),

            _ => Results.InternalServerError(new ProblemDetails 
            { 
                Title = "Server Error", 
                Status = StatusCodes.Status500InternalServerError,
                Detail = message 
            })
        };
    }
}
