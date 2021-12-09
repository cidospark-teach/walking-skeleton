using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

       
        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            var res = new ResponseDto<List<UserToReturnDto>>();

            // map data from db to dto to reshape it and remove null fields
            var listOfUsersToReturn = new List<UserToReturnDto>();
            var users = _userService.Users;
            if(users != null)
            {
                foreach(var user in users)
                {
                    listOfUsersToReturn.Add(new UserToReturnDto {
                        Id = user.Id,
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        Email = user.Email
                    });
                }
                res.Status = true;
                res.Message = "List of users!";
                res.Data = listOfUsersToReturn;
                return Ok(res);
            }
            else
            {
                res.Status = false;
                res.Message = "No results found!";
                res.Errors.Add( new ErrorItem { Key = "Notfound", 
                    ErrorMessages = new List<string> { "No result found for users" } });
                return NotFound(res);
            }

        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(string email)
        {
            var res = new ResponseDto<UserToReturnDto>();

            // map data from db to dto to reshape it and remove null fields
            var UserToReturn = new UserToReturnDto();
            var user = await _userService.GetUser(email);
            if (user != null)
            {                
                UserToReturn = new UserToReturnDto
                {
                    Id = user.Id,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email
                };

                res.Status = true;
                res.Message = "User detail!";
                res.Data = UserToReturn;
                return Ok(res);
            }
            else
            {
                res.Status = false;
                res.Message = "No results found!";
                res.Errors.Add(new ErrorItem
                {
                    Key = "Notfound",
                    ErrorMessages = new List<string> { "No result found for user" }
                });

                return NotFound(res);
            }

        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser(RegisterDto model)
        {
            // Map DTO to User
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };            

            var response = await _userService.Register(user, model.Password);
            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
           
        }
    }
}
