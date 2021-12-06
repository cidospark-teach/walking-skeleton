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
        private readonly IJWTService _jWTService;

        public UserService(IUserRepository userRepository, IJWTService jWTService)
        {
            _userRepo = userRepository;
            _jWTService = jWTService;
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


            public async Task<LoginSuccess> Login(string email, string password)
        {
            if (String.IsNullOrWhiteSpace(email)) 
                throw new Exception("Email is empty");
            if (String.IsNullOrWhiteSpace(password))
                throw new Exception("Password is empty");
           
            LoginSuccess success = new LoginSuccess();
            List<string> roles = new List<string>();
            roles.Add("admin");
            try
            {
                var response = await _userRepo.GetUserByEmail(email);
                if (Util.CompareHash(password, response.PasswordHash, response.PasswordSalt))
                {
                    success.status = true;
                    success.Id = response.Id;
                    success.token = _jWTService.GenerateToken(response, roles);
                }

                    
            }
            catch (Exception e)
            {
                //Log error

                throw new Exception(e.Message);
            }
            return success;
            
        }

        public async Task<RegisterSuccessDto> Register(User user, string password)
        {
            var res = _userRepo.GetUserByEmail(user.Email);
            var success = new RegisterSuccessDto();
            //if (res.Result != null)
            //{
            //    throw new Exception("Email already exist!");
            //}

            success.Status = false;
            var listOfHash = Util.HashGenerator(password);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = listOfHash[0];
            user.PasswordSalt = listOfHash[1];
            try
            {                
                if(await _userRepo.Add<User>(user))
                {
                    success.Status = true;
                    success.FullName = $"{user.FirstName} {user.LastName}";
                    success.Email = user.Email;
                }
            }
            catch (Exception)
            {
                // Log Error
            }
            return success;
        }
    }
}
