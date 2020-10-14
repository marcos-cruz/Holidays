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
    public class QueryHolidaysByState : QueryBaseService, IQueryHolidaysByState
    {
        #region Private Variables

        private readonly GetHolidaysByStateRequestValidator _getHolidaysByStateRequestValidator;

        #endregion

        #region Constructor

        public QueryHolidaysByState(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddHolidayService addHolidayService) : base(notificationHandler, unitOfWork, userLogged, addHolidayService)
        {
            _getHolidaysByStateRequestValidator = new GetHolidaysByStateRequestValidator(unitOfWork.CountryRepository, unitOfWork.StateRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> GetHolidaysByStateAsync(GetHolidaysByStateRequest request)
        {
            _commandName = request.GetType().Name;
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanGetHolidaysByStateAsync(request))
                {
                    commandResult = CommandResult.BadRequest("Não foi possível realizar a consulta.");
                }
                else
                {
                    var holidays = await GetHolidaysByStateAsync(request.CountryIsoCode, request.StateIsoCode, request.Year);
                    if (holidays != null && holidays.Count() > 0)
                    {
                        commandResult = CommandResult.Ok($"{holidays.Count()} feriados encontrados.");
                        commandResult.Data = holidays.ToResponse(request.StateIsoCode);
                    }
                    else
                    {
                        commandResult = CommandResult.BadRequest($"Não existe feriado cadastrado para {request.CountryIsoCode}/{request.StateIsoCode} em {request.Year}.");
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

        private async Task<bool> CanGetHolidaysByStateAsync(GetHolidaysByStateRequest request)
        {
            return InstanceNotNull(request) && (await IsValidRequestAsync(_getHolidaysByStateRequestValidator, request));
        }

        private async Task<IEnumerable<Holiday>> GetHolidaysByStateAsync(string countryIsoCode, string stateIsoCode, int year)
        {
            if (!await HolidaysAlreadyExist(countryIsoCode, year))
            {
                await CreateHoliday(countryIsoCode, year);
            }

            return await HolidayRepository.GetHolidaysByStateAsync(countryIsoCode, stateIsoCode, year);
        }

        #endregion
    }
}
