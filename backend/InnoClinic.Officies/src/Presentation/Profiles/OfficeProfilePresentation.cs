using AutoMapper;
using Business.Contracts.Office;
using Business.Features.Commands.Offices.CreateOffice;
using Business.Features.Commands.Offices.UpdateOffice;
using Presentation.ViewModels;

namespace Presentation.Profiles;

public class OfficeProfilePresentation : Profile
{
    public OfficeProfilePresentation()
    {
        CreateMap<OfficeGetResponse, OfficeGet>().ReverseMap();
        CreateMap<CreateOfficeCommand, OfficeCreateRequest>().ReverseMap();
        CreateMap<OfficeUpdateRequest, UpdateOfficeCommand>().ReverseMap();
    }
}
