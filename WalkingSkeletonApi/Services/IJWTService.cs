using System;
using System.Collections.Generic;
using WalkingSkeletonApi.Models;

namespace WalkingSkeletonApi.Services
{
    public interface IJWTService
    {
        string GenerateToken(AppUser user, List<string> userRoles);
    }
}
