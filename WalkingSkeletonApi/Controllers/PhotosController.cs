using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
using WalkingSkeletonApi.Data.Repositories.EFCoreRepositories;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Helpers;
using WalkingSkeletonApi.Models;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userMgr;
        private readonly IPhotoService _photoService;

        public PhotosController(IMapper mapper, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _mapper = mapper;
            _userMgr = userManager;
            _photoService = photoService;
        }


        // api/Photos/AddPhoto?userId=1
        //[Authorize(Roles = "Admin, Regular")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddPhoto([FromForm] PhotoUploadDto model, string userId)
        {
            //check if user logged is the one making the changes - only works for system using Auth tokens
            //ClaimsPrincipal currentUser = this.User;
            //var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //if (!userId.Equals(currentUserId))
            //{
            //    ModelState.AddModelError("Denied", $"You are not allowed to upload photo for another user");
            //    var result2 = Util.BuildResponse<List<UserToReturnDto>>(false, "Access denied!", ModelState, null);
            //    return BadRequest(result2);
            //}

            var file = model.Photo;

            if (file.Length > 0)
            {
                var uploadStatus = await _photoService.UploadPhotoAsync(model, userId);

                if (uploadStatus.Item1)
                {
                    var res = await _photoService.AddPhotoAsync(model, userId);
                    if (!res.Item1)
                    {
                        ModelState.AddModelError("Failed", "Could not add photo to database");
                        return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "Failed to add to database", ModelState, null));
                    }

                    return Ok(Util.BuildResponse<object>(true, "Uploaded successfully", null, new { res.Item2.PublicId, res.Item2.Url }));
                }

                ModelState.AddModelError("Failed", "File could not be uploaded to cloudinary");
                return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "Failed to upload", ModelState, null));

            }

            ModelState.AddModelError("Invalid", "File size must not be empty");
            return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "File is empty", ModelState, null));
        }
    }
}
