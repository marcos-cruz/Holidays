using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Bigai.Holidays.Core.Services.Api.Controllers.Abstracts
{
    /// <summary>
    /// <see cref="MainController"/> provides support for controllers.
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        #region Private Variables

        private readonly INotificationHandler _notificationHandler;

        #endregion

        #region Constructor

        protected MainController(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method determines a standard response for a request that contains errors in the request parameters.
        /// </summary>
        /// <param name="modelState">Model State informed in the request.</param>
        /// <returns>Response to request.</returns>
        public CommandResponse GetResponse(ModelStateDictionary modelState)
        {
            NotifyError(modelState);
            CommandResult commandResult = CommandResult.BadRequest("Ação não foi concluída, existem erros.");

            return GetResponse(commandResult);

        }

        /// <summary>
        /// This method determines the response to the request processed by the business layer of the domain.
        /// </summary>
        /// <param name="commandResult">Result of the action returned by the business layer.</param>
        /// <returns>Response to request.</returns>
        public CommandResponse GetResponse(CommandResult commandResult)
        {
            CommandResponse commandResponse = new CommandResponse()
            {
                Success = commandResult.Success,
                Message = commandResult.Message,
                StatusCode = commandResult.StatusCode,
                ElapsedTime = commandResult.ElapsedTime,
                Data = commandResult.Data,
            };

            if (!commandResult.Success || _notificationHandler.HasNotification())
            {
                commandResponse.Errors = _notificationHandler.GetNotifications().Select(e => e.Notification).ToList();
            }

            return commandResponse;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Notifies the occurrence of an error.
        /// </summary>
        /// <param name="modelState">A collection of errors.</param>
        private void NotifyError(ModelStateDictionary modelState)
        {
            List<ModelError> errors = modelState.Values.SelectMany(e => e.Errors).ToList();
            List<string> keys = modelState.Keys.ToList();

            if (errors.Count == keys.Count)
            {
                for (int i = 0, j = errors.Count(); i < j; i++)
                {
                    string key = keys[i];
                    string value = errors[i].Exception == null ? errors[i].ErrorMessage : errors[i].Exception.Message;

                    _notificationHandler.NotifyError(new DomainNotification(key, value));
                }
            }
            else
            {
                for (int i = 0, j = errors.Count(); i < j; i++)
                {
                    string value = errors[i].Exception == null ? errors[i].ErrorMessage : errors[i].Exception.Message;

                    _notificationHandler.NotifyError(new DomainNotification(i.ToString(), value));
                }
            }

        }

        #endregion
    }
}
