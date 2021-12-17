using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.EFCoreRepositories
{
    public interface IPhotoRepository : ICRUDRepo
    {
        Task<bool> SaveChanges();
        Task<List<Photo>> GetPhotos();
        Task<Photo> GetPhotoByPublicId(string PublicId);
        Task<List<Photo>> GetPhotosByUserId(string UserId);

    }
}
