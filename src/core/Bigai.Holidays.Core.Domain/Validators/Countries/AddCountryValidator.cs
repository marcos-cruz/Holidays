using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using FluentValidation;
using System;

namespace Bigai.Holidays.Core.Domain.Validators.Countries
{
    /// <summary>
    /// <see cref="AddCountryValidator"/> represents a set of business rules to add a country to the database.
    /// </summary>
    public class AddCountryValidator : CountryValidator
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        public AddCountryValidator(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));

            CommonValidations();

            ValidateAlphaIsoCode2();
            ValidateAlphaIsoCode3();
        }

        #endregion

        #region Validations

        private void ValidateAlphaIsoCode2()
        {
            RuleFor(country => country.CountryIsoCode2)
                .Must(AlphaIsoCode2MustBeUnique).WithMessage("Código ISO 2 do país já existe.");
        }

        private void ValidateAlphaIsoCode3()
        {
            RuleFor(country => country.CountryIsoCode3)
                .Must(AlphaIsoCode3MustBeUnique).WithMessage("Código ISO 3 do país já existe.");
        }

        private bool AlphaIsoCode2MustBeUnique(Country country, string alphaIsoCode2MustBeUnique)
        {
            return AlphaIsoCode2MustBeUnique(country, _countryRepository);
        }

        private bool AlphaIsoCode3MustBeUnique(Country country, string alphaIsoCode3MustBeUnique)
        {
            return AlphaIsoCode3MustBeUnique(country, _countryRepository);
        }

        #endregion
    }
}
