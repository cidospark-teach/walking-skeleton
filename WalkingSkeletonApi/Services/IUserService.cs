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
        Task<RegisterSuccessDto> Register(User user, string password);
        Task<LoginSuccess> Login(string email, string password);
        Task<User> GetUser(string email);
    }
}
