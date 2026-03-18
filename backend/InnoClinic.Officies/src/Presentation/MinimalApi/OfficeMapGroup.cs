using AutoMapper;
using Business.Features.Commands.Offices.CreateOffice;
using Business.Features.Commands.Offices.DeleteOffice;
using Business.Features.Commands.Offices.SetActiveStatus;
using Business.Features.Commands.Offices.UpdateOffice;
using Business.Features.Queries.Offices.GetOfficeById;
using Business.Features.Queries.Offices.GetOffices;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Extensions;
using Presentation.ViewModels;

namespace Presentation.MinimalApi;

public static class OfficeMapGroup
{
    public static RouteGroupBuilder MapOffices(this RouteGroupBuilder group)
    {
        group.MapGet("{id:guid}", async (IMediator mediator, IMapper mapper, [FromRoute] Guid id) =>
        {
            GetOfficeByIdQuery request = new(id);
            var result = await mediator.Send(request);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }

            var resultValue = mapper.Map<OfficeGetResponse>(result.Value);
            return Results.Ok(resultValue);
        });

        group.MapGet("",
            async (IMediator mediator, IMapper mapper, [FromQuery] int pageNumber = 0,
                [FromQuery] int pageCount = 10) =>
            {
                GetOfficesQuery request = new(pageNumber, pageCount);
                var result = await mediator.Send(request);
                var resultValue = mapper.Map<List<OfficeGetResponse>>(result.Value);
                return Results.Ok(resultValue);
            })
            .WithName("GetOfficeById");

        group.MapPost("", async ([FromBody] OfficeCreateRequest createRequest, IMediator mediator, IMapper mapper, IValidator<OfficeCreateRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(createRequest);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var request = mapper.Map<CreateOfficeCommand>(createRequest);
                
            var result = await mediator.Send(request);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }
            
            var resultValue = mapper.Map<OfficeGetResponse>(result.Value);
            return Results.CreatedAtRoute("GetOfficeById", new { id = resultValue.Id }, resultValue);
        });

        group.MapDelete("{id:guid}", async (IMediator mediator, [FromRoute] Guid id) =>
        {
            DeleteOfficeCommand request = new(id);
            var result = await mediator.Send(request);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }

            return Results.NoContent();
        });

        group.MapPut("", async (IMediator mediator, IMapper mapper, [FromBody] OfficeUpdateRequest updateRequest, IValidator<OfficeUpdateRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(updateRequest);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors[0].ErrorMessage);
            }

            var request = mapper.Map<UpdateOfficeCommand>(updateRequest);
            var result = await mediator.Send(request);

            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }
            
            return Results.NoContent();    
        });

        group.MapPatch("setActiveStatus", async (IMediator mediator, [FromBody] OfficeActivityRequest activityRequest) =>
        {
            SetActiveStatusCommand request = new(activityRequest.Id ,activityRequest.IsActive);
            var result = await mediator.Send(request);
            if (!result.IsSuccess)
            {
                return result.ToProblemDetails();
            }
            
            return Results.NoContent();
        });

        return group;
    }
}
