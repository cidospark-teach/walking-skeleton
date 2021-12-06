using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.Repositories.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly IADOOperations _ado;
        private readonly SqlConnection _conn;
        private readonly IConfiguration _config;

        public UserRepository(IADOOperations aDOOperations, IConfiguration config)
        {
            _ado = aDOOperations;
            _conn = new SqlConnection(config.GetSection("ConnectionStrings:Default").Value);
            _config = config;
        }

        public async Task<bool> Add<T>(T entity)
        {
            var user = entity as User;
            var stmt = $"INSERT INTO AppUser (id, lastName, firstName, email, passwordHash, passwordSalt)" +
                        $"VALUES('{user.Id}', '{user.LastName}', '{user.FirstName}', '{user.Email}', '{user.PasswordHash}', '{user.PasswordSalt}')";
            try
            {
                if (await _ado.ExecuteForQuery(stmt))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {

            var user = new User();

            string stmt = $"SELECT * FROM {_config.GetSection("Tables:UserTable").Value} WHERE email = '{email}'";

            try
            {
                var response = await _ado.ExecuteForReader(stmt, "id", "firstname", "lastname", "email");

                if (response == null)
                {
                    throw new Exception("No record found");
                }

                user = new User
                {
                    Id = response[0].Values[0],
                    LastName = response[0].Values[1],
                    FirstName = response[0].Values[2],
                    Email = response[0].Values[3]
                };

            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message);
            }

            return user;
        }

        public async Task<List<User>> GetUsers()
        {

            var listOfUsers = new List<User>();

            string stmt = $"SELECT * FROM {_config.GetSection("Tables:UserTable").Value}";

            try
            {
                var response = await _ado.ExecuteForReader(stmt, "id", "firstname", "lastname", "email");

                if(response == null)
                {
                    throw new Exception("No record found");
                }

                foreach (var item in response)
                {
                    //var values = item.Values.ToArray();

                    listOfUsers.Add(new User
                    {
                        Id = item.Values[0],
                        LastName = item.Values[1],
                        FirstName = item.Values[2],
                        Email = item.Values[3]
                    });
                }                                       
            
            }catch(DbException ex)
            {
                throw new Exception(ex.Message);
            }

            return listOfUsers;
        }
    }
}
