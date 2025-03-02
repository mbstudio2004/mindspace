using System;
using System.Security.Cryptography;
using System.Text;

namespace Nocci.Utils
{
    public static class GuidExtensions
    {
        public static Guid GenerateGuidFromSteamId(string steamId)
        {
            // Convert the Steam ID to bytes
            var steamIdBytes = Encoding.UTF8.GetBytes(steamId);

            // Create an instance of MD5
            using var md5 = MD5.Create();
            // Compute the hash from the Steam ID bytes
            var hash = md5.ComputeHash(steamIdBytes);

            // Convert the hash to a GUID
            var guid = new Guid(hash);

            return guid;
        }
    }
}