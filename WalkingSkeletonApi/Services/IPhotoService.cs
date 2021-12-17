using System;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;

namespace WalkingSkeletonApi.Services
{
    public interface IPhotoService
    {
        public Task<Tuple<bool, PhotoUploadDto>>  UploadPhotoAsync(PhotoUploadDto model, string userId);
        public Task<Tuple<bool, PhotoUploadDto>> AddPhotoAsync(PhotoUploadDto model, string userId);
    }
}
