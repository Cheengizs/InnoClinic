using Domain.Shared;
using Microsoft.AspNetCore.Mvc;  

namespace Presentation.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert success result to problem details");
        }

        return CreateProblemDetails(result.ErrorType, result.ErrorMessage);
    }

    public static IActionResult ToProblemDetails<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert success result to problem details");
        }

        return CreateProblemDetails(result.ErrorType, result.ErrorMessage);
    }

    private static IActionResult CreateProblemDetails(ErrorType errorType, string? message)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = message
        };

        return errorType switch
        {
            ErrorType.Validation => new BadRequestObjectResult(new ProblemDetails 
            { 
                Title = "Validation Error", 
                Status = StatusCodes.Status400BadRequest,
                Detail = message 
            }),

            ErrorType.NotFound => new NotFoundObjectResult(new ProblemDetails 
            { 
                Title = "Not Found", 
                Status = StatusCodes.Status404NotFound,
                Detail = message 
            }),

            ErrorType.Conflict => new ConflictObjectResult(new ProblemDetails 
            { 
                Title = "Conflict", 
                Status = StatusCodes.Status409Conflict,
                Detail = message 
            }),

            _ => new ObjectResult(new ProblemDetails 
            { 
                Title = "Server Error", 
                Status = StatusCodes.Status500InternalServerError,
                Detail = message 
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}