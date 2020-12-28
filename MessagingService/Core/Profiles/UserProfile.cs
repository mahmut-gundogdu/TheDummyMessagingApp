using AutoMapper;
using MessagingService.Entities;
using MessagingService.Models.Users;
using MessagingService.Services.Dtos;

namespace MessagingService.Core.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserModel, User>();
                CreateMap<RegisterInput,UserModel>()
                    .ForMember(dest => dest.Password ,
                        src => src.MapFrom(x=> x.Password.ToHash()));
        }
    }
}