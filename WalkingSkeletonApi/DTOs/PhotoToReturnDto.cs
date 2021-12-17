using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.DTOs
{
    public class PhotoToReturnDto
    {
        public bool IsMain { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
