using System.Security.Cryptography;
using System.Text;

namespace ProjectFruit.Helpers
{
    public class MD5Helper
    {
        public static string HashstringMd5(string value) {

            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

        public static string CheckMD5(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] enteredHash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            // Convert entered hash to string for comparison with stored hash
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < enteredHash.Length; i++)
            {
                builder.Append(enteredHash[i].ToString("x2"));
            }
            string enteredHashString = builder.ToString();

            return enteredHashString;
        }
    }
}
