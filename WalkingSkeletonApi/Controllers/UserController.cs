using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
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

                var res = Util.BuildResponse(true, "List of users", null, listOfUsersToReturn);
                return Ok(res);
            }
            else
            {
                ModelState.AddModelError("Notfound", "There was not record of users found!");
                var res = Util.BuildResponse<List<UserToReturnDto>>(false, "No results found!", ModelState, null);
                return NotFound(res);
            }

        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(string email)
        {
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

                var res = Util.BuildResponse(true, "User details", null, UserToReturn);
                return Ok(res);
            }
            else
            {
                ModelState.AddModelError("Notfound", $"There was no record found for user with email {user.Email}");
                var res = Util.BuildResponse<List<UserToReturnDto>>(false, "No result found!", ModelState, null);
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
            if (response != null)
            {
                if (response.Item1)
                {
                    var details = new RegisterSuccessDto { UserId = response.Item2, FullName = $"{response.Item3}", Email = response.Item4 };
                    var result1 = Util.BuildResponse(true, "New user added sucessfully!", null, details);
                    return Ok(result1);
                }

                ModelState.AddModelError("Invalid", $"A user already exist with this email: {user.Email}");
                var result2 = Util.BuildResponse<List<UserToReturnDto>>(false, "User already exists!", ModelState, null);
                return BadRequest(result2);
            }

            ModelState.AddModelError("Failed", "New user was not added");
            var res = Util.BuildResponse<List<UserToReturnDto>>(false, "Error adding user!", ModelState, null);
            return BadRequest(res);
           
        }

        [Authorize]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserToUpdateDto model)
        {
            // check if user logged is the one making the changes - only works for system using Auth tokens
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!id.Equals(currentUserId))
            {
                ModelState.AddModelError("Denied", $"You are not allowed to edit another user's details");
                var result2 = Util.BuildResponse<List<UserToReturnDto>>(false, "Access denied!", ModelState, null);
                return BadRequest(result2);
            }

            // Map DTO to User
            var user = new User
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var response = await _userService.EditUser(user);
            if (response != null)
            {
                var userToReturn = new UserToReturnDto
                {
                    Id = response.Id,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Email = response.Email
                };

                var result = Util.BuildResponse(true, "User updated sucessfully!", null, userToReturn);
                return Ok(result);
            }

            ModelState.AddModelError("Failed", "User not updated");
            var res = Util.BuildResponse<List<UserToReturnDto>>(false, "Could not update details of user!", ModelState, null);
            return BadRequest(res);

        }

    }
}
