using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.DTOs
{
    public class AddressToEditDto
    {
        public string AppUserId { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
