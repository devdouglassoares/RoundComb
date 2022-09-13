using System;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace RoundComb.Crypt
{
    public static class Encription
    {
        #region Desencriptação

        public static string X509EncryptString(string certificateName, string stringToEncrypt)
        {
            string cert = certificateName;
            X509Certificate2 x509_2 = LoadCertificate(StoreLocation.LocalMachine, cert);

            string encryptedKey = X509Encrypt(x509_2, stringToEncrypt);

            return encryptedKey;
        }

        private static string X509Encrypt(X509Certificate2 x509, string stringToEncrypt)
        {
            if (x509 == null || string.IsNullOrEmpty(stringToEncrypt))
                throw new Exception("A x509 certificate and string for encryption must be provided");

            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PublicKey.Key;
            byte[] bytestoEncrypt = ASCIIEncoding.ASCII.GetBytes(stringToEncrypt);
            byte[] encryptedBytes = rsa.Encrypt(bytestoEncrypt, false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string X509DecryptString(string certificateName, string stringToDecript)
        {
            string cert = certificateName;
            X509Certificate2 x509_2 = LoadCertificate(StoreLocation.LocalMachine, cert);

            string decryptedKey = X509Decrypt(x509_2, stringToDecript);

            return decryptedKey;
        }

        private static X509Certificate2 LoadCertificate(StoreLocation storeLocation, string certificateName)
        {
            X509Store store = new X509Store(storeLocation);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = store.Certificates;
            X509Certificate2 x509 = null;
            foreach (X509Certificate2 c in certCollection)
            {
                if (c.Subject == certificateName)
                {
                    x509 = c;
                    break;
                }
            }
            store.Close();
            return x509;
        }

        private static string X509Decrypt(X509Certificate2 x509, string stringTodecrypt)
        {
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)x509.PrivateKey;
            byte[] bytestodecrypt = Convert.FromBase64String(stringTodecrypt);
            byte[] plainbytes = rsa.Decrypt(bytestodecrypt, false);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(plainbytes);
        }

        public static string EncriptedString(string decriptedstring, string encriptKey, string initVector, int keysize)
        {
            string setting = string.Empty;

            setting = GetEncryptString(initVector, keysize, decriptedstring, encriptKey);

            return setting;

        }

        public static string WrapperEncryptString(string input, string pKey)
        {
            string invector = pKey.Substring(0, 16);

            return Encription.EncriptedString(input, pKey, invector, 256);
        }

        private static string GetEncryptString(string initVector, int keysize, string decriptedstring, string encriptKey)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(decriptedstring);
            PasswordDeriveBytes password = new PasswordDeriveBytes(encriptKey, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptString(string encryptedString, string encriptKey, string initVector, int keysize)
        {
            string setting = string.Empty;

            setting = GetDecryptedSetting(initVector, keysize, encryptedString, encriptKey);

            return setting;
        }

        public static string WrapperDecryptString(string input, string pKey)
        {
            string invector = pKey.Substring(0, 16);

            return Encription.DecryptString(input, pKey, invector, 256);
        }

        //public static string EncryptString(string pInitVector, int pKeySize, string plainText, string encriptKey)
        //{
        //    byte[] initVectorBytes = Encoding.UTF8.GetBytes(pInitVector);
        //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    PasswordDeriveBytes password = new PasswordDeriveBytes(encriptKey, null);
        //    byte[] keyBytes = password.GetBytes(pKeySize / 8);
        //    RijndaelManaged symmetricKey = new RijndaelManaged();
        //    symmetricKey.Mode = CipherMode.CBC;
        //    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        //    MemoryStream memoryStream = new MemoryStream();
        //    CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        //    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        //    cryptoStream.FlushFinalBlock();
        //    byte[] cipherTextBytes = memoryStream.ToArray();
        //    memoryStream.Close();
        //    cryptoStream.Close();
        //    return Convert.ToBase64String(cipherTextBytes);
        //}

        private static string GetDecryptedSetting(string pInitVector, int pKeySize, string pEncryptedSetting, string encriptKey)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(pInitVector);
            byte[] cipherTextBytes = Convert.FromBase64String(pEncryptedSetting);
            PasswordDeriveBytes password = new PasswordDeriveBytes(encriptKey, null);
            byte[] keyBytes = password.GetBytes(pKeySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static void fileEncrip(string pathFileName)
        {
            File.Encrypt(pathFileName);
        }

        public static void fileDecript(string pathFileName)
        {
            File.Decrypt(pathFileName);
        }

        public static string FileToBase64(string fileName)
        {
            using (FileStream reader = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[reader.Length];
                reader.Read(buffer, 0, (int)reader.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion desencriptação

        #region app/web config
        public static string GetCustomConfigKey(string section, string key)
        {
            string val = string.Empty;
            try
            {
                var collKeys = ConfigurationManager.GetSection(section) as NameValueCollection;
                if (collKeys != null)
                {
                    val = collKeys[key].ToString();
                }
            }
            catch
            {
            }
            return val;
        }

        #endregion app/web config
    }
}
