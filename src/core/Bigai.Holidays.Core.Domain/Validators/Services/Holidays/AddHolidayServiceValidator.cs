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
    /// <see cref="AddHolidayServiceValidator"/> represents a set of business rules to add a holiday to the database.
    /// </summary>
    public class AddHolidayServiceValidator : HolidayValidator
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IHolidayRepository _holidayRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        public AddHolidayServiceValidator() : base()
        {
        }

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        /// <param name="stateRepository">Context for accessing the repository.</param>
        /// <param name="holidayRepository">Context for accessing the repository.</param>
        public AddHolidayServiceValidator(ICountryRepository countryRepository, IStateRepository stateRepository, IHolidayRepository holidayRepository) : base()
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _holidayRepository = holidayRepository ?? throw new ArgumentNullException(nameof(holidayRepository));

            ValidateComposeKey();
            ValidateCountryId();
            ValidateStateId();
        }

        #endregion

        #region Validations

        private void ValidateComposeKey()
        {
            RuleFor(holiday => holiday.ComposeKey).MustAsync(async (holiday, composeKey, cancellation) =>
            {
                bool unique = await HolidayMustBeUniqueAsync(holiday);
                return unique;
            }).WithMessage("Feriado já existe.");
        }

        private void ValidateCountryId()
        {
            RuleFor(holiday => holiday.CountryId).MustAsync(async (holiday, countryId, cancellation) =>
            {
                bool unique = await CountryMustExistAsync(holiday);
                return unique;
            }).WithMessage("País não existe.");
        }

        private void ValidateStateId()
        {
            RuleFor(holiday => holiday.StateId).MustAsync(async (holiday, stateId, cancellation) =>
            {
                bool unique = await StateMustExistAsync(holiday);
                return unique;
            }).WithMessage("Estado não existe.");
        }

        private async Task<bool> HolidayMustBeUniqueAsync(Holiday holiday)
        {
            return await HolidayMustBeUniqueAsync(holiday, _holidayRepository);
        }

        private async Task<bool> HolidayMustBeUniqueAsync(Holiday holiday, IHolidayRepository holidayRepository)
        {
            Holiday record = (await holidayRepository.FindAsync(c => c.ComposeKey == holiday.ComposeKey)).FirstOrDefault();

            if (holiday.Action != ActionType.Register && record.Id == holiday.Id)
            {
                record = null;
            }

            return record == null;
        }

        private async Task<bool> CountryMustExistAsync(Holiday holiday)
        {
            return await CountryMustExistAsync(holiday, _countryRepository);
        }

        private async Task<bool> StateMustExistAsync(Holiday holiday)
        {
            if (!holiday.StateId.HasValue)
            {
                return true;
            }

            return await StateMustExistAsync(holiday, _stateRepository);
        }

        #endregion
    }
}
