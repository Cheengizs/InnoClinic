using AutoMapper;
using Business.Features.Commands.Officies.UpdateOffice;
using DataAccess.Models;

namespace Business.Profiles;

public class OfficeProfile : Profile
{
    public OfficeProfile()
    {
        CreateMap<UpdateOfficeCommand, Office>().ReverseMap();
    }
}
