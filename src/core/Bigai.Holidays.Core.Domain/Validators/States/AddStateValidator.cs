using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.States;
using FluentValidation;
using System;

namespace Bigai.Holidays.Core.Domain.Validators.States
{
    /// <summary>
    /// <see cref="AddStateValidator"/> represents a set of business rules to add a state to the database.
    /// </summary>
    public class AddStateValidator : StateValidator
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        public AddStateValidator(ICountryRepository countryRepository, IStateRepository stateRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));

            CommonValidations();

            ValidateCountryId();
            ValidateStateIsoCode();
        }

        #endregion

        #region Validations

        private void ValidateCountryId()
        {
            RuleFor(state => state.CountryId)
                .Must(CountryMustExist).WithMessage("País não existe.");
        }

        private bool CountryMustExist(State state, Guid countryId)
        {
            return CountryMustExist(state, _countryRepository);
        }

        private void ValidateStateIsoCode()
        {
            RuleFor(country => country.StateIsoCode)
                .Must(StateIsoCodeMustBeUnique).WithMessage("Código ISO do estado já existe.");
        }

        private bool StateIsoCodeMustBeUnique(State state, string stateIsoCode)
        {
            return StateIsoCodeMustBeUnique(state, _stateRepository);
        }

        #endregion
    }
}
