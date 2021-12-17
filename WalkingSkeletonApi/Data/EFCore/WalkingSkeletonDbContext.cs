using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.EFCore
{
    public class WalkingSkeletonDbContext : IdentityDbContext<AppUser>
    {
        public WalkingSkeletonDbContext(DbContextOptions<WalkingSkeletonDbContext> options) 
            : base(options) { }

        public DbSet<Address> Address { get; set; }
        public DbSet<Photo> Photos { get; set; }


    }
}
