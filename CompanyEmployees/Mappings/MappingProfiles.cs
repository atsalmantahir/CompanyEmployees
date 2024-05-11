using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Company, CompanyDto>()
        .ForMember(c => c.FullAddress,
        opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));
    }

}
