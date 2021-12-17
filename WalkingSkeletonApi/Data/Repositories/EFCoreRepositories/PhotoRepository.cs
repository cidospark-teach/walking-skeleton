using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Data.EFCore;

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

        public Task<bool> Delete<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
