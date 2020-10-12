using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Mappers.Countries
{
    internal static class CountryMapper
    {
        #region ToDomain

        /// <summary>
        /// This method maps the content to a list of country lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <param name="userLogged">The who is logged in.</param>
        /// <returns>Returns a list of country lists.</returns>
        internal static async Task<List<List<Country>>> ToListOfCountryListAsync(this string[,] content, IUserLogged userLogged)
        {
            List<List<Country>> list = new List<List<Country>>();

            if (content != null)
            {
                List<Country> countries;
                int items = content.GetLength(0);
                if (items > 0)
                {
                    const int itemsPerList = 64;
                    int tasks = 1;
                    int start;
                    int end = items;

                    if (items > itemsPerList)
                    {
                        tasks = (items / itemsPerList) + 1;
                        end = itemsPerList;
                    }

                    for (start = 0; tasks > 0; tasks--)
                    {
                        countries = await Task.Run(() => GetCountriesFromCsv(content, start, end, userLogged));
                        list.Add(countries);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        #endregion

        #region Private Methods

        private static List<Country> GetCountriesFromCsv(string[,] countriesCsv, int start, int end, IUserLogged userLogged)
        {
            List<Country> countries = null;

            if (countriesCsv != null && countriesCsv.GetLength(0) > 0)
            {
                countries = new List<Country>();

                try
                {
                    Guid userId = userLogged.GetUserId();

                    for (int i = start, j = end; i < j; i++)
                    {
                        string numericCode = countriesCsv[i, 1].Trim();
                        string alphaIsoCode2 = countriesCsv[i, 2].Trim();
                        string alphaIsoCode3 = countriesCsv[i, 3].Trim();
                        string name = countriesCsv[i, 4].Trim();
                        string shortName = countriesCsv[i, 5].Trim();
                        string languageCode = countriesCsv[i, 6].Trim();
                        string regionName = countriesCsv[i, 7].Trim();
                        string subRegionName = countriesCsv[i, 8].Trim();
                        string intermediateRegionName = countriesCsv[i, 9].Trim();
                        int regionCode = countriesCsv[i, 10].HasValue() ? int.Parse(countriesCsv[i, 10].Trim()) : 0;
                        int subRegionCode = countriesCsv[i, 11].HasValue() ? int.Parse(countriesCsv[i, 11].Trim()) : 0;
                        int intermediateRegionCode = countriesCsv[i, 12].HasValue() ? int.Parse(countriesCsv[i, 12].Trim()) : 0;
                        string pathCountryImage = null;

                        var country = Country.CreateCountry(null, EntityStatus.Active, ActionType.Register, userId, numericCode, alphaIsoCode2, alphaIsoCode3, name, shortName, languageCode, regionName, subRegionName, intermediateRegionName, regionCode, subRegionCode, intermediateRegionCode, pathCountryImage);
                        countries.Add(country);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return countries;
        }

        #endregion
    }
}
