using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public interface IPhotoService
    {
        public Task<Tuple<bool, PhotoUploadDto>>  UploadPhotoAsync(PhotoUploadDto model, string userId);
        public Task<Tuple<bool, PhotoUploadDto>> AddPhotoAsync(PhotoUploadDto model, string userId);
        public Task<List<Photo>> GetUserPhotosAsync(string userId);
        public Task<Photo> GetUserMainPhotoAsync(string userId);
        public Task<Tuple<bool, string>> SetMainPhotoAsync(string userId, string PublicId);
        public Task<bool> UnSetMainPhotoAsync(string userId);
        public Task<bool> DeletePhotoAsync(string PublicId);

    }
}
