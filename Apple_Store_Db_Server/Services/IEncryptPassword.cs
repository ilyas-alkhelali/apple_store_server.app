using System.Security.Cryptography;
using System.Text;

namespace Apple_Store_Db_Server.Services
{
    public interface IEncryptPassword
    {
        void CreatePasswordHashandSalt(string password, out byte[] salt, out byte[] hash);
    }

    public class EncryptPassword : IEncryptPassword
    {
        public void CreatePasswordHashandSalt(string password, out byte[] salt, out byte[] hash)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
