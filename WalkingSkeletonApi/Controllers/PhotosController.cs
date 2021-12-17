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


        // this route is only open to logged-in users
        // api/Photos/add-photo?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
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

        // this route is also open to users that are not logged-in
        // api/Photos/get-user-photos?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
        [HttpGet("get-user-photos")]
        [AllowAnonymous]
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

        // this route is only open to logged-in users
        // api/Photos/get-user-main-photo?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
        [HttpGet("get-user-main-photo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserMainPhoto(string userId)
        {
            var photo = await _photoService.GetUserMainPhotoAsync(userId);
            if (photo == null)
            {
                ModelState.AddModelError("Not found", "No result found for main photo");
                return NotFound(Util.BuildResponse<ImageUploadResult>(false, "Result is empty", ModelState, null));
            }

            // map result
            var photosToReturn = _mapper.Map<PhotoToReturnDto>(photo);


            return Ok(Util.BuildResponse<PhotoToReturnDto>(true, "User's main photo", null, photosToReturn));
        }


        // this route is only open to logged-in users
        // api/Photos/set-main-photo/kbumxhsvbyqyebrtagmk?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
        [HttpPatch("set-main-photo/{publicId}")]
        public async Task<IActionResult> SetMainPhoto(string userId, string publicId)
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

            var res = await _photoService.SetMainPhotoAsync(userId, publicId);
            if (!res.Item1)
            {
                ModelState.AddModelError("Failed", "Could not set main photo");
                return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "Set main failed!", ModelState, null));
            }

            return Ok(Util.BuildResponse<string>(true, "Main photo is set sucessfully!", null, res.Item2));

        }

        // this route is only open to logged-in users
        // api/Photos/unset-main-photo?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
        [HttpPatch("unset-main-photo")]
        public async Task<IActionResult> UnsetMainPhoto(string userId)
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

            var res = await _photoService.UnSetMainPhotoAsync(userId);
            if (!res)
            {
                ModelState.AddModelError("Failed", "Could not unset main photo");
                return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "Unset failed!", ModelState, null));
            }

            return Ok(Util.BuildResponse<string>(true, "Unset main photo is sucessful!", null, ""));

        }

        // this route is only open to logged-in users
        // api/Photos/delete-photo/publicId=kbumxhsvbyqyebrtagmk?userId=d274fa2f-201a-41f9-8a89-f17c4f07544d
        [HttpDelete("delete-photo/{publicId}")]
        public async Task<IActionResult> DeletePhoto(string userId, string publicId)
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

            var res = await _photoService.DeletePhotoAsync(publicId);
            if (!res)
            {
                ModelState.AddModelError("Failed", "Could not delete photo");
                return BadRequest(Util.BuildResponse<ImageUploadResult>(false, "Delete failed!", ModelState, null));
            }

            return Ok(Util.BuildResponse<string>(true, "Photo deleted sucessful!", null, ""));

        }
    }
}


//"id": "d274fa2f-201a-41f9-8a89-f17c4f07544d",
// "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJkMjc0ZmEyZi0yMDFhLTQxZjktOGE4OS1mMTdjNGYwNzU0NGQiLCJ1bmlxdWVfbmFtZSI6Ik1jZGFuaWVsIEZseW5uIiwiZW1haWwiOiJtY2RhbmllbGZseW5uQGdhbGxheGlhLmNvbSIsInJvbGUiOiJSZWd1bGFyIiwibmJmIjoxNjM5Nzc3MDI2LCJleHAiOjE2Mzk3ODIwMDAsImlhdCI6MTYzOTc3NzAyNn0.NAoZYRMHJbVbLsVPoEDZ6HmxIreB_d7UwLLR3t1oijk",