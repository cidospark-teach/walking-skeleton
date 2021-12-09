using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Commons;
using WalkingSkeletonApi.Data.Repositories.Database;
using WalkingSkeletonApi.DTOs;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepository)
        {
            _userRepo = userRepository;
        }
        public List<User> Users {
            get {
                return _userRepo.GetUsers().Result;
            }
        }

        public async Task<bool> DeleteUser(User user)
        {
            var status = false;
            try
            {
                if (await _userRepo.Delete<User>(user))
                {
                    status = true;
                }
            }catch(Exception ex)
            {
                // log err
            }
            return status;
        }

        public async Task<User> EditUser(User user)
        {
            User res = null;
            try
            {
                var userFromDb = await _userRepo.GetUserByEmail(user.Email);
                if(userFromDb != null)
                {
                    // map new details to user fetched
                    userFromDb.LastName = user.LastName;
                    userFromDb.FirstName = user.LastName;
                    userFromDb.Email = user.LastName;
                }

                if(await _userRepo.Edit<User>(user))
                {
                    res = new User
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    };
                }
                
            }
            catch (Exception ex)
            {
                // log error
            }
            return res;
        }

        public async Task<User> GetUser(string email)
        {
            User user = null;
            try
            {
                user = await _userRepo.GetUserByEmail(email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return user;
        }

        public async Task<Tuple<bool, string, string, string>> Register(User user, string password)
        {

            Tuple<bool, string, string, string> result = null;

            var res = new ResponseDto<RegisterSuccessDto>();
            var userFromDb = await _userRepo.GetUserByEmail(user.Email);
            if(userFromDb != null)
            {
                result = new Tuple<bool, string, string, string>(
                        false,
                        "User already exist!",
                        "",
                        ""
                    );
                return result;
            }

            var listOfHash = Util.HashGenerator(password);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = listOfHash[0];
            user.PasswordSalt = listOfHash[1];

            try
            {                
                if(await _userRepo.Add<User>(user))
                {
                    result = new Tuple<bool, string, string, string>(
                        true,
                        user.Id, 
                        $"{user.FirstName} {user.LastName}", 
                        user.Email
                    );
                }
            }
            catch (Exception ex)
            {
                // Log Error
            }
            return result;
        }
    }
}
