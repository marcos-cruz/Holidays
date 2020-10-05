using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using System;

namespace Bigai.Holidays.Core.Domain.Models.States
{
    /// <summary>
    /// <see cref="State"/> represents basic information about state.
    /// </summary>
    public class State : Entity
    {
        #region Properties

        /// <summary>
        /// Id that identifies who this state belongs to.
        /// </summary>
        public Guid CountryId { get; private set; }

        /// <summary>
        /// Country code consisting of 3 letters, according to ISO-3166.
        /// </summary>
        public string CountryIsoCode { get; private set; }

        /// <summary>
        /// State code, according to ISO-3166.
        /// </summary>
        public string StateIsoCode { get; private set; }

        /// <summary>
        /// Official name.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Navigation Properites

        public virtual Country Country { get; private set; }

        #endregion

        #region Constructor

        protected State() { }

        private State(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, Guid countryId, string countryIsoCode, string stateIsoCode, string name) : base(id, status, action, userId)
        {
            CountryId = countryId;
            CountryIsoCode = countryIsoCode.HasValue() ? countryIsoCode.Trim().ToUpper() : countryIsoCode;
            StateIsoCode = stateIsoCode.HasValue() ? stateIsoCode.Trim().ToUpper() : stateIsoCode;
            Name = name.HasValue() ? name.Trim().Replace("  ", " ") : name;
        }

        /// <summary>
        /// Return a instance of <see cref="State"/>.
        /// </summary>
        /// <param name="id">Record identifier. Optional if action equal <c>Register</c>. Required for other actions.</param>
        /// <param name="status">Current status of the entity. Required.</param>
        /// <param name="action">Action to take with entity. Required.</param>
        /// <param name="userId">Who is taking this action. Optional.</param>
        /// <param name="countryId">Who owns this state. Required.</param>
        /// <param name="countryIsoCode">Country code consisting of 3 letters, according to ISO-3166. Required.</param>
        /// <param name="stateIsoCode">State code, according to ISO-3166. Required.</param>
        /// <param name="name">Official name. Required.</param>
        /// <returns>Instance of <see cref="State"/>.</returns>
        public static State CreateState(Guid? id, EntityStatus status, TypeProcess action, Guid? userId, Guid countryId, string countryIsoCode, string stateIsoCode, string name)
        {
            return new State(id, status, action, userId, countryId, countryIsoCode, stateIsoCode, name);
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
            var state = other as State;

            return CountryIsoCode == state.CountryIsoCode &&
                   StateIsoCode == state.StateIsoCode &&
                   Name == state.Name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns the country id to the current state.
        /// </summary>
        /// <param name="countryId">Id that identifies the Country.</param>
        public void CreateRelationship(Guid countryId)
        {
            CountryId = countryId;
        }

        #endregion
    }
}
