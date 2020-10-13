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
    public class QueryHolidaysByCountry : QueryBaseService, IQueryHolidaysByCountry
    {
        #region Private Variables

        private readonly GetHolidaysByCountryRequestValidator _getHolidaysByCountryRequestValidator;

        #endregion

        #region Constructor

        public QueryHolidaysByCountry(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddHolidayService addHolidayService) : base(notificationHandler, unitOfWork, userLogged, addHolidayService)
        {
            _getHolidaysByCountryRequestValidator = new GetHolidaysByCountryRequestValidator(unitOfWork.CountryRepository, unitOfWork.StateRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> GetHolidaysByCountryAsync(GetHolidaysByCountryRequest request)
        {
            _commandName = request.GetType().Name;
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanGetHolidaysByCountryAsync(request))
                {
                    commandResult = CommandResult.BadRequest("Não foi possível realizar a consulta.");
                }
                else
                {
                    var holidays = await GetHolidaysByCountryAsync(request.CountryIsoCode, request.Year);
                    if (holidays != null && holidays.Count() > 0)
                    {
                        commandResult = CommandResult.Ok($"{holidays.Count()} feriados encontrados.");
                        commandResult.Data = holidays.ToResponse();
                    }
                    else
                    {
                        commandResult = CommandResult.BadRequest($"Não existe feriado cadastrado para {request.CountryIsoCode} em {request.Year}.");
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

        private async Task<bool> CanGetHolidaysByCountryAsync(GetHolidaysByCountryRequest request)
        {
            return InstanceNotNull(request) && (await IsValidRequestAsync(_getHolidaysByCountryRequestValidator, request));
        }

        private async Task<IEnumerable<Holiday>> GetHolidaysByCountryAsync(string countryIsoCode, int year)
        {
            if (!await HolidaysAlreadyExist(countryIsoCode, year))
            {
                await CreateHoliday(countryIsoCode, year);
            }

            return await HolidayRepository.GetHolidaysByCountryAsync(countryIsoCode, year);
        }

        #endregion
    }
}
