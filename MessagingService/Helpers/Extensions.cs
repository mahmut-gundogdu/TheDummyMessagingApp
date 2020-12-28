using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Pluralize.NET;

namespace MessagingService
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
        
 

        public static string ToHash(this string plainText)
        {
            using var sha256 = SHA256.Create();
            var bytes = plainText.ToByteArray();
            return sha256.ComputeHash(bytes)
                .Select(x => x.ToString("x2"))
                .JoinWith(string.Empty);
        }

        private static byte[] ToByteArray(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        private static string JoinWith(this IEnumerable<string> text, string separator)
        {
            return string.Join(separator, text);
        }

        public static string Pluralize(this string word)
        {
            var pluralizer = new Pluralizer();
            return pluralizer.IsPlural(word) ? word : pluralizer.Pluralize(word);
        }
    }
}