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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService user)
        {
            _userService = user;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            try
            {
                var response = await _userService.Login(model.email, model.password);
                if (!response.status)
                    return BadRequest("Error loging in ");
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                
            }
           
           
        }
    }
}
