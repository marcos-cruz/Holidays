using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
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
    [Produces("application/json")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/holidays")]
    public class HolidaysController : MainController
    {
        #region Private Variables

        private readonly IQueryHolidayService _queryHolidayService;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="HolidaysController"/>.
        /// </summary>
        /// <param name="notificationHandler">For handling error notification messages.</param>
        /// <param name="queryHolidayService">To query holidays in database.</param>
        public HolidaysController(INotificationHandler notificationHandler,
                                  IQueryHolidayService queryHolidayService) : base(notificationHandler)
        {
            _queryHolidayService = queryHolidayService ?? throw new ArgumentNullException(nameof(queryHolidayService));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Obtains a list of holidays by country code for a specific year.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <param name="countryIsoCode">Country code consisting of 3 letters, according to ISO-3166.</param>
        /// <param name="year">Year to query holidays.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpPost, DisableRequestSizeLimit]
        [Route("{token:guid}/countryIsoCode/year")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> GetByCountry([FromRoute] Guid token, string countryIsoCode, int year)
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
                    commandResult = await _queryHolidayService.GetHolidaysAsync(countryIsoCode, year);
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
