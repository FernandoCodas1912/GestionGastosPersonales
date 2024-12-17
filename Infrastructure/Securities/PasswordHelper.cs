

using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Securities
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                foreach(byte t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }

                return builder.ToString();


            }
        }
    }
}
