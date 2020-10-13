using Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays;
using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Core.Services.Api.Controllers.Abstracts;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Services.Api.Controllers.V1
{
    /// <summary>
    /// Allows you to perform operations with holidays.
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/holidays")]
    public class HolidaysController : MainController
    {
        #region Private Variables

        private readonly IQueryHolidaysByCountry _queryHolidaysByCountry;
        private readonly IQueryHolidaysByMonth _queryHolidaysByMonth;
        private readonly IQueryHolidaysByState _queryHolidaysByState;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="HolidaysController"/>.
        /// </summary>
        /// <param name="notificationHandler">For handling error notification messages.</param>
        /// <param name="queryHolidaysByCountry">To query holidays in database.</param>
        /// <param name="queryHolidaysByMonth">To query holidays in database.</param>
        /// <param name="queryHolidaysByState">To query holidays in database.</param>
        public HolidaysController(INotificationHandler notificationHandler,
                                  IQueryHolidaysByCountry queryHolidaysByCountry,
                                  IQueryHolidaysByMonth queryHolidaysByMonth,
                                  IQueryHolidaysByState queryHolidaysByState) : base(notificationHandler)
        {
            _queryHolidaysByCountry = queryHolidaysByCountry ?? throw new ArgumentNullException(nameof(queryHolidaysByCountry));
            _queryHolidaysByMonth = queryHolidaysByMonth ?? throw new ArgumentNullException(nameof(queryHolidaysByMonth));
            _queryHolidaysByState = queryHolidaysByState ?? throw new ArgumentNullException(nameof(queryHolidaysByState));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a list of all holidays for a specific country and year.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <param name="countryIsoCode">Country code according to ISO-3166.</param>
        /// <param name="year">Year to query holidays.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpGet]
        [Route("{token:guid}/{countryIsoCode}/{year:int}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> GetAllHolidaysByCountry([FromRoute] Guid token, string countryIsoCode, int year)
        {
            CommandResponse commandResponse;
            Stopwatch watch = Stopwatch.StartNew();

            if (!ModelState.IsValid)
            {
                commandResponse = FormatResponse(ModelState);
            }
            else
            {
                CommandResult commandResult;

                if (token != _bigaiId)
                {
                    commandResult = CommandResult.Unauthorized("Authorization token is not valid.");
                }
                else
                {
                    var request = new GetHolidaysByCountryRequest()
                    {
                        CountryIsoCode = countryIsoCode,
                        Year = year
                    };
                    commandResult = await _queryHolidaysByCountry.GetHolidaysByCountryAsync(request);
                }
                commandResponse = FormatResponse(commandResult);
            }

            watch.Stop();
            commandResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return StatusCode(commandResponse.StatusCode, commandResponse);
        }

        /// <summary>
        /// Gets a list of all national and state holidays for a specific state and year.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <param name="countryIsoCode">Country code according to ISO-3166.</param>
        /// <param name="stateIsoCode">State code according to ISO-3166.</param>
        /// <param name="year">Year to query holidays.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpGet]
        [Route("{token:guid}/{countryIsoCode}/{stateIsoCode}/{year:int}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> GetAllHolidaysByState([FromRoute] Guid token, string countryIsoCode, string stateIsoCode, int year)
        {
            CommandResponse commandResponse;
            Stopwatch watch = Stopwatch.StartNew();

            if (!ModelState.IsValid)
            {
                commandResponse = FormatResponse(ModelState);
            }
            else
            {
                CommandResult commandResult;

                if (token != _bigaiId)
                {
                    commandResult = CommandResult.Unauthorized("Authorization token is not valid.");
                }
                else
                {
                    var request = new GetHolidaysByStateRequest()
                    {
                        StateIsoCode = stateIsoCode,
                        CountryIsoCode = countryIsoCode,
                        Year = year
                    };
                    commandResult = await _queryHolidaysByState.GetHolidaysByStateAsync(request);
                }
                commandResponse = FormatResponse(commandResult);
            }

            watch.Stop();
            commandResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return StatusCode(commandResponse.StatusCode, commandResponse);
        }

        /// <summary>
        /// Get a list of all holidays of the month for a specific country.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <param name="countryIsoCode">Country code according to ISO-3166.</param>
        /// <param name="year">Year to query holidays.</param>
        /// <param name="month">Month to query holidays.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpGet]
        [Route("{token:guid}/{countryIsoCode}/{year:int}/{month:int}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> GetAllHolidaysByMonth([FromRoute] Guid token, string countryIsoCode, int year, int month)
        {
            CommandResponse commandResponse;
            Stopwatch watch = Stopwatch.StartNew();

            if (!ModelState.IsValid)
            {
                commandResponse = FormatResponse(ModelState);
            }
            else
            {
                CommandResult commandResult;

                if (token != _bigaiId)
                {
                    commandResult = CommandResult.Unauthorized("Authorization token is not valid.");
                }
                else
                {
                    var request = new GetHolidaysByMonthRequest()
                    {
                        CountryIsoCode = countryIsoCode,
                        Year = year,
                        Month = month
                    };
                    commandResult = await _queryHolidaysByMonth.GetHolidaysByMonthAsync(request);
                }
                commandResponse = FormatResponse(commandResult);
            }

            watch.Stop();
            commandResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return StatusCode(commandResponse.StatusCode, commandResponse);
        }

        #endregion
    }
}
