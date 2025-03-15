using AuctriAPI.Application.Services.Authentication;
using AuctriAPI.Contracts.Authentication;
using AuctriAPI.Core.Entitites;
using AutoMapper;

namespace AuctriAPI.Mapping;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        // Entity to DTO
        CreateMap<User, AuthenticationResult>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());

        CreateMap<AuthenticationResult, AuthenticationResponse>();

        // DTO to Entity
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
    }
}