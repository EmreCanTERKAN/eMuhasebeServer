using AutoMapper;
using eMuhasebeServer.Application.Features.Users;
using eMuhasebeServer.Domain.Entities;

namespace eMuhasebeServer.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, AppUser>();
        CreateMap<UpdateUserCommand, AppUser>();
    }
}
