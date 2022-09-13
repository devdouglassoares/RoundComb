using System.Security.Cryptography;
using System.Text;

namespace Membership.Core.Implementation
{
    public class MD5HashCrypto
    {
        private static readonly ASCIIEncoding Encoder = new ASCIIEncoding();

        public static string GetHash(string input)
        {
            var x = new MD5CryptoServiceProvider();
            byte[] data = Encoder.GetBytes(input);
            data = x.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
