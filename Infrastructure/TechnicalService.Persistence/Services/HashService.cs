using Konscious.Security.Cryptography;
using PasswordGenerator;
using System.Security.Cryptography;
using System.Text;
using TechnicalService.Application.Contracts.ServicesContracts;

namespace TechnicalService.Persistence.Services
{
    public class HashService : IHashService
    {
        private const int SaltSize = 16; // 128-bit tuz
        private const int HashLength = 64; // 512-bit hash
        private const int Iterations = 4; // CPU maliyeti
        private const int MemorySize = 65536; // 64 MB bellek
        private const int Parallelism = 2; // Paralel işlem

        public (string Hash, string Salt) HashItem(string Item)
        {
            // Tuz (Salt) Üretimi
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Argon2 ile Hash'leme
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(Item));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = Parallelism;
            argon2.Iterations = Iterations;
            argon2.MemorySize = MemorySize;

            byte[] hash = argon2.GetBytes(HashLength);
            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public bool VerifyItem(string requestItem, string hashedItem, string saltItem)
        {
            byte[] storedHash = Convert.FromBase64String(hashedItem);
            byte[] salt = Convert.FromBase64String(saltItem);

            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(requestItem));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = Parallelism;
            argon2.Iterations = Iterations;
            argon2.MemorySize = MemorySize;

            byte[] computedHash = argon2.GetBytes(HashLength);
            return computedHash.SequenceEqual(storedHash);
        }

        public string GeneratePassword()
        {
            var pwdGen = new Password(); //Default olarak 16 karakterlik şifre
            return pwdGen.Next();
        }
    }
}
