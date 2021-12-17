using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WalkingSkeletonApi.Data.Repositories.EFCoreRepositories;
using Microsoft.Extensions.Options;
using WalkingSkeletonApi.Helpers;
using Microsoft.EntityFrameworkCore;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userMgr;
        private readonly IPhotoRepository _photoRepo;

        public PhotoService(IOptions<CloudinarySettings> config,
            IMapper mapper, UserManager<AppUser> userManager,
            IPhotoRepository photoRepository)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
            _mapper = mapper;
            _userMgr = userManager;
            _photoRepo = photoRepository;
        }

        public async Task<Tuple<bool, PhotoUploadDto>> UploadPhotoAsync(PhotoUploadDto model, string userId)
        {
            var uploadResult = new ImageUploadResult();

            using (var stream = model.Photo.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams
                {
                    File = new FileDescription(model.Photo.FileName, stream),
                    Transformation = new Transformation().Width(300).Height(300).Gravity("face").Crop("fill")
                };

                uploadResult = await _cloudinary.UploadAsync(imageUploadParams);
            }

            var status = uploadResult.StatusCode.ToString();

            if (status.Equals("OK"))
            {
                model.PublicId = uploadResult.PublicId;
                model.Url = uploadResult.Url.ToString();
                return new Tuple<bool, PhotoUploadDto>(true, model);
            }

            return new Tuple<bool, PhotoUploadDto>(false, model);

        }

        public async Task<Tuple<bool, PhotoUploadDto>> AddPhotoAsync(PhotoUploadDto model, string userId)
        {   
            var user = await _userMgr.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == userId);

            var photo = _mapper.Map<Photo>(model);
            photo.AppUserId = userId;

            if (!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            // add photo to database
            var res = await _photoRepo.Add(photo);

            return new Tuple<bool, PhotoUploadDto>(res, model);
        }
    }
}
