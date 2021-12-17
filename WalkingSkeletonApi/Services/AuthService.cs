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

        public AuthService(UserManager<AppUser> userManager, IJWTService jWTService)
        {
        }
        //public async Task<ResponseDto<LoginCredDto>> Login(string email, string password)
        //{
        //    // the code below has no much importance now that LoginDto have been added data annotations
        //    #region removable code
        //    if (String.IsNullOrWhiteSpace(email))
        //        throw new Exception("Email is empty");
        //    if (String.IsNullOrWhiteSpace(password))
        //        throw new Exception("Password is empty");
        //    #endregion

        //    var loginCred = new LoginCredDto();
        //    var res = new ResponseDto<LoginCredDto>();
        //    List<string> roles = new List<string>();
        //    roles.Add("admin");
        //    try
        //    {
        //        var response = await _userRepo.GetUserByEmail(email);
        //        if (Util.CompareHash(password, response.PasswordHash, response.PasswordSalt))
        //        {
        //            loginCred.Id = response.Id;
        //            loginCred.token = _jWTService.GenerateToken(response, roles);

        //            res.Status = true;
        //            res.Message = "Login sucessfully!";
        //            res.Data = loginCred;
        //        }
        //        else
        //        {
        //            res.Status = false;
        //            res.Message = "Login failed!";
        //            res.Data = null;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        //Log error
        //    }
        //    return res;

        //}
    }
}
