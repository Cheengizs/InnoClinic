using InnoClinic.Profiles.Application.ViewModels;
using InnoClinic.Profiles.Domain.Models;
using MediatR;
using Shared.Results;

namespace InnoClinic.Profiles.Application.Features.Queries.Doctors.GetDoctorById;

public record GetDoctorByIdQuery(Guid Id) : IRequest<Result<DoctorGet?>>;

