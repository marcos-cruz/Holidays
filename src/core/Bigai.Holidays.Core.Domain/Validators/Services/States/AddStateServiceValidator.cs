using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Validators.Services.Abstracts;
using FluentValidation;
using System;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Services.States
{
    /// <summary>
    /// <see cref="AddStateServiceValidator"/> represents a set of business rules to add a state to the database.
    /// </summary>
    public class AddStateServiceValidator : StateValidator
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        public AddStateServiceValidator() : base()
        {
        }

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        /// <param name="stateRepository">Context for accessing the repository.</param>
        public AddStateServiceValidator(ICountryRepository countryRepository, IStateRepository stateRepository) : base()
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));

            ValidateCountryId();
            ValidateStateIsoCode();
        }

        #endregion

        #region Validations

        private void ValidateCountryId()
        {
            RuleFor(state => state.CountryId).MustAsync(async (state, countryId, cancellation) =>
            {
                bool exist = await CountryMustExistAsync(state);
                return exist;
            }).WithMessage("País não existe.");
        }

        private async Task<bool> CountryMustExistAsync(State state)
        {
            return await CountryMustExistAsync(state, _countryRepository);
        }

        private void ValidateStateIsoCode()
        {
            RuleFor(country => country.StateIsoCode).MustAsync(async (country, stateIsoCode, cancellation) =>
            {
                bool unique = await StateIsoCodeMustBeUniqueAsync(country);
                return unique;
            }).WithMessage("{PropertyValue} já existe.");
        }

        private async Task<bool> StateIsoCodeMustBeUniqueAsync(State state)
        {
            return await StateIsoCodeMustBeUniqueAsync(state, _stateRepository);
        }

        #endregion
    }
}
