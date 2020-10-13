using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Core.Domain.Validators.Abstracts;
using FluentValidation;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Validators.Holidays
{
    public class GetHolidaysByCountryRequestValidator : RequestValidator<GetHolidaysByCountryRequest>
    {
        #region Constructor

        public GetHolidaysByCountryRequestValidator(ICountryRepository countryRepository, IStateRepository stateRepository) : base(countryRepository, stateRepository)
        {
            ValidateCountryIsoCode();
            ValidateYear();
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

        private void ValidateYear()
        {
            RuleFor(request => request.Year)
                .InclusiveBetween(1900, 2300).WithMessage("Ano deve estar no intervalo entre 1900 e 2300.");
        }

        private async Task<bool> CountryMustExistAsync(GetHolidaysByCountryRequest request)
        {
            return await CountryMustExistAsync(request.CountryIsoCode);
        }

        #endregion
    }
}
