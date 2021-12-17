using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Helpers;
using WalkingSkeletonApi.Services;

namespace WalkingSkeletonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public PhotosController(IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
        }


        // api/Photos/AddPhoto?userId=1
        [AllowAnonymous]
        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto([FromForm] PhotoUploadDto model, string userId)
        {
            //check if user logged is the one making the changes - only works for system using Auth tokens
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!userId.Equals(currentUserId))
            {
                ModelState.AddModelError("Denied", $"You are not allowed to upload photo for another user");
                var result2 = Util.BuildResponse<string>(false, "Access denied!", ModelState, "");
                return BadRequest(result2);
            }

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

        [HttpGet("get-user-photos")]
        public async Task<IActionResult> GetUserPhotos(string userId)
        {
            var photos = await _photoService.GetUserPhotosAsync(userId);
            if(photos == null)
            {
                ModelState.AddModelError("Not found", "No result found for photos");
                return NotFound(Util.BuildResponse<ImageUploadResult>(false, "Result is empty", ModelState, null));
            }

            // map result
            var listOfUsersToReturn = new List<PhotoToReturnDto>();
            foreach(var photo in photos)
            {
                var photosToReturn = _mapper.Map<PhotoToReturnDto>(photo);
                listOfUsersToReturn.Add(photosToReturn);

            }

            return Ok(Util.BuildResponse<List<PhotoToReturnDto>>(true, "List of user's photos", null, listOfUsersToReturn));
        }
    }
}
