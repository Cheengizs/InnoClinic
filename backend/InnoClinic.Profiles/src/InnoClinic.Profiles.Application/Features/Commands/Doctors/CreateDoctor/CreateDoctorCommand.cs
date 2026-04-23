using InnoClinic.Profiles.Application.ViewModels;
using InnoClinic.Profiles.Domain.Shared;
using MediatR;
using Shared.Results;

namespace InnoClinic.Profiles.Application.Features.Commands.Doctors.CreateDoctor;

public record CreateDoctorCommand(
    Stream? PhotoStream,
    string? PhotoContentType,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string Email,
    Guid SpecializationId,
    Guid OfficeId,
    int CareerStartYear,
    DoctorStatus Status) : IRequest<Result<DoctorGet>>;
