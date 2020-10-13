using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Services.Abstracts;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Queries.Abstracts
{
    public abstract class QueryBaseService : HolidayBaseService
    {
        #region Private Variables

        private readonly IAddHolidayService _addHolidayService;

        #endregion

        #region Constructor

        protected QueryBaseService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddHolidayService addHolidayService) : base(notificationHandler, unitOfWork, userLogged)
        {
            _addHolidayService = addHolidayService ?? throw new ArgumentNullException(nameof(addHolidayService));
        }

        #endregion

        #region Protected Methods

        protected async Task<bool> HolidaysAlreadyExist(string countryIsoCode, int year)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            return (await HolidayRepository.GetCountAsync(h => h.CountryCode == countryIsoCode && h.HolidayDate >= startDate && h.HolidayDate <= endDate)) > 0;
        }

        protected async Task<CommandResult> CreateHoliday(string countryIsoCode, int year)
        {
            return await _addHolidayService.AddAsync(countryIsoCode, year);
        }

        #endregion
    }
}
