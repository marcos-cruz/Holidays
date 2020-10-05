using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bigai.Holidays.Core.Domain.Models.Countries
{
    /// <summary>
    /// <see cref="Country"/> represents basic information about country.
    /// </summary>
    public class Country : Entity
    {
        #region Private Variables

        private IList<State> _states;
        private IList<RuleHoliday> _rulesHolidays;
        private IList<Holiday> _holidays;

        #endregion

        #region Properties

        /// <summary>
        /// Numeric code, according to ISO-3166.
        /// </summary>
        public string NumericCode { get; private set; }

        /// <summary>
        /// Country code consisting of 2 letters, according to ISO-3166.
        /// </summary>
        public string CountryIsoCode2 { get; private set; }

        /// <summary>
        /// Country code consisting of 3 letters, according to ISO-3166.
        /// </summary>
        public string CountryIsoCode3 { get; private set; }

        /// <summary>
        /// Official country name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Official short country name.
        /// </summary>
        public string ShortName { get; private set; }

        /// <summary>
        /// Language code.
        /// </summary>
        public string LanguageCode { get; private set; }

        /// <summary>
        /// Region name.
        /// </summary>
        public string RegionName { get; private set; }

        /// <summary>
        /// Subregion name.
        /// </summary>
        public string SubRegionName { get; private set; }

        /// <summary>
        /// Intermediate region name.
        /// </summary>
        public string IntermediateRegionName { get; private set; }

        /// <summary>
        /// Region code.
        /// </summary>
        public int RegionCode { get; private set; }

        /// <summary>
        /// Subregion code.
        /// </summary>
        public int SubRegionCode { get; private set; }

        /// <summary>
        /// Intermediate region code.
        /// </summary>
        public int IntermediateRegionCode { get; private set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        /// List with information on the country's states.
        /// </summary>
        public virtual IReadOnlyCollection<State> States => _states.ToArray();

        /// <summary>
        /// List of rules for the dynamic creation of holidays.
        /// </summary>
        public virtual IReadOnlyCollection<RuleHoliday> RulesHolidays => _rulesHolidays.ToArray();

        /// <summary>
        /// List of rules for the dynamic creation of holidays.
        /// </summary>
        public virtual IReadOnlyCollection<Holiday> Holidays => _holidays.ToArray();

        #endregion

        #region Constructor

        protected Country()
        {
            CreateListOfAggregated();
        }

        private Country(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, string numericCode, string countryIsoCode2, string countryIsoCode3, string name, string shortName, string languageCode, string regionName, string subRegionName, string intermediateRegionName, int regionCode, int subRegionCode, int intermediateRegionCode) : base(id, status, action, userId)
        {
            NumericCode = numericCode.HasValue() ? numericCode.Trim().ToUpper() : numericCode;
            CountryIsoCode2 = countryIsoCode2.HasValue() ? countryIsoCode2.Trim().ToUpper() : countryIsoCode2;
            CountryIsoCode3 = countryIsoCode3.HasValue() ? countryIsoCode3.Trim().ToUpper() : countryIsoCode3;
            Name = name.HasValue() ? name.Trim().Replace("  ", " ") : name;
            ShortName = shortName.HasValue() ? shortName.Trim().Replace("  ", " ") : shortName;
            LanguageCode = languageCode.HasValue() ? languageCode.Trim().ToLower() : languageCode;
            RegionName = regionName.HasValue() ? regionName.Trim().Replace("  ", " ") : regionName;
            SubRegionName = subRegionName.HasValue() ? subRegionName.Trim().Replace("  ", " ") : subRegionName;
            IntermediateRegionName = intermediateRegionName.HasValue() ? intermediateRegionName.Trim().Replace("  ", " ") : intermediateRegionName;
            RegionCode = regionCode;
            SubRegionCode = subRegionCode;
            IntermediateRegionCode = intermediateRegionCode;

            CreateListOfAggregated();
        }

        /// <summary>
        /// Return a instance of <see cref="Country"/>.
        /// </summary>
        /// <param name="id">Record identifier. Optional if action equal <c>Register</c>. Required for other actions.</param>
        /// <param name="status">Current status of the entity. Required.</param>
        /// <param name="action">Action to take with entity. Required.</param>
        /// <param name="userId">Who is taking this action. Optional.</param>
        /// <param name="numericCode">Numeric code, according to ISO-3166. Optional.</param>
        /// <param name="countryIsoCode2">Country code consisting of 2 letters, according to ISO-3166. Required.</param>
        /// <param name="countryIsoCode3">Country code consisting of 3 letters, according to ISO-3166. Required.</param>
        /// <param name="name">Official country name. Required.</param>
        /// <param name="shortName">Official short country name. Required.</param>
        /// <param name="languageCode">Language code. Required.</param>
        /// <param name="regionName">Region name. Required.</param>
        /// <param name="subRegionName">Sub region name. Required.</param>
        /// <param name="intermediateRegionName">Intermediate region name. Optional.</param>
        /// <param name="regionCode">Region code. Required.</param>
        /// <param name="subRegionCode">Sub region code. Required.</param>
        /// <param name="intermediateRegionCode">Intermediate region code. Optional.</param>
        /// <returns>Instance of <see cref="Country"/>.</returns>
        public static Country CreateCountry(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, string numericCode, string countryIsoCode2, string countryIsoCode3, string name, string shortName, string languageCode, string regionName, string subRegionName, string intermediateRegionName, int regionCode, int subRegionCode, int intermediateRegionCode)
        {
            return new Country(id, status, action, userId, numericCode, countryIsoCode2, countryIsoCode3, name, shortName, languageCode, regionName, subRegionName, intermediateRegionName, regionCode, subRegionCode, intermediateRegionCode);
        }

        /// <summary>
        /// Return a instance of <see cref="Country"/>.
        /// </summary>
        /// <param name="id">Record identifier. Optional if action equal <c>Register</c>. Required for other actions.</param>
        /// <param name="status">Current status of the entity.</param>
        /// <param name="action">Action to take with entity.</param>
        /// <param name="userId">Who is taking this action.</param>
        /// <param name="numericCode">Numeric code, according to ISO-3166. Optional.</param>
        /// <param name="countryIsoCode2">Country code consisting of 2 letters, according to ISO-3166. Required.</param>
        /// <param name="countryIsoCode3">Country code consisting of 3 letters, according to ISO-3166. Required.</param>
        /// <param name="name">Official country name. Required.</param>
        /// <param name="shortName">Official short country name. Required.</param>
        /// <param name="languageCode">Language code. Required.</param>
        /// <param name="regionName">Region name. Required.</param>
        /// <param name="subRegionName">Sub region name. Required.</param>
        /// <param name="intermediateRegionName">Intermediate region name. Optional.</param>
        /// <param name="regionCode">Region code. Required.</param>
        /// <param name="subRegionCode">Sub region code. Required.</param>
        /// <param name="intermediateRegionCode">Intermediate region code. Optional.</param>
        /// <returns>Instance of <see cref="Country"/>.</returns>
        public static Country CreateCountry(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, string numericCode, string countryIsoCode2, string countryIsoCode3, string name, string shortName, string languageCode, string regionName, string subRegionName, string intermediateRegionName, int regionCode, int subRegionCode, int intermediateRegionCode, IList<State> states)
        {
            var country = new Country(id, status, action, userId, numericCode, countryIsoCode2, countryIsoCode3, name, shortName, languageCode, regionName, subRegionName, intermediateRegionName, regionCode, subRegionCode, intermediateRegionCode);

            country.AssignStates(states);

            return country;
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
            var country = other as Country;

            return NumericCode == country.NumericCode &&
                   CountryIsoCode2 == country.CountryIsoCode2 &&
                   CountryIsoCode3 == country.CountryIsoCode3 &&
                   Name == country.Name &&
                   ShortName == country.ShortName &&
                   LanguageCode == country.LanguageCode &&
                   RegionName == country.RegionName &&
                   SubRegionName == country.SubRegionName &&
                   IntermediateRegionName == country.IntermediateRegionName &&
                   RegionCode == country.RegionCode &&
                   SubRegionCode == country.SubRegionCode &&
                   IntermediateRegionCode == country.IntermediateRegionCode;
        }

        #endregion

        #region Private Methods

        private void CreateListOfAggregated()
        {
            _states = new List<State>();
            _rulesHolidays = new List<RuleHoliday>();
            _holidays = new List<Holiday>();
        }

        private void AssignStates(IList<State> states)
        {
            if (states != null)
            {
                foreach (State state in states)
                {
                    state.CreateRelationship(Id);

                    state.SynchronizeRegistrationDate(this);

                    _states.Add(state);
                }
            }
        }

        #endregion
    }
}
