using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using FluentValidation;
using System;
using System.Threading.Tasks;

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
        public AddCountryValidator() : base()
        {
        }

        /// <summary>
        /// Determines whether the record meets the business rules for adding a new record.
        /// </summary>
        /// <param name="countryRepository">Context for accessing the repository.</param>
        public AddCountryValidator(ICountryRepository countryRepository) : base()
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));

            ValidateAlphaIsoCode2();
            ValidateAlphaIsoCode3();
        }

        #endregion

        #region Validations

        private void ValidateAlphaIsoCode2()
        {
            RuleFor(country => country.CountryIsoCode2).MustAsync(async (country, countryIsoCode2, cancellation) =>
            {
                bool unique = await AlphaIsoCode2MustBeUniqueAsync(country);
                return unique;
            }).WithMessage("{PropertyValue} já existe.");
        }

        private void ValidateAlphaIsoCode3()
        {
            RuleFor(country => country.CountryIsoCode3).MustAsync(async (country, countryIsoCode3, cancellation) =>
            {
                bool unique = await AlphaIsoCode3MustBeUniqueAsync(country);
                return unique;
            }).WithMessage("{PropertyValue} já existe.");
        }

        private async Task<bool> AlphaIsoCode2MustBeUniqueAsync(Country country)
        {
            return await AlphaIsoCode2MustBeUniqueAsync(country, _countryRepository);
        }

        private async Task<bool> AlphaIsoCode3MustBeUniqueAsync(Country country)
        {
            return await AlphaIsoCode3MustBeUniqueAsync(country, _countryRepository);
        }

        #endregion
    }
}
