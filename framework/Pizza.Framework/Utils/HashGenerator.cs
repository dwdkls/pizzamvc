using System.Security.Cryptography;
using System.Text;

namespace Pizza.Framework.Utils
{
    public class HashGenerator
    {
        public static string Generate(string password)
        {
            var sha512 = new SHA512Managed();
            byte[] hashValue = sha512.ComputeHash(Encoding.Unicode.GetBytes(password));

            var sb = new StringBuilder();
            foreach (byte x in hashValue)
            {
                sb.Append(x.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}