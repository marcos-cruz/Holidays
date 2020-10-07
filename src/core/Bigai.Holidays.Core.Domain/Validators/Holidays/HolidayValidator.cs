using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Validators;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using FluentValidation;
using System;

namespace Bigai.Holidays.Core.Domain.Validators.Holidays
{
    /// <summary>
    /// This class provides support for common <see cref="Holiday"/> validations.
    /// </summary>
    public abstract class HolidayValidator : EntityValidatorError<Holiday>
    {
        public HolidayValidator()
        {
            CommonValidations();
        }

        private void CommonValidations()
        {
            ValidateCountryId();
            ValidateCityId();
            ValidateCityName();
            ValidateHolidayType();
            ValidateNativeDescription();
            ValidateAlternativeDescription();
        }

        private void ValidateCountryId()
        {
            RuleFor(holiday => holiday.CountryId)
                .NotEmpty().WithMessage("País deve ser informado.")
                .NotEqual(Guid.Empty).WithMessage("País não é válido.");
        }

        private void ValidateCityId()
        {
            When(holiday => !holiday.CityId.HasValue() && holiday.CityName.HasValue(), () =>
            {
                RuleFor(holiday => holiday.CityId)
                    .NotEmpty().WithMessage("Id da cidade deve ser informado.");
            });

            When(holiday => holiday.CityId.HasValue() && holiday.CityName.HasValue(), () =>
            {
                RuleFor(holiday => holiday.CityId)
                    .MaximumLength(32).WithMessage("Id da cidade deve ter no máximo 32 caracteres.");
            });
        }

        private void ValidateCityName()
        {
            When(holiday => !holiday.CityName.HasValue() && holiday.CityCode.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityName)
                    .NotEmpty().WithMessage("Nome da cidade deve ser informado.");
            });

            When(holiday => holiday.CityId.HasValue() && !holiday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityName)
                    .NotEmpty().WithMessage("Nome da cidade deve ser informado.");
            });

            When(holiday => holiday.CityId.HasValue() && holiday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityName)
                    .MaximumLength(100).WithMessage("Nome da cidade deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateHolidayType()
        {
            RuleFor(ruleHoliday => ruleHoliday.HolidayType)
                .Must(BeHolidayType).WithMessage("{PropertyValue} não é um tipo de feriado válido.");
        }

        private void ValidateNativeDescription()
        {
            RuleFor(holiday => holiday.NativeDescription)
                .NotEmpty().WithMessage("Nome do feriado na língua nativa deve ser informado.")
                .MaximumLength(100).WithMessage("Nome do feriado na língua nativa deve ter no máximo 100 caracteres.");
        }

        private void ValidateAlternativeDescription()
        {
            When(holiday => !holiday.AlternativeDescription.HasValue(), () =>
            {
                RuleFor(holiday => holiday.AlternativeDescription)
                    .NotEmpty().WithMessage("Nome do feriado na língua inglesa deve ser informado.");
            });

            When(holiday => holiday.AlternativeDescription.HasValue(), () =>
            {
                RuleFor(holiday => holiday.AlternativeDescription)
                    .MaximumLength(100).WithMessage("Nome do feriado na língua inglesa deve ter no máximo 100 caracteres.");
            });
        }

        protected bool BeHolidayType(HolidayType holidayType)
        {
            return holidayType != null && HolidayType.GetById(holidayType.Key) != null;
        }

        protected bool CountryMustExist(Holiday holiday, ICountryRepository countryRepository)
        {
            Country record = countryRepository.GetById(holiday.CountryId);

            if (record != null && holiday.CountryCode != record.CountryIsoCode3)
            {
                record = null;
            }

            return record == null;
        }

        protected bool StateMustExist(Holiday holiday, IStateRepository stateRepository)
        {
            State record = stateRepository.GetById(holiday.StateId.Value);

            if (record != null && holiday.StateCode != record.StateIsoCode)
            {
                record = null;
            }

            return record == null;
        }
    }
}
