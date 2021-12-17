using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Data.Repositories.EFCoreRepositories;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepo;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepo = addressRepository;
        }

        public List<Address> Addresses => _addressRepo.GetAddresses().Result;

        public async Task<bool> AddAddress(Address address)
        {
            return await _addressRepo.Add(address);
        }

        public async Task<bool> DeleteAddress(string addressId)
        {
            var addressToDelete = await _addressRepo.GetAddress(addressId);
            if (addressToDelete == null)
                return false;

            return await _addressRepo.Delete(addressToDelete);
        }

        public async Task<Address> EditAddress(AddressToEditDto address)
        {
            var addressToEdit = await _addressRepo.GetAddress(address.AppUserId);

            addressToEdit.Street = address.Street;
            addressToEdit.State = address.State;
            addressToEdit.Country = address.Country;


            if (await _addressRepo.Edit(address))
                return addressToEdit;

            else return null;
        }

        public async Task<Address> GetAddress(string userId)
        {
            return await _addressRepo.GetAddress(userId);
        }
    }
}
