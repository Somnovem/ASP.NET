using System.Security.Cryptography;
using System.Text;

namespace Practice_20
{
    public static class Encrypter
    {
        public static string CalculateSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));
            return objUtf8.GetString(hashValue);
        }
    }
}
