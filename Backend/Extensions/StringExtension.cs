using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Email.Management.Extensions
{
    public static class StringExtension
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string ToSha256(this string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string ToSha512(this string input)
        {
            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string ToMd5(this string input)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var t in hash)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }
    }
}
