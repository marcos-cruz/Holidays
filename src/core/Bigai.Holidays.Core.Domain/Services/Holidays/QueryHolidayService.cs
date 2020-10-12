using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Mappers.Holidays;
using Bigai.Holidays.Core.Domain.Services.Abstracts;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Holidays
{
    public class QueryHolidayService : HolidayBaseService, IQueryHolidayService
    {
        #region Private Variables

        private readonly IAddHolidayService _addHolidayService;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="QueryHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing.</param>
        /// <param name="userLogged">User who is logged in.</param>
        public QueryHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _addHolidayService = new AddHolidayService(notificationHandler, unitOfWork, userLogged);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> GetHolidaysAsync(string countryIsoCode, int year)
        {
            _commandName = "GetHolidays";
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                var holidays = await HolidayRepository.GetHolidaysAsync(countryIsoCode, year);
                if (holidays == null || holidays.Count() == 0)
                {
                    commandResult = await _addHolidayService.AddAsync(countryIsoCode, year);
                    if (commandResult.Success)
                    {
                        commandResult = await GetHolidaysAsync(countryIsoCode, year);
                    }
                }
                else
                {
                    commandResult = CommandResult.Ok($"{holidays.Count()} feriados encontrados.");
                    commandResult.Data = holidays.ToResponse();
                }
            }
            catch (Exception)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro na busca.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        #endregion
    }
}
