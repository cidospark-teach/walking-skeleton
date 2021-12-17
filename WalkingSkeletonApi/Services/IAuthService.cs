using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;

namespace WalkingSkeletonApi.Services
{
    public interface IAuthService
    {
        Task<LoginCredDto> Login(string email, string password, bool rememberMe);
    }
}
