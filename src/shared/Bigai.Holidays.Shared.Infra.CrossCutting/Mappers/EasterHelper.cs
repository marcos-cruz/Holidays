using System;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Mappers
{
    /// <summary>
    /// <see cref="EasterHelper"/> Contains methods for identifying the date on which Easter, Resurrection of 
    /// Our Lord Jesus Christ, occurs, using the Gauss algorithm.
    /// </summary>
    internal static class EasterHelper
    {
        private static int StartYear { get; set; }

        private static int FinalYear { get; set; }

        private static int X { get; set; }

        private static int Y { get; set; }

        private static readonly int[,] _factorGauss =
        {
             // StartYear, FinalYear, X, Y
             { 1582, 1599, 22, 2 },
             { 1600, 1699, 22, 2 },
             { 1700, 1799, 23, 3 },
             { 1800, 1899, 24, 4 },
             { 1900, 1999, 24, 5 },
             { 2000, 2099, 24, 5 },
             { 2100, 2199, 24, 6 },
             { 2200, 2299, 25, 7 }
         };

        /// <summary>
        /// Determines the date on which the celebration of Easter occurs.
        /// </summary>
        /// <param name="year">Year to be validated.</param>
        /// <returns>Returns the date on which Easter occurs for the requested year.</returns>
        /// <remarks>When the calculated Easter Sunday is 26/4, it is corrected for a week before, that is, 19 April (occurs in 2076).</remarks>
        /// <remarks>When the calculated Easter Sunday is 25/4 and 'd' = 28 and 'a'> 10, then Easter is on April 18 (occurs in 2049).</remarks>
        internal static DateTime Easter(this int year)
        {
            DateTime easterDate;

            SetFaixaByAno(year);

            if (StartYear != 0)
            {
                int a = year % 19;
                int b = year % 4;
                int c = year % 7;
                int d = (19 * a + X) % 30;
                int e = (6 * d + 4 * c + 2 * b + Y) % 7;
                easterDate = new DateTime(year, 3, 22).AddDays(d + e);
                int day = easterDate.Day;
                switch (day)
                {
                    case 26:
                        easterDate = new DateTime(year, 4, 19);
                        break;
                    case 25:
                        if (a > 10)
                            easterDate = new DateTime(year, 4, 18);
                        break;
                }
            }
            else
                easterDate = new DateTime(1, 1, 1);

            return easterDate.Date;
        }

        /// <summary>
        /// This function sets the range in which the requested year fits.
        /// </summary>
        /// <param name="year">Year for research in Factor Gauss.</param>
        internal static void SetFaixaByAno(int year)
        {
            FinalYear = X = Y = 0;

            for (int i = 0, j = _factorGauss.GetLength(0); i < j; i++)
            {
                if (year >= _factorGauss[i, 0] && year <= _factorGauss[i, 1])
                {
                    StartYear = _factorGauss[i, 0];
                    FinalYear = _factorGauss[i, 1];
                    X = _factorGauss[i, 2];
                    Y = _factorGauss[i, 3];
                    i = j;
                }
            }
        }
    }
}
