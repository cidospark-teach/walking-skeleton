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
            CreateMap<AppUser, RegisterSuccessDto>();
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.Address.Street, x => x.MapFrom(x => x.Street))
                .ForMember(dest => dest.Address.State, x => x.MapFrom(x => x.State))
                .ForMember(dest => dest.Address.Country, x => x.MapFrom(x => x.Country));
            CreateMap<PhotoUploadDto, Photo>();
            CreateMap<Photo, PhotoToReturnDto>();
        }
    }
}
