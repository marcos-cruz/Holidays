using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Validators.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Validators;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Bigai.Holidays.Core.Domain.Validators.Countries
{
    /// <summary>
    /// This class provides support for validating <see cref="Country"/>.
    /// </summary>
    public class CountryValidator : EntityValidatorError<Country>
    {
        #region Constructor

        public CountryValidator()
        {
            CommonValidations();
        }

        #endregion

        #region Validations

        protected void CommonValidations()
        {
            ValidateNumericCode();
            ValidateAlphaIsoCode2();
            ValidateAlphaIsoCode3();
            ValidateName();
            ValidateShortName();
            ValidateLanguageCode();
            ValidateRegionName();
            ValidateSubRegionName();
            ValidateIntermediateRegionName();
            ValidateRegionCode();
            ValidateSubRegionCode();
            ValidateStates();
        }

        private void ValidateNumericCode()
        {
            When(country => country.NumericCode.HasValue(), () =>
            {
                RuleFor(country => country.NumericCode)
                    .Must(BePositiveInteger).WithMessage("Código numérico do país não é válido.")
                    .MaximumLength(int.MaxValue.ToString().Length).WithMessage($"Código numérico deve ter no máximo { int.MaxValue.ToString().Length} caracteres.");
            });
        }

        private void ValidateAlphaIsoCode2()
        {
            RuleFor(country => country.CountryIsoCode2)
                .NotEmpty().WithMessage("Código ISO 2 do país deve ser informado.")
                .Length(2).WithMessage("Código ISO 2 do país deve ter 2 caracteres.");
        }

        private void ValidateAlphaIsoCode3()
        {
            RuleFor(country => country.CountryIsoCode3)
                .NotEmpty().WithMessage("Código ISO 3 do país deve ser informado.")
                .Length(3).WithMessage("Código ISO 3 do país deve ter 3 caracteres.");
        }

        private void ValidateName()
        {
            When(country => !country.Name.HasValue(), () =>
            {
                RuleFor(state => state.Name)
                    .NotEmpty().WithMessage("Nome oficial do país deve ser informado.");
            });

            When(country => country.Name.HasValue(), () =>
            {
                RuleFor(country => country.Name)
                    .MaximumLength(100).WithMessage("Nome oficial do país deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateShortName()
        {
            When(country => !country.ShortName.HasValue(), () =>
            {
                RuleFor(state => state.ShortName)
                    .NotEmpty().WithMessage("Nome abreviado do país deve ser informado.");
            });

            When(country => country.ShortName.HasValue(), () =>
            {
                RuleFor(country => country.ShortName)
                    .MaximumLength(100).WithMessage("Nome abreviado do país deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateLanguageCode()
        {
            When(country => !country.LanguageCode.HasValue(), () =>
            {
                RuleFor(state => state.LanguageCode)
                    .NotEmpty().WithMessage("Código da linguagem do país deve ser informado.");
            });

            When(country => country.LanguageCode.HasValue(), () =>
            {
                RuleFor(country => country.LanguageCode)
                    .MaximumLength(100).WithMessage("Código da linguagem deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateRegionName()
        {
            When(country => !country.RegionName.HasValue(), () =>
            {
                RuleFor(state => state.RegionName)
                    .NotEmpty().WithMessage("Continente do país deve ser informado.");
            });

            When(country => country.RegionName.HasValue(), () =>
            {
                RuleFor(country => country.RegionName)
                    .MaximumLength(100).WithMessage("Continente do país deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateSubRegionName()
        {
            When(country => !country.SubRegionName.HasValue(), () =>
            {
                RuleFor(state => state.SubRegionName)
                    .NotEmpty().WithMessage("Nome da sub divisão do continente deve ser informado.");
            });

            When(country => country.SubRegionName.HasValue(), () =>
            {
                RuleFor(country => country.SubRegionName)
                    .MaximumLength(100).WithMessage("Nome da sub divisão do continente deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateIntermediateRegionName()
        {
            When(country => country.IntermediateRegionName.HasValue(), () =>
            {
                RuleFor(country => country.IntermediateRegionName)
                    .MaximumLength(100).WithMessage("Nome intermediario do continente deve ter no máximo 100 caracteres.");
            });
        }

        private void ValidateRegionCode()
        {
            RuleFor(country => country.RegionCode)
                .NotEmpty().WithMessage("Código do continente deve ser informado.");
        }

        private void ValidateSubRegionCode()
        {
            RuleFor(country => country.SubRegionCode)
                .NotEmpty().WithMessage("Código do sub continente deve ser informado.");
        }

        private void ValidateStates()
        {
            When(country => country.States != null, () =>
            {
                //
                // Test if any state is duplicated in the list
                //
                When(country => country.States.Count > 1, () =>
                {
                    RuleFor(country => country.States)
                        .Must(BeUnique).WithMessage("Não podem existir estados duplicados.");
                });

                //
                // Test if states are valid.
                //
                When(country => country.States.Count > 0, () =>
                {
                    RuleForEach(country => country.States)
                        .SetValidator(new StateValidator());
                });
            });
        }

        private bool BeUnique(IReadOnlyCollection<State> listOfStates)
        {
            if (listOfStates.Count < 2)
            {
                return true;
            }

            bool duplicate = false;
            List<State> list = listOfStates.ToList();

            for (int i = 0, j = list.Count; i < j && !duplicate; i++)
            {
                for (int k = i + 1; k < j && !duplicate; k++)
                {
                    duplicate = list[i] == list[k];
                }
            }

            return !duplicate;
        }

        protected bool AlphaIsoCode2MustBeUnique(Country country, ICountryRepository countryRepository)
        {
            Country record = countryRepository.Find(c => c.CountryIsoCode2 == country.CountryIsoCode2).FirstOrDefault();

            if (country.Action != TypeProcess.Register && record.Id == country.Id)
            {
                record = null;
            }

            return record == null;
        }

        protected bool AlphaIsoCode3MustBeUnique(Country country, ICountryRepository countryRepository)
        {
            Country record = countryRepository.Find(c => c.CountryIsoCode3 == country.CountryIsoCode3).FirstOrDefault();

            if (country.Action != TypeProcess.Register && record.Id == country.Id)
            {
                record = null;
            }

            return record == null;
        }

        #endregion
    }
}
