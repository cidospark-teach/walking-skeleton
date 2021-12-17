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
            CreateMap<RegisterDto, AppUser>();
            CreateMap<PhotoUploadDto, Photo>();
            CreateMap<Photo, PhotoToReturnDto>();
        }
    }
}
