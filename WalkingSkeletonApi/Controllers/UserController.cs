using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Helpers;
using WalkingSkeletonApi.Models;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userMgr;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, 
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _logger = logger;
            _userMgr = userManager;
            _mapper = mapper;
        }

       
        [HttpGet("get-users")]
        public IActionResult GetUsers(int page, int perPage)
        {
            // map data from db to dto to reshape it and remove null fields
            var listOfUsersToReturn = new List<UserToReturnDto>();
            
            //var users = _userService.Users;
            var users = _userMgr.Users.ToList();

            if(users != null)
            {
                var pagedList = PagedList<AppUser>.Paginate(users, page, perPage);
                foreach(var user in pagedList.Data)
                {
                    listOfUsersToReturn.Add(_mapper.Map<UserToReturnDto>(user));
                }

                var res = new PaginatedListDto<UserToReturnDto>
                {
                    MetaData = pagedList.MetaData,
                    Data = listOfUsersToReturn
                };

                return Ok(Util.BuildResponse(true, "List of users", null, res));
            }
            else
            {
                ModelState.AddModelError("Notfound", "There was no record for users found!");
                var res = Util.BuildResponse<List<UserToReturnDto>>(false, "No results found!", ModelState, null);
                return NotFound(res);
            }

        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(string email)
        {
            // map data from db to dto to reshape it and remove null fields
            var UserToReturn = new UserToReturnDto();
            //var user = await _userService.GetUser(email);
            var user = await _userMgr.FindByEmailAsync(email);
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
            // map data from model to user
            var user = _mapper.Map<AppUser>(model);
            var response = await _userMgr.CreateAsync(user, model.Password);

            if (!response.Succeeded)
            {
                foreach(var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user!", ModelState, null));
            }

            var res = await _userMgr.AddToRoleAsync(user, "Regular");

            if (!res.Succeeded)
            {
                foreach (var err in response.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return BadRequest(Util.BuildResponse<string>(false, "Failed to add user role!", ModelState, null));
            }

            // if you system requires user's email to be confirmed before they can login 
            // you can generate confirm email token here
            // but ensure AddDefaultTokenProviders() have been enabled in startup else there won't be token generated
            var token = await _userMgr.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("ConfrimEmail", "User", new { Email = user.Email, Token = token }, Request.Scheme);  // this is the url to send

            // next thing TODO here is to send an email to this new user to the email provided using a notification service you should build

            // map data to dto
            var details = _mapper.Map<RegisterSuccessDto>(user);

            return Ok(Util.BuildResponse(true, "New user added!", null, details));

           
        }



    }
}
