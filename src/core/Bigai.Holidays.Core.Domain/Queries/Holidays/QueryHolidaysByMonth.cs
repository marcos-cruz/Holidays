using Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Mappers.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Queries.Abstracts;
using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Core.Domain.Validators.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Queries.Holidays
{
    /// <summary>
    /// <see cref="QueryHolidaysByMonth"/> implements a contract for searching a country's holidays for a specific month.
    /// </summary>
    public class QueryHolidaysByMonth : QueryBaseService, IQueryHolidaysByMonth
    {
        #region Private Variables

        private readonly GetHolidaysByMonthRequestValidator _getHolidaysByMonthRequestValidator;

        #endregion

        #region Constructor

        public QueryHolidaysByMonth(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddHolidayService addHolidayService) : base(notificationHandler, unitOfWork, userLogged, addHolidayService)
        {
            _getHolidaysByMonthRequestValidator = new GetHolidaysByMonthRequestValidator(unitOfWork.CountryRepository, unitOfWork.StateRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> GetHolidaysByMonthAsync(GetHolidaysByMonthRequest request)
        {
            _commandName = request.GetType().Name;
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanGetHolidaysByMonthAsync(request))
                {
                    commandResult = CommandResult.BadRequest("Não foi possível realizar a consulta.");
                }
                else
                {
                    var holidays = await GetHolidaysByMonthAsync(request.CountryIsoCode, request.Year, request.Month);
                    if (holidays != null && holidays.Count() > 0)
                    {
                        commandResult = CommandResult.Ok($"{holidays.Count()} feriados encontrados.");
                        commandResult.Data = holidays.ToResponse();
                    }
                    else
                    {
                        commandResult = CommandResult.BadRequest($"Não existe feriado cadastrado para {request.CountryIsoCode} em {request.Year}/{request.Month}.");
                    }
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

        #region Private Methods

        private async Task<bool> CanGetHolidaysByMonthAsync(GetHolidaysByMonthRequest request)
        {
            return InstanceNotNull(request) && (await IsValidRequestAsync(_getHolidaysByMonthRequestValidator, request));
        }

        private async Task<IEnumerable<Holiday>> GetHolidaysByMonthAsync(string countryIsoCode, int year, int month)
        {
            if (!await HolidaysAlreadyExist(countryIsoCode, year))
            {
                await CreateHoliday(countryIsoCode, year);
            }

            return await HolidayRepository.GetHolidaysByMonthAsync(countryIsoCode, year, month);
        }

        #endregion
    }
}
