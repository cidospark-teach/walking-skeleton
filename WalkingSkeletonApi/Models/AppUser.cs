using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.Models
{
    public class AppUser : IdentityUser
    {

        [Required]
        [StringLength(15, MinimumLength =3, ErrorMessage ="Must be between 3 and 15")]
        public string LastName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Must be between 3 and 15")]
        public string FirstName { get; set; }

        public bool IsActive { get; set; }
        public Address Address { get; set; }
        public List<Photo> Photos { get; set; }

        public AppUser()
        {
            Photos = new List<Photo>();
            Address = new Address();
        }
    }
}
