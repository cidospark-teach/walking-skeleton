using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public interface IUserService
    {
        public List<User> Users { get;}
        Task<ResponseDto<RegisterSuccessDto>> Register(User user, string password);
        Task<User> GetUser(string email);
    }
}
