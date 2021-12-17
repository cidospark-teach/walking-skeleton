using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.DTOs
{
    public class LoginCredDto
    {
        public string Id { get; set; }
        public string token { get; set; }
        public bool status { get; set; }
    }
}
