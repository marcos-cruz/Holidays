using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Helpers
{
    /// <summary>
    /// <see cref="CsvHelper"/> Contains methods to support reading files in csv format, delimited by a semicolon.
    /// </summary>
    public class CsvHelper
    {
        /// <summary>
        /// This method reads a CSV file and returns its contents in an array of rows and columns.
        /// </summary>
        /// <param name="fileName">Path for the CSV file to read.</param>
        /// <returns>Returns a multidimensional array with the rows and columns that were read from the CSV file. If you are unable to read the file or if it is empty, then return null.</returns>
        public static string[,] LoadCsv(string fileName)
        {
            string[,] spreadsheetContent = null;

            try
            {
                string contentFile = ReadFile(fileName);

                if (contentFile.HasValue())
                {
                    string[] lines = ToArrayOfStrings(contentFile);

                    if (lines != null && lines.Length > 0)
                    {
                        spreadsheetContent = CreateArray(lines);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return spreadsheetContent;
        }

        /// <summary>
        /// This method reads a CSV file and returns its contents in an array of rows and columns.
        /// </summary>
        /// <param name="fileName">Path for the CSV file to read.</param>
        /// <returns>Returns a multidimensional array with the rows and columns that were read from the CSV file. If you are unable to read the file or if it is empty, then return null.</returns>
        public static async Task<string[,]> LoadCsvAsync(string fileName)
        {
            string[,] spreadsheetContent = null;

            try
            {
                string contentFile = await ReadFileAsync(fileName);

                if (contentFile.HasValue())
                {
                    string[] lines = ToArrayOfStrings(contentFile);

                    if (lines != null && lines.Length > 0)
                    {
                        spreadsheetContent = CreateArray(lines);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return spreadsheetContent;
        }

        /// <summary>
        /// This method reads the entire contents of the file at once.
        /// </summary>
        /// <param name="pathFile">Name of file to read.</param>
        /// <returns>Returns a string with all the contents of the file. In case of error, return <c>null</c>.</returns>
        private static string ReadFile(string pathFile)
        {
            string contentFile = null;

            try
            {
                if (File.Exists(pathFile))
                {
                    var fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);

                    using var streamReader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1"));

                    contentFile = streamReader.ReadToEnd();
                }
                else
                {
                    throw new Exception(pathFile + " NOT FOUND!!!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contentFile;
        }

        /// <summary>
        /// This method reads the entire contents of the file at once.
        /// </summary>
        /// <param name="pathFile">Name of file to read.</param>
        /// <returns>Returns a string with all the contents of the file. In case of error, return <c>null</c>.</returns>
        private static async Task<string> ReadFileAsync(string pathFile)
        {
            string contentFile = null;

            try
            {
                if (File.Exists(pathFile))
                {
                    var fileStream = new FileStream(pathFile, FileMode.Open, FileAccess.Read);

                    using var streamReader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1"));

                    contentFile = await streamReader.ReadToEndAsync();
                }
                else
                {
                    throw new Exception(pathFile + " NOT FOUND!!!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return contentFile;
        }

        /// <summary>
        /// This method returns an array of lines broken by the '\r' character.
        /// </summary>
        /// <param name="content">String with content to be broken into lines</param>
        /// <returns>Returns the content in the form of an array of lines, otherwise <c>null</c>.</returns>
        private static string[] ToArrayOfStrings(string content)
        {
            if (!content.HasValue())
            {
                return null;
            }

            content = content.Replace('\n', '\r');

            return content.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// This method create array lines and columns.
        /// </summary>
        /// <param name="lines">array of lines.</param>
        /// <returns>returns an array representing rows and columns.</returns>
        /// <remarks>To obtain the number of columns it is considered that all rows have the same number of columns as the first row after the header.</remarks>
        private static string[,] CreateArray(string[] lines)
        {
            string[,] spreadsheet = null;

            if (lines != null && lines.Length > 0)
            {
                int linesInHeader = SkipHeader(lines);
                int numberOfLines = lines.Length - linesInHeader;
                int numberOfColumns = lines[linesInHeader].Split(';').Length;

                spreadsheet = new string[numberOfLines, numberOfColumns];

                for (int line = 0; line < numberOfLines; line++)
                {
                    string[] columns = lines[line + linesInHeader].Split(';');

                    for (int column = 0; column < numberOfColumns; column++)
                    {
                        spreadsheet[line, column] = columns[column].Replace("  ", " ").Trim();
                    }
                }
            }

            return spreadsheet;
        }

        /// <summary>
        /// Determines how many lines there are in the header.
        /// </summary>
        /// <param name="lines">array of lines of spreadsheet.</param>
        /// <returns>Returns the number of lines in the header.</returns>
        private static int SkipHeader(string[] lines)
        {
            int linesInHeader = 0;

            if (lines == null || lines.Length == 0)
            {
                return linesInHeader;
            }

            for (int i = 0, j = lines.Length; i < j; i++)
            {
                bool isNumeric = lines[i].HasValue() && char.IsNumber(lines[i][0]);

                if (isNumeric)
                {
                    linesInHeader = i;
                    i = j;
                }
            }

            return linesInHeader;
        }
    }
}
