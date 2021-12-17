using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService auth)
        {
            _authService = auth;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginDTO model)
        //{
        //    //var response = await _authService.Login(model.email, model.password);
        //    //if (!response.Status)
        //    //    return BadRequest(response);

        //    return Ok();
           
        //}
    }
}
