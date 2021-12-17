using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.DTOs
{
    public class RegisterSuccessDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

    }
}
