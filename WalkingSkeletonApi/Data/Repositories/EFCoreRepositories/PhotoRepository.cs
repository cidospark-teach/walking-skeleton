using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Data.EFCore;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.EFCoreRepositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly WalkingSkeletonDbContext _ctx;

        public PhotoRepository(WalkingSkeletonDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> Add<T>(T entity)
        {
            _ctx.Add(entity);
            return await SaveChanges();
        }

        public async Task<bool> Delete<T>(T entity)
        {
            _ctx.Remove(entity);
            return await SaveChanges();
        }
        
        public async Task<bool> Edit<T>(T entity)
        {
            _ctx.Update(entity);
            return await SaveChanges();
        }

        public async Task<Photo> GetPhotoByPublicId(string PublicId)
        {
            return await _ctx.Photos.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.PublicId == PublicId);
        }

        public async Task<List<Photo>> GetPhotos()
        {
            return await _ctx.Photos.ToListAsync();
        }

        public async Task<List<Photo>> GetPhotosByUserId(string UserId)
        {
            return await _ctx.Photos.Where(x => x.AppUserId == UserId).ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
