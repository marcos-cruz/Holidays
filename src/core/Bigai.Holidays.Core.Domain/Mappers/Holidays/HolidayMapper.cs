using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Responses;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using Bigai.Holidays.Shared.Infra.CrossCutting.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bigai.Holidays.Core.Domain.Mappers.Holidays
{
    internal static class HolidayMapper
    {
        #region ToResponse

        /// <summary>
        /// This method mapping a list of <see cref="Holiday"/> to <see cref="HolidayResponse"/>.
        /// </summary>
        /// <param name="holidays">List to mapping.</param>
        /// <returns>List of <see cref="HolidayResponse"/>. In case of error <c>null</c>.</returns>
        internal static IEnumerable<HolidayResponse> ToResponse(this IEnumerable<Holiday> holidays)
        {
            if (holidays == null || holidays.Count() == 0)
            {
                return null;
            }

            List<HolidayResponse> list = new List<HolidayResponse>();

            foreach (var holiday in holidays)
            {
                list.Add(holiday.ToResponse());
            }

            return list;
        }

        /// <summary>
        /// This method mapping a instance of <see cref="HolidayEvent"/> to <see cref="HolidayResponse"/>.
        /// </summary>
        /// <param name="holiday">Instance to mapping.</param>
        /// <returns><see cref="HolidayResponse"/>. In case of error <c>null</c>.</returns>
        internal static HolidayResponse ToResponse(this Holiday holiday)
        {
            if (holiday == null)
            {
                return null;
            }

            string optional = holiday.Optional ? " | Optional" : "";
            return new HolidayResponse()
            {
                HolidayDate = holiday.HolidayDate,
                HolidayDay = holiday.HolidayDate.DayOfWeek.ToString(),
                Type = holiday.HolidayType.Name + optional,
                NativeDescription = holiday.NativeDescription,
                AlternativeDescription = holiday.AlternativeDescription,
                Country = holiday.CountryCode,
                State = holiday.StateCode,
                CityCode = holiday.CityCode
            };
        }

        #endregion

        #region ToDomain

        internal static List<List<Holiday>> ToHolidayList(this List<RuleHoliday> rulesHolidays, INotificationHandler notificationHandler, int year, IUserLogged userLogged)
        {
            List<List<Holiday>> list = new List<List<Holiday>>();

            if (rulesHolidays != null && rulesHolidays.Count() > 0)
            {
                List<Holiday> holidays;
                int items = rulesHolidays.Count();
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
                    holidays = GetHolidaysFromRules(notificationHandler, rulesHolidays, start, end, year, userLogged);
                    list.Add(holidays);

                    start = end;
                    end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                }
            }

            return list;
        }

        #endregion

        #region Private Methods

        private static List<Holiday> GetHolidaysFromRules(INotificationHandler notificationHandler, List<RuleHoliday> rulesHolidays, int start, int end, int year, IUserLogged userLogged)
        {
            List<Holiday> holidays = null;

            if (rulesHolidays != null && rulesHolidays.Count() > 0)
            {
                holidays = new List<Holiday>();

                try
                {
                    Guid userId = userLogged.GetUserId();
                    RuleHoliday ruleHoliday = null;
                    DateTime? holidayDate = null;

                    for (int i = start, j = end; i < j; i++)
                    {
                        ruleHoliday = rulesHolidays[i];
                        holidayDate = null;

                        if (ruleHoliday.BussinessRule.HasValue())
                        {
                            if (!ApplyBusinessRuleForDate(notificationHandler, ruleHoliday, year, out holidayDate))
                            {
                                ApplyBusinessRuleForTime(notificationHandler, ruleHoliday, year, out holidayDate);
                            }
                        }
                        else
                        {
                            if (ruleHoliday.Month.HasValue && ruleHoliday.Day.HasValue)
                            {
                                holidayDate = new DateTime(year, ruleHoliday.Month.Value, ruleHoliday.Day.Value);
                            }
                        }

                        if (holidayDate == null)
                        {
                            var notification = new DomainNotification(ruleHoliday.Id.ToString(), $"O feriado {ruleHoliday.NativeHolidayName} do {ruleHoliday.CountryIsoCode} não tem data.");
                            notificationHandler.NotifyError(notification);
                        }
                        else
                        {
                            var holiday = Holiday.CreateHoliday(null, EntityStatus.Active, ActionType.Register, userId, ruleHoliday.CountryId, ruleHoliday.StateId, holidayDate.Value, ruleHoliday.HolidayType, ruleHoliday.Optional, ruleHoliday.NativeHolidayName, ruleHoliday.EnglishHolidayName, ruleHoliday.CountryIsoCode, ruleHoliday.StateIsoCode, ruleHoliday.CityName, ruleHoliday.CityCode);
                            holidays.Add(holiday);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return holidays;
        }

        private static bool ApplyBusinessRuleForDate(INotificationHandler notificationHandler, RuleHoliday ruleHoliday, int year, out DateTime? holidayDate)
        {
            bool businessRuleApplied = false;
            DateTime date = DateTime.MinValue;

            if (ruleHoliday.BussinessRule == "ascensionday")
            {
                date = year.GetAscensionDay();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "ashwednesday")
            {
                date = year.GetAshWednesday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "assumptionday")
            {
                date = year.GetAshWednesday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "carnivalmonday")
            {
                date = year.GetCarnivalMonday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "carnivaltuesday")
            {
                date = year.GetCarnivalTuesday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "corpuschristi")
            {
                date = year.GetCorpusChristi();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "easter" || ruleHoliday.BussinessRule == "eastersunday")
            {
                date = year.GetEasterSunday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "eastermonday")
            {
                date = year.GetEasterSunday();
                date = date.AddDays(1);
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "eastertuesday")
            {
                date = year.GetEasterSunday();
                date = date.AddDays(2);
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "goodfriday")
            {
                date = year.GetGoodFriday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "holysaturday")
            {
                date = year.GetHolySaturday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "holythursday")
            {
                date = year.GetHolyThursday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "maundythursday")
            {
                date = year.GetMaundyThursday();
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "pentecostmonday")
            {
                date = year.GetPentecostDay();
                date = date.AddDays(1);
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "pentecosttuesday")
            {
                date = year.GetPentecostDay();
                date = date.AddDays(2);
                businessRuleApplied = true;
            }
            else if (ruleHoliday.BussinessRule == "whitmonday")
            {
                date = year.GetWhitsunDay();
                date = date.AddDays(1);
                businessRuleApplied = true;
            }

            holidayDate = null;

            if (businessRuleApplied)
            {
                holidayDate = new DateTime(year, date.Month, date.Day);
            }

            if (!ruleHoliday.BussinessRule.Contains(",") && !businessRuleApplied)
            {
                var notification = new DomainNotification(ruleHoliday.Id.ToString(), $"A regra {ruleHoliday.BussinessRule} do {ruleHoliday.CountryIsoCode} não foi implementada.");
                notificationHandler.NotifyError(notification);
            }

            return businessRuleApplied;
        }

        private static void ApplyBusinessRuleForTime(INotificationHandler notificationHandler, RuleHoliday ruleHoliday, int year, out DateTime? holidayDate)
        {
            string[] bussinesRules = ruleHoliday.BussinessRule.Split(",");

            holidayDate = null;

            if (bussinesRules != null && bussinesRules.Length >= 3)
            {
                if (bussinesRules[2] == "nearest")
                {
                    bussinesRules.ApplyNearestBusinessRule(year, out holidayDate);
                }
                else if (bussinesRules[2] == "workday")
                {
                    bussinesRules.ApplyWorkdayBusinessRule(year, out holidayDate);
                }
                else if (bussinesRules[2] == "!sunday")
                {
                    bussinesRules.ApplyNotSundayBusinessRule(year, out holidayDate);
                }
                else if (bussinesRules[2] == "after")
                {
                    int times = bussinesRules[0].ToWeekTimes();
                    int dayOfWeek = bussinesRules[1].ToDayOfWeek();
                    DateTime date;

                    if (bussinesRules[3] == "easter")
                    {
                        date = year.GetEasterSunday();

                        for (int i = 0; i < times; i++)
                        {
                            date = date.GetDayAfterDate((DayOfWeek)dayOfWeek);
                        }

                        holidayDate = date;
                    }
                    else
                    {
                        var notification = new DomainNotification(ruleHoliday.Id.ToString(), $"A regra {ruleHoliday.BussinessRule} do {ruleHoliday.CountryIsoCode} não foi implementada.");
                        notificationHandler.NotifyError(notification);
                    }
                }
                else
                {
                    int weekNumber = bussinesRules[0].ToWeekNumber();
                    int dayOfWeek = bussinesRules[1].ToDayOfWeek();
                    int month = bussinesRules[2].ToMonth();

                    if (weekNumber != -1 && dayOfWeek != -1 && month != -1)
                    {
                        DateTime date = year.GetWeekDayOfMonth(month, (DayOfWeek)dayOfWeek, weekNumber);
                        holidayDate = date;
                    }
                }
            }
        }

        /// <summary>
        /// This method determines the working day when the holiday falls on a weekend.
        /// </summary>
        /// <param name="bussinesRules">Business rule to be applied to obtain the correct day of the holiday.</param>
        /// <param name="year">The year in which the holiday occurs.</param>
        /// <param name="holidayDate">Date of holiday generated by method.</param>
        private static void ApplyNearestBusinessRule(this string[] bussinesRules, int year, out DateTime? holidayDate)
        {
            int day = bussinesRules[0].ToDay();
            int month = bussinesRules[1].ToMonth();

            holidayDate = null;

            if (day != -1 && month != -1)
            {
                DateTime date = new DateTime(year, month, day);
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    date = date.AddDays(-1);
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                }
                holidayDate = date;
            }
        }

        /// <summary>
        /// this method determines the first work day from Monday to Friday, after a date.
        /// </summary>
        /// <param name="bussinesRules">Business rule to be applied to obtain the correct day of the holiday.</param>
        /// <param name="year">The year in which the holiday occurs.</param>
        /// <param name="holidayDate">Date of holiday generated by method.</param>
        private static void ApplyWorkdayBusinessRule(this string[] bussinesRules, int year, out DateTime? holidayDate)
        {
            int day = bussinesRules[0].ToDay();
            int month = bussinesRules[1].ToMonth();

            holidayDate = null;

            if (day != -1 && month != -1)
            {
                DateTime date = new DateTime(year, month, day);
                date = date.GetFirstWorkDayAfterDate();
                holidayDate = date;
            }
        }

        /// <summary>
        /// This method determines the working day when the holiday falls on a sunday.
        /// </summary>
        /// <param name="bussinesRules">Business rule to be applied to obtain the correct day of the holiday.</param>
        /// <param name="year">The year in which the holiday occurs.</param>
        /// <param name="holidayDate">Date of holiday generated by method.</param>
        /// <remarks>If he falls into the Sunday he will move to Monday.</remarks>
        private static void ApplyNotSundayBusinessRule(this string[] bussinesRules, int year, out DateTime? holidayDate)
        {
            int day = bussinesRules[0].ToDay();
            int month = bussinesRules[1].ToMonth();

            holidayDate = null;

            if (day != -1 && month != -1)
            {
                DateTime date = new DateTime(year, month, day);
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date = date.AddDays(1);
                }
                holidayDate = date;
            }
        }

        #endregion
    }
}
