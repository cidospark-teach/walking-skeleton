using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Data.EFCore
{
    public class SeederClass
    {
        private readonly WalkingSkeletonDbContext _ctx;
        private readonly UserManager<AppUser> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public SeederClass(WalkingSkeletonDbContext ctx,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userMgr = userManager;
            _roleMgr = roleManager;
        }

        public async Task SeedMe()
        {
            _ctx.Database.EnsureCreated();

            try
            {
                var roles = new string[] { "Regular", "Admin" };
                if (!_roleMgr.Roles.Any())
                {
                    foreach (var role in roles)
                    {
                        await _roleMgr.CreateAsync(new IdentityRole(role));
                    }
                }

                var data = System.IO.File.ReadAllText("Data/EFCore/SeedData.json");
                var ListOfAppUsers = JsonConvert.DeserializeObject<List<AppUser>>(data);

                if (!_userMgr.Users.Any())
                {
                    var counter = 0;
                    var role = "";
                    foreach (var user in ListOfAppUsers)
                    {
                        user.UserName = user.Email;
                        role = counter < 1 ? roles[1] : roles[0]; // tenary operator

                        var res = await _userMgr.CreateAsync(user, "P@ssw0rd");
                        if (res.Succeeded)
                            await _userMgr.AddToRoleAsync(user, role);

                        counter++;
                    }
                }
            }catch(DbException)
            {
                //log err
            }


        }
    }
}
