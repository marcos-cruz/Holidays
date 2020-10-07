using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

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
        protected readonly Guid _bigaiId;

        #endregion

        #region Constructor

        protected MainController(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _bigaiId = Guid.Parse("8987EF64-B45A-4545-9D5B-EFE0EDEC6147");
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method determines a standard response for a request that contains errors in the request parameters.
        /// </summary>
        /// <param name="modelState">Model State informed in the request.</param>
        /// <returns>Response to request.</returns>
        protected CommandResponse FormatResponse(ModelStateDictionary modelState)
        {
            NotifyError(modelState);
            CommandResult commandResult = CommandResult.BadRequest("Ação não foi concluída, existem erros.");

            return FormatResponse(commandResult);

        }

        /// <summary>
        /// This method determines the response to the request processed by the business layer of the domain.
        /// </summary>
        /// <param name="commandResult">Result of the action returned by the business layer.</param>
        /// <returns>Response to request.</returns>
        protected CommandResponse FormatResponse(CommandResult commandResult)
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

        /// <summary>
        /// Notifies the occurrence of an error.
        /// </summary>
        /// <param name="propertyName">Who the message refers to.</param>
        /// <param name="errorMessage">Message text.</param>
        protected void NotifyError(string propertyName, string errorMessage)
        {
            _notificationHandler.NotifyError(new DomainNotification(propertyName, errorMessage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected CommandResult UploadCsv()
        {
            CommandResult result = null;
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file.Length == 0 || file.ContentType != "text/csv")
                    {
                        return CommandResult.BadRequest($"{ file.FileName } não é um arquivo válido.");
                    }

                    string foldername = Path.Combine("Resources", "Data");
                    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), foldername);
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(pathToSave, fileName);
                    string dbPath = Path.Combine(foldername, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        result = CommandResult.Ok($"Arquivo {fileName} pronto para ser importado.");
                        result.Data = fullPath;
                    }
                }
                else
                {
                    result = CommandResult.BadRequest("Nenhum arquivo foi informado.");
                }
            }
            catch (Exception ex)
            {
                NotifyError("Upload", ex.Message);
                result = CommandResult.InternalServerError($"Arquivo não é válido.");
            }

            return result;
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
