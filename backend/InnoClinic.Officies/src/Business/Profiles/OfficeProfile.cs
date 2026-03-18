using AutoMapper;
using Business.Contracts.Office;
using Business.Features.Commands.Offices.CreateOffice;
using Business.Features.Commands.Offices.UpdateOffice;
using DataAccess.Models;

namespace Business.Profiles;

public class OfficeProfile : Profile
{
    public OfficeProfile()
    {
        CreateMap<UpdateOfficeCommand, Office>().ReverseMap();
        CreateMap<Office, OfficeGet>().ReverseMap();
        CreateMap<Office, CreateOfficeCommand>().ReverseMap();
    }
}
