
using AutoMapper;
using StackPoint.Data.Models;
using StackPoint.Domain.Models;

namespace StackPoint.Service2.AutoMaps
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.OrganisationName, src => src.MapFrom(x => x.Organisation.Name));

            CreateMap<UserDto, User>();
        }
    }
}
