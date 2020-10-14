using Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Mappers.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Queries.Abstracts;
using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Core.Domain.Validators.Requests.Holidays;
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
    public class QueryHolidaysByDate : QueryBaseService, IQueryHolidaysByDate
    {
        #region Private Variables

        private readonly GetHolidaysByDateRequestValidator _getHolidaysByDateRequestValidator;

        #endregion

        #region Constructor

        public QueryHolidaysByDate(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddHolidayService addHolidayService) : base(notificationHandler, unitOfWork, userLogged, addHolidayService)
        {
            _getHolidaysByDateRequestValidator = new GetHolidaysByDateRequestValidator(unitOfWork.CountryRepository, unitOfWork.StateRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> GetHolidaysByDateAsync(GetHolidaysByDateRequest request)
        {
            _commandName = request.GetType().Name;
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanGetHolidaysByDateAsync(request))
                {
                    commandResult = CommandResult.BadRequest("Não foi possível realizar a consulta.");
                }
                else
                {
                    var holidays = await GetHolidaysByDateAsync(request.CountryIsoCode, request.HolidayDate);
                    if (holidays != null && holidays.Count() > 0)
                    {
                        commandResult = CommandResult.Ok($"{holidays.Count()} feriados encontrados.");
                        commandResult.Data = holidays.ToResponse();
                    }
                    else
                    {
                        commandResult = CommandResult.BadRequest($"Não existe feriado cadastrado para {request.CountryIsoCode} em {request.HolidayDate}.");
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

        private async Task<bool> CanGetHolidaysByDateAsync(GetHolidaysByDateRequest request)
        {
            return InstanceNotNull(request) && (await IsValidRequestAsync(_getHolidaysByDateRequestValidator, request));
        }

        private async Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(string countryIsoCode, DateTime holidayDate)
        {
            if (!await HolidaysAlreadyExist(countryIsoCode, holidayDate.Year))
            {
                await CreateHoliday(countryIsoCode, holidayDate.Year);
            }

            return await HolidayRepository.GetHolidaysByDateAsync(countryIsoCode, holidayDate);
        }

        #endregion
    }
}
