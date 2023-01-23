using System.Security.Cryptography;
using System.Text;

namespace iBay.Tools
{
    /// <summary>
    /// Classe pour toutes les fonctions pour les passwords
    /// </summary>
    public class Password
    {
        /// <summary>
        /// Fonction du hash password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var byteArray = Encoding.Default.GetBytes(password);
            var hashPassword = sha.ComputeHash(byteArray);
            return Convert.ToBase64String(hashPassword);
        }

    }
}
