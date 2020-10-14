using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Core.Domain.Validators.Requests.Abstracts;
using FluentValidation;

namespace Bigai.Holidays.Core.Domain.Validators.Requests.Holidays
{
    public class GetHolidaysByDateRequestValidator : RequestValidator<GetHolidaysByDateRequest>
    {
        #region Constructor

        public GetHolidaysByDateRequestValidator(ICountryRepository countryRepository, IStateRepository stateRepository) : base(countryRepository, stateRepository)
        {
            ValidateYear();
        }

        #endregion

        #region Validations

        private void ValidateYear()
        {
            RuleFor(request => request.HolidayDate.Year)
                .InclusiveBetween(1900, 2300).WithMessage("Ano deve estar no intervalo entre 1900 e 2300.");
        }

        #endregion
    }
}
