﻿using AutoMapper;
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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(u => u.Email))
                .ForMember(dest => dest.Address, x => x.MapFrom(s => new Address {  Street = s.Street, State = s.State, Country = s.Country }));
            CreateMap<PhotoUploadDto, Photo>();
            CreateMap<Photo, PhotoToReturnDto>();
        }
    }
}
