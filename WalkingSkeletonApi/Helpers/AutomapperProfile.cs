using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Helpers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AppUser, UserToReturnDto>();
            CreateMap<RegisterDto, AppUser>();
               // .ForMember(dest => dest.PhoneNumber, x => x.MapFrom(x => x.Password));
        }
    }
}
