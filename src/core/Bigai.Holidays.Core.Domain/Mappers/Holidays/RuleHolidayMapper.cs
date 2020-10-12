using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using Bigai.Holidays.Shared.Infra.CrossCutting.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Mappers.Holidays
{
    internal static class RuleHolidayMapper
    {
        #region ToDomain

        /// <summary>
        /// This method maps the content to a list of rule holidays lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <param name="countryRepository">For country search.</param>
        /// <param name="stateRepository">For state search.</param>
        /// <param name="userLogged">The who is logged in.</param>
        /// <returns>Returns a list of rule holidays lists.</returns>
        internal static async Task<List<List<RuleHoliday>>> ToListOfRuleHolidayListAsync(this string[,] content, ICountryRepository countryRepository, IStateRepository stateRepository, IUserLogged userLogged)
        {
            List<List<RuleHoliday>> list = new List<List<RuleHoliday>>();

            if (content != null)
            {
                List<RuleHoliday> rules;
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
                        rules = await Task.Run(() => GetRulesHolidaysFromCsvAsync(content, start, end, countryRepository, stateRepository, userLogged));
                        list.Add(rules);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        #endregion

        #region Private Methods

        private static async Task<List<RuleHoliday>> GetRulesHolidaysFromCsvAsync(string[,] rulesHolidaysCsv, int start, int end, ICountryRepository countryRepository, IStateRepository stateRepository, IUserLogged userLogged)
        {
            List<RuleHoliday> rules = null;

            if (rulesHolidaysCsv != null && rulesHolidaysCsv.GetLength(0) > 0)
            {
                rules = new List<RuleHoliday>();

                try
                {
                    Guid userId = userLogged.GetUserId();
                    Guid countryId = Guid.Empty;
                    Guid? stateId = null;
                    Country country = null;
                    string stateCodePrevious = "?";

                    for (int i = start, j = end; i < j; i++)
                    {
                        var id = rulesHolidaysCsv[i, 0].HasValue() ? int.Parse((rulesHolidaysCsv[i, 0]).Trim()) : -1;
                        var countryCode = rulesHolidaysCsv[i, 1].Trim();
                        var stateName = rulesHolidaysCsv[i, 2].Trim();
                        var stateCode = rulesHolidaysCsv[i, 3].Trim();
                        var month = rulesHolidaysCsv[i, 4].ToMonth();
                        var day = rulesHolidaysCsv[i, 5].ToDay();
                        var nativeDescription = rulesHolidaysCsv[i, 6].Trim();
                        var alternativeDescription = rulesHolidaysCsv[i, 7].Trim();
                        var holidayType = rulesHolidaysCsv[i, 8].Replace("  ", " ").Trim();
                        var cityName = rulesHolidaysCsv[i, 9].Trim();
                        var cityCode = rulesHolidaysCsv[i, 10].Trim();
                        var optional = rulesHolidaysCsv[i, 11].HasValue();
                        var bussinessRule = rulesHolidaysCsv[i, 12].ToLower().Trim().Replace(" ", "");
                        var comments = rulesHolidaysCsv[i, 13].Trim();

                        if (country == null || country.CountryIsoCode3 != countryCode)
                        {
                            country = (await countryRepository.FindAsync(c => c.CountryIsoCode3 == countryCode)).FirstOrDefault();
                            countryId = country != null ? country.Id : Guid.Empty;
                        }

                        if (stateCode.HasValue() && stateCode != stateCodePrevious)
                        {
                            stateCodePrevious = stateCode;
                            stateId = await SetStateIdAsync(stateRepository, stateCode, countryCode);
                        }

                        HolidayType type = SetHolidayType(holidayType, cityName, stateCode);

                        comments = SetComments(holidayType, comments);

                        var ruleHoliday = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Active, ActionType.Register, userId, countryId, stateId, countryCode, stateCode, cityCode, cityName, type, nativeDescription, alternativeDescription, month, day, optional, bussinessRule, comments);

                        rules.Add(ruleHoliday);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rules;
        }

        private static async Task<Guid?> SetStateIdAsync(IStateRepository stateRepository, string stateCode, string countryCode)
        {
            Guid? stateId = null;

            if (stateCode.HasValue())
            {
                State state = (await stateRepository.FindAsync(s => s.CountryIsoCode == countryCode && s.StateIsoCode == stateCode)).FirstOrDefault();

                if (state != null)
                {
                    stateId = state.Id;
                }
            }

            return stateId;
        }

        private static HolidayType SetHolidayType(string holidayType, string cityName, string stateCode)
        {
            HolidayType type = HolidayType.National;
            if (holidayType == "Not A Public Holiday")
            {
                type = HolidayType.Observance;
            }
            else if (cityName.HasValue())
            {
                type = HolidayType.Local;
            }
            else if (!cityName.HasValue() && stateCode.HasValue())
            {
                type = HolidayType.Regional;
            }

            return type;
        }

        private static string SetComments(string holidayType, string comments)
        {
            if (holidayType == "Not A Public Holiday")
            {
                comments = holidayType + "; " + comments;
                comments.Trim();
            }

            return comments;
        }

        #endregion
    }
}
