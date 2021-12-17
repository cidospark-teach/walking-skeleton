using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userMgr;
        private readonly SignInManager<AppUser> _signinMgr;
        private readonly IJWTService _jwtService;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, IJWTService jWTService)
        {
            _userMgr = userManager;
            _signinMgr = signinManager;
            _jwtService = jWTService;
        }

        public async Task<LoginCredDto> Login(string email, string password, bool rememberMe)
        {
            var user = await _userMgr.FindByEmailAsync(email);

            var res = await _signinMgr.PasswordSignInAsync(user, password, rememberMe, false);

            if (!res.Succeeded)
            {
                return new LoginCredDto { status = false };
            }

            // get jwt token
            var userRoles = await _userMgr.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, userRoles.ToList());


            return new LoginCredDto {  status = true, Id = user.Id, token = token };

        }
    }
}
