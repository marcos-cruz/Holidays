using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Requests.Holidays.Abstracts;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Requests.Abstracts
{
    public abstract class RequestValidator<T> : AbstractValidator<T> where T : HolidaysRequest
    {
        #region Private Variable

        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;

        #endregion

        #region Constructor

        protected RequestValidator(ICountryRepository countryRepository, IStateRepository stateRepository)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));

            ValidateCountryIsoCode();
        }

        #endregion

        #region Validations

        private void ValidateCountryIsoCode()
        {
            RuleFor(request => request.CountryIsoCode).MustAsync(async (request, countryIsoCode, cancellation) =>
            {
                bool exist = await CountryMustExistAsync(request);
                return exist;
            }).WithMessage("Não existe país {PropertyValue}.");
        }

        private async Task<bool> CountryMustExistAsync(HolidaysRequest request)
        {
            return await CountryMustExistAsync(request.CountryIsoCode);
        }

        protected async Task<bool> CountryMustExistAsync(string countryIsoCode3)
        {
            Country country = (await _countryRepository.FindAsync(c => c.CountryIsoCode3 == countryIsoCode3)).FirstOrDefault();

            return country != null;
        }

        protected async Task<bool> StateMustExistAsync(string countryIsoCode3, string stateIsoCode)
        {
            State state = (await _stateRepository.FindAsync(s => s.CountryIsoCode == countryIsoCode3 && s.StateIsoCode == stateIsoCode)).FirstOrDefault();

            return state != null;
        }

        #endregion
    }
}
