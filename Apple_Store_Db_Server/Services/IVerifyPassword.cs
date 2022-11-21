using System.Security.Cryptography;
using System.Text;

namespace Apple_Store_Db_Server.Services
{
    public interface IVerifyPassword
    {
        bool IsPasswordVerify(string password, byte[] salt, byte[] hash);
    }

    class VerifyPassword : IVerifyPassword
    {
        public bool IsPasswordVerify(string password, byte[] salt, byte[] hash)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }
    }
}

