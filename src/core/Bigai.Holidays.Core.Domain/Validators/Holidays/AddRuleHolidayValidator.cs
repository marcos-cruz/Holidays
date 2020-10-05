using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using FluentValidation;
using System;
using System.Linq;

namespace Bigai.Holidays.Core.Domain.Validators.Holidays
{
    /// <summary>
    /// <see cref="AddRuleHolidayValidator"/> represents a set of business rules to add a rule of holiday to the database.
    /// </summary>
    public class AddRuleHolidayValidator : RuleHolidayValidator
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IRuleHolidayRepository _ruleHolidayRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        /// <param name="stateRepository">Context for accessing the repository.</param>
        /// <param name="ruleHolidayRepository">Context for accessing the repository.</param>
        public AddRuleHolidayValidator(ICountryRepository countryRepository, IStateRepository stateRepository, IRuleHolidayRepository ruleHolidayRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _ruleHolidayRepository = ruleHolidayRepository ?? throw new ArgumentNullException(nameof(ruleHolidayRepository));

            CommonValidations();
            ValidateComposeKey();
            ValidateCountryId();
            ValidateStateId();
        }

        #endregion

        #region Validations

        private void ValidateComposeKey()
        {
            RuleFor(ruleHoliday => ruleHoliday.ComposeKey)
                .Must(RuleHolidayMustBeUnique).WithMessage("Regra de feriado já existe.");
        }

        private void ValidateCountryId()
        {
            RuleFor(ruleHoliday => ruleHoliday.CountryId)
                .Must(CountryMustExist).WithMessage("País não existe.");
        }

        private void ValidateStateId()
        {
            RuleFor(ruleHoliday => ruleHoliday.StateId)
                .Must(StateMustExist).WithMessage("Estado não existe.");
        }

        private bool RuleHolidayMustBeUnique(RuleHoliday ruleHoliday, string composeKey)
        {
            return RuleHolidayMustBeUnique(ruleHoliday, _ruleHolidayRepository);
        }

        private bool RuleHolidayMustBeUnique(RuleHoliday ruleHoliday, IRuleHolidayRepository ruleRepository)
        {
            RuleHoliday record = ruleRepository.Find(c => c.ComposeKey == ruleHoliday.ComposeKey).FirstOrDefault();

            if (ruleHoliday.Action != TypeProcess.Register && record.Id == ruleHoliday.Id)
            {
                record = null;
            }

            return record == null;
        }

        private bool CountryMustExist(RuleHoliday ruleHoliday, Guid countryId)
        {
            return CountryMustExist(ruleHoliday, _countryRepository);
        }

        private bool StateMustExist(RuleHoliday ruleHoliday, Guid? stateId)
        {
            if (!ruleHoliday.StateId.HasValue)
            {
                return true;
            }

            return StateMustExist(ruleHoliday, _stateRepository);
        }

        #endregion
    }
}
