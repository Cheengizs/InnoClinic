using AutoMapper;
using Business.Contracts.Office;
using Business.Features.Commands.Officies.UpdateOffice;
using DataAccess.Models;

namespace Business.Profiles;

public class OfficeProfile : Profile
{
    public OfficeProfile()
    {
        CreateMap<UpdateOfficeCommand, Office>().ReverseMap();
        CreateMap<Office, OfficeGet>().ReverseMap();
    }
}
