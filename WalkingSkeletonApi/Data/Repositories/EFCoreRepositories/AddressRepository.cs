using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Data.EFCore;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.EFCoreRepositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly WalkingSkeletonDbContext _ctx;

        public AddressRepository(WalkingSkeletonDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<bool> Add<T>(T entity)
        {
            _ctx.Add(entity);
            return SaveChanges();
        }

        public Task<bool> Delete<T>(T entity)
        {
            _ctx.Remove(entity);
            return SaveChanges();
        }

        public Task<bool> Edit<T>(T entity)
        {
            _ctx.Update(entity);
            return SaveChanges();
        }

        public async Task<Address> GetAddress(string userId)
        {
            //return await _ctx.Address.Where(x => x.AppUserId == userId).FirstOrDefaultAsync();
            return await _ctx.Address.Include(x => x.AppUser).FirstAsync(x => x.AppUserId == userId);
        }

        public async Task<List<Address>> GetAddresses()
        {
            return await _ctx.Address.ToListAsync();
            //return await _ctx.Address
            //            .Select(x => new {
            //                Id = x.AppUserId,
            //                address = $"{x.Street} {x.State} {x.Country}"
            //            }).OrderByDescending(k => k.Id).ThenByDescending(a => a.address)
            //            .ToListAsync();

            /*
             return await _ctx.Address.GroupBy(i => i.AppUserId).Select(x => new
            {
                addressId = x.Key,
                addressses = x.Count()
            }).ToListAsync();
             */
        }

        public async Task<int> RowCount()
        {
            return await _ctx.Address.CountAsync(); ;
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
