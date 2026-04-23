using InnoClinic.Profiles.Application.Repositories;
using InnoClinic.Profiles.Application.ViewModels;
using MediatR;
using Shared.Results;

namespace InnoClinic.Profiles.Application.Features.Queries.Doctors.GetDoctorById;

public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorGet>>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetDoctorByIdQueryHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<Result<DoctorGet>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var doctorFromRepo = await _doctorRepository.GetDoctorByIdAsync(request.Id, cancellationToken);
        if(doctorFromRepo == null)
        {
            return Result<DoctorGet>.Failure("Doctor not found", ErrorType.NotFound);
        }

        var doctorGet = new DoctorGet()
        {
            Id = doctorFromRepo.Id,
            FirstName = doctorFromRepo.FirstName,
            LastName = doctorFromRepo.LastName,
            MiddleName = doctorFromRepo.MiddleName,
            CareerStartYear = doctorFromRepo.CareerStartYear,
            DateOfBirth = doctorFromRepo.DateOfBirth,
            Email = doctorFromRepo.Email,
            SpecializationId = doctorFromRepo.SpecializationId,
            OfficeId = doctorFromRepo.OfficeId,
            Status = doctorFromRepo.Status,
            AccountId = doctorFromRepo.AccountId,
            PhotoUrl = doctorFromRepo.PhotoUrl
        };
        
        return Result<DoctorGet>.Success(doctorGet);
    }
}
