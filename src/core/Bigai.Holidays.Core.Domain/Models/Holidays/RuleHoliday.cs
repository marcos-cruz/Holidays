﻿using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using System;

namespace Bigai.Holidays.Core.Domain.Models.Holidays
{
    /// <summary>
    /// <see cref="RuleHoliday"/> represents the rules for creating holidays.
    /// </summary>
    public class RuleHoliday : Entity
    {
        #region Properties

        /// <summary>
        /// Id that identifies which country this rule belongs to.
        /// </summary>
        public Guid CountryId { get; private set; }

        /// <summary>
        /// Id that identifies which state this rule belongs to.
        /// </summary>
        public Guid? StateId { get; private set; }

        /// <summary>
        /// Country code consisting of 3 letters, according to ISO-3166.
        /// </summary>
        public string CountryIsoCode { get; private set; }

        /// <summary>
        /// State code, according to ISO-3166.
        /// </summary>
        public string StateIsoCode { get; private set; }

        /// <summary>
        /// Code to identify the city.
        /// </summary>
        public string CityId { get; private set; }

        /// <summary>
        /// Official Code of the city.
        /// </summary>
        public string CityCode { get; private set; }

        /// <summary>
        /// City name.
        /// </summary>
        public string CityName { get; private set; }

        /// <summary>
        /// Holiday range type.
        /// </summary>
        public HolidayType HolidayType { get; private set; }

        /// <summary>
        /// Official holiday name.
        /// </summary>
        public string NativeHolidayName { get; private set; }

        /// <summary>
        /// English holiday name.
        /// </summary>
        public string EnglishHolidayName { get; private set; }

        /// <summary>
        /// Month in which the holiday occurs.
        /// </summary>
        public int? Month { get; private set; }

        /// <summary>
        /// Day in which the holiday occurs.
        /// </summary>
        public int? Day { get; private set; }

        /// <summary>
        /// Indicates whether the holiday is optional.
        /// </summary>
        public bool Optional { get; private set; }

        /// <summary>
        /// Rule for creating the holiday.
        /// </summary>
        public string BussinessRule { get; private set; }

        /// <summary>
        /// Any comment about the holiday.
        /// </summary>
        public string Comments { get; private set; }

        /// <summary>
        /// This key is generated by the system to avoid duplication
        /// </summary>
        public string ComposeKey { get; private set; }

        #endregion

        #region Navigation Properites

        public virtual Country Country { get; private set; }

        #endregion

        #region Constructor

        protected RuleHoliday() { }

        private RuleHoliday(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, Guid countryId, Guid? stateId, string countryIsoCode, string stateIsoCode, string cityCode, string cityName, HolidayType holidayType, string nativeHolidayName, string englishHolidayName, int? month, int? day, bool optional, string bussinessRule, string comments) : base(id, status, action, userId)
        {
            CountryId = countryId;
            StateId = stateId;
            CountryIsoCode = countryIsoCode.HasValue() ? countryIsoCode.Trim().ToUpper() : countryIsoCode;
            StateIsoCode = stateIsoCode.HasValue() ? stateIsoCode.Trim().ToUpper() : stateIsoCode;
            CityId = cityName.HasValue() ? cityName.ToMD5HashString() : cityName;
            CityCode = cityCode.HasValue() ? cityCode.Trim() : cityCode;
            CityName = cityName.HasValue() ? cityName.Replace("  ", " ").Trim() : cityName;
            HolidayType = holidayType;
            NativeHolidayName = nativeHolidayName.HasValue() ? nativeHolidayName.Replace("  ", " ").Trim() : nativeHolidayName;
            EnglishHolidayName = englishHolidayName.HasValue() ? englishHolidayName.Replace("  ", " ").Trim() : englishHolidayName;
            Month = month;
            Day = day;
            Optional = optional;
            BussinessRule = bussinessRule.HasValue() ? bussinessRule.Replace(" ", "").Trim().ToLower() : bussinessRule;
            Comments = comments.HasValue() ? comments.Replace("  ", " ").Trim() : comments;

            var baseKey = CountryIsoCode + StateIsoCode + CityId + HolidayType.Key.ToString() + NativeHolidayName + BussinessRule;
            if (Month.HasValue)
            {
                baseKey += Month.Value.ToString();
            }
            if (Day.HasValue)
            {
                baseKey += Day.Value.ToString();
            }

            ComposeKey = baseKey.ToMD5HashString();
        }

        /// <summary>
        /// Return a instance of <see cref="RuleHoliday"/>.
        /// </summary>
        /// <param name="id">Record identifier. Optional if action equal <c>Register</c>. Required for other actions.</param>
        /// <param name="status">Current status of the entity. Required.</param>
        /// <param name="action">Action to take with entity. Required.</param>
        /// <param name="userId">Who is taking this action. Optional.</param>
        /// <param name="countryId">Identifies which country this rule belongs to. Required.</param>
        /// <param name="stateId">Identifies which state this rule belongs to. Required if stateIsoCode was filled.</param>
        /// <param name="countryIsoCode">Country code consisting of 3 letters, according to ISO-3166. Required.</param>
        /// <param name="stateIsoCode">State code, according to ISO-3166. Required if rule is for state.</param>
        /// <param name="cityCode">Official Code of the city. Optional.</param>
        /// <param name="cityName">City name. Required if rule is for local holiday.</param>
        /// <param name="holidayType">Holiday range type. Required.</param>
        /// <param name="nativeHolidayName">Official holiday name. Required.</param>
        /// <param name="englishHolidayName">English holiday name. Optional.</param>
        /// <param name="month">Month in which the holiday occurs. Required id bussiness rule is empty.</param>
        /// <param name="day">Day in which the holiday occurs. Required id bussiness rule is empty.</param>
        /// <param name="optional">Indicates whether the holiday is optional.</param>
        /// <param name="bussinessRule">Rule for creating the holiday. Optional.</param>
        /// <param name="comments">Any comment about the holiday. Optional.</param>
        /// <returns>Instance of <see cref="RuleHoliday"/>.</returns>
        public static RuleHoliday CreateRuleHoliday(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, Guid countryId, Guid? stateId, string countryIsoCode, string stateIsoCode, string cityCode, string cityName, HolidayType holidayType, string nativeHolidayName, string englishHolidayName, int? month, int? day, bool optional, string bussinessRule, string comments)
        {
            if (month.HasValue && month.Value < 1)
            {
                month = null;
            }

            if (day.HasValue && day.Value < 1)
            {
                day = null;
            }

            return new RuleHoliday(id, status, action, userId, countryId, stateId, countryIsoCode, stateIsoCode, cityCode, cityName, holidayType, nativeHolidayName, englishHolidayName, month, day, optional, bussinessRule, comments);
        }

        #endregion

        #region Override

        /// <summary>
        /// Determines whether the specified object instances are considered equal.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        /// <returns><c>true</c> if the objects are considered equal, otherwise, <c>false</c>. If both objects are <c>null</c>,
        /// returns <c>true</c>.</returns>
        protected override bool EqualsCore(object other)
        {
            var rule = other as RuleHoliday;

            return CountryId == rule.CountryId &&
                   StateId == rule.StateId &&
                   CountryIsoCode == rule.CountryIsoCode &&
                   StateIsoCode == rule.StateIsoCode &&
                   CityId == rule.CityId &&
                   CityCode == rule.CityCode &&
                   CityName == rule.CityName &&
                   HolidayType == rule.HolidayType &&
                   NativeHolidayName == rule.NativeHolidayName &&
                   EnglishHolidayName == rule.EnglishHolidayName &&
                   Month == rule.Month &&
                   Day == rule.Day &&
                   Optional == rule.Optional &&
                   BussinessRule == rule.BussinessRule &&
                   Comments == rule.Comments;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns the country id to the current rule holiday.
        /// </summary>
        /// <param name="countryId">Id that identifies the Country.</param>
        public void CreateCountryRelationship(Guid countryId)
        {
            CountryId = countryId;
        }

        /// <summary>
        /// Assigns the state id to the current rule holiday.
        /// </summary>
        /// <param name="stateId">Id that identifies the State.</param>
        public void CreateStateRelationship(Guid stateId)
        {
            StateId = stateId;
        }

        #endregion
    }
}
