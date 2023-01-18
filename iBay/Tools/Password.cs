using System.Security.Cryptography;
using System.Text;

namespace iBay.Tools
{
    public class Password
    {
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var byteArray = Encoding.Default.GetBytes(password);
            var hashPassword = sha.ComputeHash(byteArray);
            return Convert.ToBase64String(hashPassword);
        }

    }
}
