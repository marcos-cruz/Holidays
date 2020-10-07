using System;
using System.IO;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Determines whether a string has been filled.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns><c>true</c> if the string is diferent from <c>null</c> or empty string, otherwise <c>false</c>.</returns>
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Determines whether a string has been filled with numeric value.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns><c>true</c> if the value is numeric integer, otherwise <c>false</c>.</returns>
        public static bool IsPositiveInteger(this string value)
        {
            if (!value.HasValue()) return false;

            bool isInteger = int.TryParse(value, out int number);

            return isInteger && number > -1;
        }

        /// <summary>
        /// Extract the path file name.
        /// </summary>
        /// <param name="value">The path filename to be testes.</param>
        /// <returns>The name of the file extracted from the path.</returns>
        public static string GetFileNameFromPath(this string value)
        {
            string file = "????????.???";
            if (value.HasValue())
            {
                string[] columns = value.Split('\\');
                if (columns != null && columns.Length > 0)
                {
                    var item = columns.Length - 1;
                    file = columns[item];
                }
            }

            return file;
        }

        public static void RenameFile(string filename)
        {
            if (filename.HasValue())
            {
                DateTime now = DateTime.Now;
                string newFileName = $"Imported-{ now.Year}{now.Month}{now.Day}-{filename}";
                FileInfo fi = new FileInfo(filename);
                fi.MoveTo(newFileName);
            }
        }

    }
}
