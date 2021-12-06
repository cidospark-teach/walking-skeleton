using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.Database
{
    public interface IUserRepository : ICRUDRepo
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserByEmail(string email);
    }
}
