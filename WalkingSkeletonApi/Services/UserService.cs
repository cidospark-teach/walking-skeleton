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

        public async Task<User> GetUser(string email)
        {
            var user = new User();
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

        public async Task<ResponseDto<RegisterSuccessDto>> Register(User user, string password)
        {

            var res = new ResponseDto<RegisterSuccessDto>();
            var userFromDb = await _userRepo.GetUserByEmail(user.Email);
            if(userFromDb != null)
            {
                res.Status = false;
                res.Message = "User already exist!";
                res.Errors.Add(new ErrorItem {
                    Key = "Invalid",
                    ErrorMessages = new List<string> { $"A user already exist with this email: {user.Email}" }
                });
                return res;
            }

            var listOfHash = Util.HashGenerator(password);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = listOfHash[0];
            user.PasswordSalt = listOfHash[1];


            try
            {                
                if(await _userRepo.Add<User>(user))
                {
                    res.Status = true;
                    res.Data = new RegisterSuccessDto { UserId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email
                    };
                    res.Message = "New user added sucessfully!";
                }
                else
                {
                    res.Status = false;
                    res.Message = "Error adding user!";
                    res.Errors.Add(new ErrorItem { Key = "Failed",
                        ErrorMessages = new List<string> { $"New user was not added" }
                    });
                }
            }
            catch (Exception ex)
            {
                // Log Error
            }
            return res;
        }
    }
}
