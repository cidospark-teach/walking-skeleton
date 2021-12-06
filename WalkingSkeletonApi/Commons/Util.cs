using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalkingSkeletonApi.Commons
{
    public static class Util
    {
       
        public static List<byte[]> HashGenerator(string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            
            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hash.Key;
                passwordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            var result = new List<byte[]>();
            result.Add(passwordHash);
            result.Add(passwordSalt);
            return result;
        }

        public static bool CompareHash(string password,byte[] passwordHash, byte[] passwordSalt)
        {
            using var hash = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var genhash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < genhash.Length; i++)
                {
                    if (genhash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }
    }
}
