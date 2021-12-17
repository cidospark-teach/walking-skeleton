using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.EFCoreRepositories
{
    public interface IAddressRepository : ICRUDRepo
    {
        Task<List<Address>> GetAddresses();
        Task<Address> GetAddress(string userId);
        Task<bool> SaveChanges();
        Task<int> RowCount();
    }
}
