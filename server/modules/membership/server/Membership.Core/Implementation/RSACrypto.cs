using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Membership.Core.Implementation
{
    public class RSACrypto
    {
        private const string PrivateKey = "<RSAKeyValue><Modulus>8HU7bFleNl8WUY+xfwom60Fyo6JhzWrK3QYvhGgQebzoDBXfkUxgew4G1+FU9i3YWfjSyDGjaBK/OA3ESlfSq27FD2/cK4fT+1CGfDJ83aG1le7Qp6CmoM9dOkvMOs1ORcm632cBOPgLrFSp0lhOZ0cQgTeoafdoEcJNjdlzngs=</Modulus><Exponent>EQ==</Exponent><P>+v/T6yP+6GO2VlHbCrSYlNy9aicA3e3Ib5/pWj4czvEjViisk1wEIdjwiDAd5qFIlkHA8Lfdmh1dOPmpJAO/AQ==</P><Q>9T+j2on2Hm41TaDabcRxvrZ14AnR7/w6k4UPFzMcu1op4UlUllbICAeVE2JYKrpauVMYdbqpmIijRfTrdvNpCw==</Q><DP>k6WLt36V8hyJX/PqQohZwPpRTYBa3OY5qxLFgGDFpugy52M4Vq6ZBNn25rLkaYwMlJ8mMz760yBU9FafutUG8Q==</DP><DQ>c2k+DH0ohspzb9M5nREmd91kpapErStm3AJhdFRJwZPXeRNzGZJAA8dVNkxlucEbooF07BubGpqnL/rJKOsEQQ==</DQ><InverseQ>iRzpW292d/S68CpZdJt5iwIWaLRR/5KPjhIecmnn+WONGpx2WBRW2olTX9pN8sYnOMFST8WBtcYTlELNSrd1vA==</InverseQ><D>RrkRejhnAO7KVDlSUoqD6tcDmYocw/I7qms7JvFuQexEP8oyskOj5/URElFVOVjHKYVrK9JdPLo4PamyM/u2bhYnSrmVHxbifRSj7yemxa2RpIrFmPKv8i0BfMfAachqSRv3UGxiXDyjR5/ZsbqDowyNx/I9BnYa9gTBlJnoXvE=</D></RSAKeyValue>";
        private const string PublicKey = "<RSAKeyValue><Modulus>8HU7bFleNl8WUY+xfwom60Fyo6JhzWrK3QYvhGgQebzoDBXfkUxgew4G1+FU9i3YWfjSyDGjaBK/OA3ESlfSq27FD2/cK4fT+1CGfDJ83aG1le7Qp6CmoM9dOkvMOs1ORcm632cBOPgLrFSp0lhOZ0cQgTeoafdoEcJNjdlzngs=</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>";
        private static readonly UnicodeEncoding Encoder = new UnicodeEncoding();

        public static string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = StringToByteArray(data);
            var dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(PrivateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return Encoder.GetString(decryptedByte);
        }

        public static string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicKey);
            var dataToEncrypt = Encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();

            return ByteArrayToString(encryptedByteArray);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
            // return Convert.FromBase64String(hex);
        }

        private static string ByteArrayToString(byte[] bytes)
        {
            return bytes.Select(c => c.ToString("x2")).Aggregate((s, c) => s + c);
            //return Convert.ToBase64String(bytes);
        }
    }
}
