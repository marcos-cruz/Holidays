using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Requests;
using FluentValidation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Abstracts
{
    public abstract class RequestValidator<T> : AbstractValidator<T> where T : Request
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
        }

        #endregion

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
    }
}
