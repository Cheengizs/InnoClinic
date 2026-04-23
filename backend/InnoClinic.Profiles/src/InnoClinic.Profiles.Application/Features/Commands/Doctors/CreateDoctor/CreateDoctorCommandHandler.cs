using InnoClinic.Profiles.Application.BlobStorage;
using InnoClinic.Profiles.Application.Repositories;
using InnoClinic.Profiles.Application.ViewModels;
using InnoClinic.Profiles.Domain.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts;
using Shared.Results;

namespace InnoClinic.Profiles.Application.Features.Commands.Doctors.CreateDoctor;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<DoctorGet>>
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IPublishEndpoint _endpoint;
    
    public CreateDoctorCommandHandler(IDoctorRepository doctorRepository, IServiceScopeFactory scopeFactory, IPublishEndpoint endpoint)
    {
        _doctorRepository = doctorRepository;
        _scopeFactory = scopeFactory;
        _endpoint = endpoint;
    }
    
    public async Task<Result<DoctorGet>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorFromRepo = await _doctorRepository.GetDoctorByEmailAsync(request.Email, cancellationToken);

        if (doctorFromRepo != null)
        {
            return Result<DoctorGet>.Failure("Doctor already exists", ErrorType.Conflict);
        }
        
        var blobService = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IBlobService>();
        
        Guid? photoGuid = null;
        if (request.PhotoStream != null && !string.IsNullOrEmpty(request.PhotoContentType))
        {
            photoGuid = await blobService.UploadAsync(request.PhotoStream, request.PhotoContentType);
        }

        Guid accountId = Guid.CreateVersion7();
        
        var doctorToAdd = Doctor.Create(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.DateOfBirth,
            request.Email,
            accountId,
            request.SpecializationId,
            request.OfficeId,
            request.CareerStartYear,
            photoGuid?.ToString());

        var createdDoctor = await _doctorRepository.CreateDoctorAsync(doctorToAdd);
        
        var messageToPublish = new CreateUserDoctorAccountRequest(accountId, createdDoctor.Email);
        await _endpoint.Publish(messageToPublish);
        
        var result = new DoctorGet()
        {
            Id = createdDoctor.Id,
            FirstName = createdDoctor.FirstName,
            LastName = createdDoctor.LastName,
            MiddleName = createdDoctor.MiddleName,
            CareerStartYear = createdDoctor.CareerStartYear,
            DateOfBirth = createdDoctor.DateOfBirth,
            Status = createdDoctor.Status,
            OfficeId = createdDoctor.OfficeId,
            PhotoUrl = createdDoctor.PhotoUrl,
            SpecializationId = createdDoctor.SpecializationId,
            AccountId = createdDoctor.AccountId,
            Email = createdDoctor.Email
        };
        
        return Result<DoctorGet>.Success(result);
    }
}
