using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    interface IAddressService
    {
        public List<Address> Addresses { get; }

        Task<bool> AddAddress(Address address);
        Task<Address> GetAddress(string userId);
        Task<Address> EditAddress(AddressToEditDto address);
        Task<bool> DeleteAddress(string addressId);
    }
}
