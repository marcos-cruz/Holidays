using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Validators.Services.Abstracts;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Services.Holidays
{
    /// <summary>
    /// <see cref="AddRuleHolidayServiceValidator"/> represents a set of business rules to add a rule of holiday to the database.
    /// </summary>
    public class AddRuleHolidayServiceValidator : RuleHolidayValidator
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
        public AddRuleHolidayServiceValidator() : base()
        {
        }

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        /// <param name="stateRepository">Context for accessing the repository.</param>
        /// <param name="ruleHolidayRepository">Context for accessing the repository.</param>
        public AddRuleHolidayServiceValidator(ICountryRepository countryRepository, IStateRepository stateRepository, IRuleHolidayRepository ruleHolidayRepository) : base()
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _ruleHolidayRepository = ruleHolidayRepository ?? throw new ArgumentNullException(nameof(ruleHolidayRepository));

            ValidateComposeKey();
            ValidateCountryId();
            ValidateStateId();
        }

        #endregion

        #region Validations

        private void ValidateComposeKey()
        {
            RuleFor(ruleHoliday => ruleHoliday.ComposeKey).MustAsync(async (ruleHoliday, composeKey, cancellation) =>
            {
                bool unique = await RuleHolidayMustBeUniqueAsync(ruleHoliday);
                return unique;
            }).WithMessage("Regra de feriado já existe.");
        }

        private void ValidateCountryId()
        {
            RuleFor(ruleHoliday => ruleHoliday.CountryId).MustAsync(async (ruleHoliday, countryId, cancellation) =>
            {
                bool exist = await CountryMustExistAsync(ruleHoliday);
                return exist;
            }).WithMessage("País não existe.");
        }

        private void ValidateStateId()
        {
            RuleFor(ruleHoliday => ruleHoliday.StateId).MustAsync(async (ruleHoliday, stateId, cancellation) =>
            {
                bool exist = await StateMustExistAsync(ruleHoliday);
                return exist;
            }).WithMessage("Estado não existe.");
        }

        private async Task<bool> RuleHolidayMustBeUniqueAsync(RuleHoliday ruleHoliday)
        {
            return await RuleHolidayMustBeUniqueAsync(ruleHoliday, _ruleHolidayRepository);
        }

        private async Task<bool> RuleHolidayMustBeUniqueAsync(RuleHoliday ruleHoliday, IRuleHolidayRepository ruleRepository)
        {
            RuleHoliday record = (await ruleRepository.FindAsync(c => c.ComposeKey == ruleHoliday.ComposeKey)).FirstOrDefault();

            if (ruleHoliday.Action != ActionType.Register && record.Id == ruleHoliday.Id)
            {
                record = null;
            }

            return record == null;
        }

        private async Task<bool> CountryMustExistAsync(RuleHoliday ruleHoliday)
        {
            return await CountryMustExistAsync(ruleHoliday, _countryRepository);
        }

        private async Task<bool> StateMustExistAsync(RuleHoliday ruleHoliday)
        {
            if (!ruleHoliday.StateId.HasValue)
            {
                return true;
            }

            return await StateMustExistAsync(ruleHoliday, _stateRepository);
        }

        #endregion
    }
}
