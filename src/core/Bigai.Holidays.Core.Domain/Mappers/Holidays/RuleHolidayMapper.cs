using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Mappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Mappers.Holidays
{
    internal static class RuleHolidayMapper
    {
        /// <summary>
        /// This method maps the content to a list of rule holidays lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <returns>Returns a list of rule holidays lists.</returns>
        internal static List<List<RuleHoliday>> ToListOfRuleHolidayList(this string[,] content)
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
                        rules = GetRulesHolidaysFromCsv(content, start, end);
                        list.Add(rules);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// This method maps the content to a list of rule holidays lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <returns>Returns a list of rule holidays lists.</returns>
        internal static async Task<List<List<RuleHoliday>>> ToListOfRuleHolidayListAsync(this string[,] content)
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
                        rules = await Task.Run(() => GetRulesHolidaysFromCsv(content, start, end));
                        list.Add(rules);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        private static List<RuleHoliday> GetRulesHolidaysFromCsv(string[,] rulesHolidaysCsv, int start, int end)
        {
            List<RuleHoliday> rules = null;

            if (rulesHolidaysCsv != null && rulesHolidaysCsv.GetLength(0) > 0)
            {
                rules = new List<RuleHoliday>();

                try
                {
                    Guid userId = Guid.Parse("3332c0c3-c506-4ec2-beea-e7dd5c942038");

                    for (int i = start, j = end; i < j; i++)
                    {
                        Guid countryId = Guid.Empty;
                        Guid stateId = Guid.Empty;
                        var id = rulesHolidaysCsv[i, 0].HasValue() ? int.Parse((rulesHolidaysCsv[i, 0]).Trim()) : -1;
                        var countryCode = rulesHolidaysCsv[i, 1];
                        var stateName = rulesHolidaysCsv[i, 2];
                        var stateCode = rulesHolidaysCsv[i, 3];
                        var month = rulesHolidaysCsv[i, 4].ToMonth();
                        var day = rulesHolidaysCsv[i, 5].ToDay();
                        var nativeDescription = rulesHolidaysCsv[i, 6];
                        var alternativeDescription = rulesHolidaysCsv[i, 7];
                        var holidayType = rulesHolidaysCsv[i, 8].Replace("  ", " ").Trim();
                        var cityName = rulesHolidaysCsv[i, 9].Trim();
                        var cityCode = rulesHolidaysCsv[i, 10].Trim();
                        var optional = rulesHolidaysCsv[i, 11].HasValue();
                        var bussinessRule = rulesHolidaysCsv[i, 12].ToLower().Trim().Replace(" ", "");
                        var comments = rulesHolidaysCsv[i, 13].Trim();

                        HolidayType type = HolidayType.National;
                        if (holidayType == "Not A Public Holiday")
                        {
                            type = HolidayType.Observance;
                            comments = holidayType + " " + comments;
                        }
                        else if (cityName.HasValue())
                        {
                            type = HolidayType.Local;
                        }
                        else if (!cityName.HasValue() && stateCode.HasValue())
                        {
                            type = HolidayType.Regional;
                        }

                        var rule = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Active, TypeProcess.Register, userId, countryId, stateId, countryCode, stateCode, cityCode, cityName, type, nativeDescription, alternativeDescription, month, day, optional, bussinessRule, comments);
                        rules.Add(rule);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return rules;
        }
    }
}
