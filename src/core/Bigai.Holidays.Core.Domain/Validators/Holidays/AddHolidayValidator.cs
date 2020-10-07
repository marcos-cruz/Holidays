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
    /// <see cref="AddHolidayValidator"/> represents a set of business rules to add a holiday to the database.
    /// </summary>
    public class AddHolidayValidator : HolidayValidator
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
        /// <param name="countryRepository">Context for accessing the repository.</param>
        /// <param name="stateRepository">Context for accessing the repository.</param>
        /// <param name="holidayRepository">Context for accessing the repository.</param>
        public AddHolidayValidator(ICountryRepository countryRepository, IStateRepository stateRepository, IHolidayRepository holidayRepository)
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
            RuleFor(holiday => holiday.ComposeKey)
                .Must(HolidayMustBeUnique).WithMessage("Feriado já existe.");
        }

        private void ValidateCountryId()
        {
            RuleFor(holiday => holiday.CountryId)
                .Must(CountryMustExist).WithMessage("País não existe.");
        }

        private void ValidateStateId()
        {
            RuleFor(holiday => holiday.StateId)
                .Must(StateMustExist).WithMessage("Estado não existe.");
        }

        private bool HolidayMustBeUnique(Holiday holiday, string composeKey)
        {
            return HolidayMustBeUnique(holiday, _holidayRepository);
        }

        private bool HolidayMustBeUnique(Holiday holiday, IHolidayRepository holidayRepository)
        {
            Holiday record = holidayRepository.Find(c => c.ComposeKey == holiday.ComposeKey).FirstOrDefault();

            if (holiday.Action != TypeProcess.Register && record.Id == holiday.Id)
            {
                record = null;
            }

            return record == null;
        }

        private bool CountryMustExist(Holiday holiday, Guid countryId)
        {
            return CountryMustExist(holiday, _countryRepository);
        }

        private bool StateMustExist(Holiday holiday, Guid? stateId)
        {
            if (!holiday.StateId.HasValue)
            {
                return true;
            }

            return StateMustExist(holiday, _stateRepository);
        }

        #endregion
    }
}
