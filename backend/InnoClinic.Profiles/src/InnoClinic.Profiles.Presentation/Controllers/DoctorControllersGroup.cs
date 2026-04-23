using System.Security.Claims;
using FluentValidation;
using InnoClinic.Profiles.Application.Features.Commands.Doctors.CreateDoctor;
using InnoClinic.Profiles.Application.Features.Queries.Doctors.GetDoctorById;
using InnoClinic.Profiles.Presentation.Contracts;
using InnoClinic.Profiles.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.Presentation.Controllers;

public static class DoctorControllersGroup
{
    public static RouteGroupBuilder MapDoctorControllersGroup(this RouteGroupBuilder group)
    {
        group.MapPost("", async Task<IResult> ([FromForm] DoctorCreateRequest request,
                IValidator<DoctorCreateRequest> validator, IMediator mediator) =>
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors[0].ErrorMessage);
                }

                var photoStream = request.Photo?.OpenReadStream();
                var photoContentType = request.Photo?.ContentType;

                var command = new CreateDoctorCommand(
                    CareerStartYear: request.CareerStartYear,
                    DateOfBirth: request.DateOfBirth,
                    SpecializationId: request.SpecializationId,
                    OfficeId: request.OfficeId,
                    Email: request.Email,
                    FirstName: request.FirstName,
                    LastName: request.LastName,
                    MiddleName: request.MiddleName,
                    PhotoContentType: photoContentType,
                    PhotoStream: photoStream,
                    Status: request.Status);

                var result = await mediator.Send(command);

                if (!result.IsSuccess)
                {
                    return result.ToProblemDetails();
                }

                var resultValue = result.Value!;

                // todo: change route
                return Results.CreatedAtRoute("GetDoctorById", new { id = resultValue.Id }, resultValue);
            })
            .DisableAntiforgery()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin, Receptionist" });

        group.MapGet("{id:guid}", async Task<IResult> ([FromRoute] Guid id, ClaimsPrincipal user, IMediator mediator) =>
            {
                var query = new GetDoctorByIdQuery(id);
                var queryResult = await mediator.Send(query);
                if (!queryResult.IsSuccess)
                {
                    return queryResult.ToProblemDetails();
                }

                var doctor = queryResult.Value!;

                bool isPrivateView = false;

                if (user.Identity?.IsAuthenticated == true)
                {
                    var isPrivilegedRole = user.IsInRole("Admin") || user.IsInRole("Receptionist");

                    var currentUserIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    bool isOwnProfile = false;
                    if (Guid.TryParse(currentUserIdClaim, out Guid currentUserId))
                    {
                        isOwnProfile = user.IsInRole("Doctor") && currentUserId == doctor.AccountId;
                    }

                    if (isPrivilegedRole || isOwnProfile)
                    {
                        isPrivateView = true;
                    }
                }

                if (isPrivateView)
                {
                    var privateResponse = new DoctorPrivateResponse
                    {
                        Id = doctor.Id,
                        PhotoUrl = doctor.PhotoUrl,
                        FirstName = doctor.FirstName,
                        LastName = doctor.LastName,
                        MiddleName = doctor.MiddleName,
                        DateOfBirth = doctor.DateOfBirth,
                        SpecializationId = doctor.SpecializationId,
                        OfficeId = doctor.OfficeId,
                        CareerStartYear = doctor.CareerStartYear,
                        Status = doctor.Status
                    };

                    return Results.Ok(privateResponse);
                }

                var fullName = string.IsNullOrWhiteSpace(doctor.MiddleName)
                    ? $"{doctor.FirstName} {doctor.LastName}"
                    : $"{doctor.FirstName} {doctor.MiddleName} {doctor.LastName}";

                var currentYear = DateTime.UtcNow.Year;
                var experience = currentYear - doctor.CareerStartYear + 1;

                var publicResponse = new DoctorPublicResponse
                {
                    Id = doctor.Id,
                    PhotoUrl = doctor.PhotoUrl,
                    FullName = fullName,
                    OfficeId = doctor.OfficeId,
                    Experience = experience,
                    SpecializationId = doctor.SpecializationId
                };

                return Results.Ok(publicResponse);
            })
            .WithName("GetDoctorById")
            .AllowAnonymous();

        return group;
    }
}
