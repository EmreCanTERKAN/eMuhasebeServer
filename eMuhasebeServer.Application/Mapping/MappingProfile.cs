using AutoMapper;
using eMuhasebeServer.Application.Features.Companies;
using eMuhasebeServer.Application.Features.Users;
using eMuhasebeServer.Domain.Entities;

namespace eMuhasebeServer.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, AppUser>();
        CreateMap<UpdateUserCommand, AppUser>();

        CreateMap<CreateCompanyCommand, Company>();
        CreateMap<UpdateCompanyCommand, Company>();
    }
}
