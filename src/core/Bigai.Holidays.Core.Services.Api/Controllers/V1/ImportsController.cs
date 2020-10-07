using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
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
    [Route("api/v{version:apiVersion}/imports")]
    public class ImportsController : MainController
    {
        #region Private Variables

        private readonly INotificationHandler _notificationHandler;
        private readonly IImportCountryService _importCountryService;
        private readonly IImportStateService _importStateService;
        private readonly IImportRuleHolidayService _importRuleHolidayService;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="ImportsController"/>.
        /// </summary>
        /// <param name="notificationHandler">For handling error notification messages.</param>
        /// <param name="importCountryService">To import countries to database.</param>
        /// <param name="importStateService">To import states to database.</param>
        /// <param name="importRuleHolidayService">To import rules holidays to database.</param>
        public ImportsController(INotificationHandler notificationHandler, IImportCountryService importCountryService, IImportStateService importStateService, IImportRuleHolidayService importRuleHolidayService) : base(notificationHandler)
        {
            _notificationHandler = notificationHandler ?? throw new ArgumentNullException(nameof(notificationHandler));
            _importCountryService = importCountryService ?? throw new ArgumentNullException(nameof(importCountryService));
            _importStateService = importStateService ?? throw new ArgumentNullException(nameof(importStateService));
            _importRuleHolidayService = importRuleHolidayService ?? throw new ArgumentNullException(nameof(importRuleHolidayService));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method uploads a file in CSV format, containing country information, and inserts it in the database.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpPost, DisableRequestSizeLimit]
        [Route("country/{token:guid}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> Country([FromRoute] Guid token)
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
                    commandResult = UploadCsv();

                    if (commandResult.Success)
                    {
                        commandResult = await _importCountryService.ImportAsync(commandResult.Data.ToString());
                    }
                }
                commandResponse = FormatResponse(commandResult);
            }

            watch.Stop();
            commandResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return StatusCode(commandResponse.StatusCode, commandResponse);
        }

        /// <summary>
        /// This method uploads a file in CSV format, containing state information, and inserts it in the database.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpPost, DisableRequestSizeLimit]
        [Route("state/{token:guid}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> State([FromRoute] Guid token)
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
                    commandResult = UploadCsv();

                    if (commandResult.Success)
                    {
                        commandResult = await _importStateService.ImportAsync(commandResult.Data.ToString());
                    }
                }
                commandResponse = FormatResponse(commandResult);
            }

            watch.Stop();
            commandResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return StatusCode(commandResponse.StatusCode, commandResponse);
        }

        /// <summary>
        /// This method uploads a file in CSV format, containing rule holiday information, and inserts it in the database.
        /// </summary>
        /// <param name="token">Authorization key.</param>
        /// <returns>Returns a CommandResponse containing the result of executing the command.</returns>
        [HttpPost, DisableRequestSizeLimit]
        [Route("rule-holiday/{token:guid}")]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CommandResponse), (int)HttpStatusCode.ServiceUnavailable)]
        public async Task<IActionResult> RuleHoliday([FromRoute] Guid token)
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
                    commandResult = UploadCsv();

                    if (commandResult.Success)
                    {
                        commandResult = await _importRuleHolidayService.ImportAsync(commandResult.Data.ToString());
                    }
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
