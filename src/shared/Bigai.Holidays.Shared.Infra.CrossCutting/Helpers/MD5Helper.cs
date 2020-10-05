using System;
using System.Text;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Helpers
{
    /// <summary>
    /// <see cref="MD5Helper"/> Contains methods for hashing string based on the MD5 algorithm.
    /// </summary>
    public static class MD5Helper
    {
        /// <summary>
        /// Determines a hash string for the value.
        /// </summary>
        /// <param name="value">Base value for hashing.</param>
        /// <returns>Returns hash string for the value.</returns>
        public static string ToMD5HashString(this string value)
        {
            byte[] hash = GenerateHashMD5(value);
            string hashString = "";

            if (hash != null)
                hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return hashString;
        }

        private static byte[] GenerateHashMD5(this string value)
        {
            byte[] hash = null;

            if (!string.IsNullOrEmpty(value))
            {
                value = value.RemoveChars();
                value = value.NormalizeChar();

                value.Normalize();

                byte[] byteArray = Encoding.ASCII.GetBytes(value);

                using var md5 = System.Security.Cryptography.MD5.Create();
                hash = md5.ComputeHash(byteArray);
            }

            return hash;
        }

        private static string RemoveChars(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace(".", "")
                    .Replace("-", "")
                    .Replace("/", "")
                    .Replace("|", "")
                    .Replace(" ", "")
                    .ToUpper()
                    .Trim();
            }

            return value;
        }

        private static string NormalizeChar(this string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                string[] withAccent = new string[] { "À", "Á", "Ã", "Â", "Ä", "à", "á", "ã", "â", "ä", "È", "É", "Ê", "Ë", "è", "é", "ê", "ë", "Ì", "Í", "Î", "Ï", "ì", "í", "î", "ï", "Ò", "Ó", "Õ", "Ô", "Ö", "ò", "ó", "õ", "ô", "ö", "Ù", "Ú", "Û", "Ü", "ù", "ú", "û", "ü", "Ý", "ý", "Ñ", "ñ", "ç", "Ç" };
                string[] noneAccent = new string[] { "A", "A", "A", "A", "A", "a", "a", "a", "a", "a", "E", "E", "E", "E", "e", "e", "e", "e", "I", "I", "I", "I", "i", "i", "i", "i", "O", "O", "O", "O", "O", "o", "o", "o", "o", "o", "U", "U", "U", "U", "u", "u", "u", "u", "Y", "y", "N", "n", "c", "C" };

                for (int i = 0, j = withAccent.Length; i < j; i++)
                {
                    valor = valor.Replace(withAccent[i], noneAccent[i]);
                }
            }
            return valor;
        }
    }
}
