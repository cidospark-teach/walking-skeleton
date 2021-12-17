using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.Data.Repositories.EFCoreRepositories
{
    public interface IPhotoRepository : ICRUDRepo
    {
        Task<bool> SaveChanges();
    }
}
