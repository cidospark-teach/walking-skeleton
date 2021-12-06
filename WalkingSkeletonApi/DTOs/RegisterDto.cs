using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.DTOs
{
    public class RegisterDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
