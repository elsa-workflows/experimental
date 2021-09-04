using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Elsa.Contracts;

namespace Elsa.Services
{
    public class Hasher : IHasher
    {
        public string Hash(object value)
        {
            var json = JsonSerializer.Serialize(value);
            return Hash(json);
        }
        
        private static string Hash(string input)
        {
            using var sha = new HMACMD5();
            return Hash(sha, input);
        }

        private static string Hash(HashAlgorithm hashAlgorithm, string input)
        {
            var data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(data);
        }
    }
}