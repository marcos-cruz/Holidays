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
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Holidays
{
    /// <summary>
    /// This class provides support for common <see cref="RuleHoliday"/> validations.
    /// </summary>
    public abstract class RuleHolidayValidator : EntityValidatorError<RuleHoliday>
    {
        #region Constructor

        public RuleHolidayValidator()
        {
            CommonValidations();
        }

        #endregion

        #region Validations

        private void CommonValidations()
        {
            ValidateCountryId();
            ValidateStateId();
            ValidateCountryIsoCode();
            ValidateStateIsoCode();
            ValidateCityId();
            ValidateCityCode();
            ValidateCityName();
            ValidateHolidayType();
            ValidateNativeHolidayName();
            ValidateEnglishHolidayName();
            ValidateMonth();
            ValidateDay();
            ValidateBussinessRule();
            ValidateComments();
        }

        private void ValidateCountryId()
        {
            RuleFor(ruleHoliday => ruleHoliday.CountryId)
                .NotEmpty().WithMessage("País deve ser informado.")
                .NotEqual(Guid.Empty).WithMessage("País não é válido.");
        }

        private void ValidateStateId()
        {
            When(ruleHoliday => ruleHoliday.StateIsoCode.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.StateId)
                    .NotEmpty().WithMessage("Estado deve ser informado.")
                    .NotEqual(Guid.Empty).WithMessage("Estado não é válido.");
            });
        }

        private void ValidateCountryIsoCode()
        {
            RuleFor(ruleHoliday => ruleHoliday.CountryIsoCode)
                .NotEmpty().WithMessage("Código ISO do país deve ser informado.")
                .Must(BeCountry).WithMessage("{PropertyValue} não é um código ISO de país válido.");
        }

        private void ValidateStateIsoCode()
        {
            When(ruleHoliday => ruleHoliday.StateIsoCode.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.StateIsoCode)
                    .Must(BeState).WithMessage("Código ISO do estado deve ser informado.");
            });
            
            When(ruleHoliday => !ruleHoliday.StateIsoCode.HasValue() && ruleHoliday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.StateIsoCode)
                    .NotEmpty().WithMessage("Código ISO do estado deve ser informado.");
            });
        }

        private void ValidateCityId()
        {
            When(ruleHoliday => !ruleHoliday.CityId.HasValue() && ruleHoliday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityId)
                    .NotEmpty().WithMessage("Id da cidade deve ser informado.");
            });

            When(ruleHoliday => ruleHoliday.CityId.HasValue() && ruleHoliday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityId)
                    .MaximumLength(32).WithMessage("Id da cidade deve ter no máximo 32 caracteres.");
            });
        }

        private void ValidateCityCode()
        {
            When(ruleHoliday => ruleHoliday.CityCode.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityCode)
                    .MaximumLength(32).WithMessage("Código da cidade deve ter no máximo 32 caracteres.");
            });
        }

        private void ValidateCityName()
        {
            When(rule => !rule.CityName.HasValue() && rule.CityCode.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityName)
                    .NotEmpty().WithMessage("Nome da cidade deve ser informado.");
            });

            When(ruleHoliday => ruleHoliday.CityId.HasValue() && !ruleHoliday.CityName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.CityName)
                    .NotEmpty().WithMessage("Nome da cidade deve ser informado.");
            });

            When(ruleHoliday => ruleHoliday.CityId.HasValue() && ruleHoliday.CityName.HasValue(), () =>
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

        private void ValidateNativeHolidayName()
        {
            RuleFor(ruleHoliday => ruleHoliday.NativeHolidayName)
                .NotEmpty().WithMessage("Nome do feriado na língua nativa deve ser informado.")
                .MaximumLength(100).WithMessage("Nome do feriado na língua nativa deve ter no máximo 100 caracteres.");
        }

        private void ValidateEnglishHolidayName()
        {
            When(rule => !rule.EnglishHolidayName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.EnglishHolidayName)
                    .NotEmpty().WithMessage("Nome do feriado na língua inglesa deve ser informado.");
            });

            When(ruleHoliday => ruleHoliday.EnglishHolidayName.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.EnglishHolidayName)
                    .MaximumLength(100).WithMessage("Nome do feriado na língua inglesa deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateMonth()
        {
            When(ruleHoliday => ruleHoliday.Month.HasValue && !ruleHoliday.BussinessRule.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.Month)
                    .GreaterThanOrEqualTo(1).WithMessage("{PropertyValue} não é um mês válido.")
                    .LessThanOrEqualTo(12).WithMessage("{PropertyValue} não é um mês válido.");
            });
        }

        private void ValidateDay()
        {
            When(ruleHoliday => ruleHoliday.Day.HasValue && !ruleHoliday.BussinessRule.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.Day)
                    .GreaterThanOrEqualTo(1).WithMessage("{PropertyValue} não é um dia válido.")
                    .LessThanOrEqualTo(31).WithMessage("{PropertyValue} não é um dia válido.");
            });
        }

        private void ValidateBussinessRule()
        {
            When(ruleHoliday => ruleHoliday.BussinessRule.HasValue(), () =>
            {
                RuleFor(state => state.BussinessRule)
                    .MaximumLength(100).WithMessage("Regra de negócio deve ter no máximo 100 caracteres.");
            });

            When(rule => !rule.BussinessRule.HasValue() && (!rule.Month.HasValue || !rule.Day.HasValue), () =>
            {
                RuleFor(country => country.BussinessRule)
                    .NotEmpty().WithMessage("Regra de negócio deve ser informada.");
            });
        }

        private void ValidateComments()
        {
            When(ruleHoliday => ruleHoliday.Comments.HasValue(), () =>
            {
                RuleFor(ruleHoliday => ruleHoliday.Comments)
                    .MaximumLength(100).WithMessage("Comentário deve ter no máximo 100 caracteres.");
            });
        }

        private bool BeState(RuleHoliday ruleHoliday, string stateIsoCode)
        {
            return BeState(ruleHoliday.CountryIsoCode, stateIsoCode);
        }

        protected bool BeHolidayType(HolidayType holidayType)
        {
            return holidayType != null && HolidayType.GetById(holidayType.Key) != null;
        }

        protected async Task<bool> CountryMustExistAsync(RuleHoliday ruleHoliday, ICountryRepository countryRepository)
        {
            Country record = await countryRepository.GetByIdAsync(ruleHoliday.CountryId);

            if (record != null && ruleHoliday.CountryIsoCode != record.CountryIsoCode3)
            {
                record = null;
            }

            return record != null;
        }

        protected async Task<bool> StateMustExistAsync(RuleHoliday ruleHoliday, IStateRepository stateRepository)
        {
            State record = await stateRepository.GetByIdAsync(ruleHoliday.StateId.Value);

            if (record != null && ruleHoliday.StateIsoCode != record.StateIsoCode)
            {
                record = null;
            }

            return record != null;
        }

        #endregion
    }
}
