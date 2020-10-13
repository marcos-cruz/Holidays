using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Domain.Notifications;
using Bigai.Holidays.Shared.Domain.Requests;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Bigai.Holidays.Shared.Domain.Services
{
    /// <summary>
    /// <see cref="DomainService"/> implements a contract to perform domain operations and business rules.
    /// </summary>
    public abstract class DomainService : IDomainService
    {
        #region Private Variables

        private readonly INotificationHandler _notificationHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserLogged _userLogged;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="DomainService"/>.
        /// </summary>
        /// <param name="notificationHandler">Error message handler.</param>
        /// <param name="unitOfWork">Context to commit changes.</param>
        /// <param name="userLogged">The who is logged in.</param>
        protected DomainService(INotificationHandler notificationHandler, IUnitOfWork unitOfWork, IUserLogged userLogged)
        {
            _notificationHandler = notificationHandler ?? throw new ArgumentNullException(nameof(notificationHandler));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userLogged = userLogged ?? throw new ArgumentNullException(nameof(userLogged));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the instance of the notification handler.
        /// </summary>
        /// <returns>The instance of the notification handler.</returns>
        protected INotificationHandler GetNotificationHandler()
        {
            return _notificationHandler;
        }

        /// <summary>
        /// Determines the user who is logged in.
        /// </summary>
        /// <returns>The user who is logged in.</returns>
        protected IUserLogged GetUserLogged()
        {
            return _userLogged;
        }

        /// <summary>
        /// Save all change in this context to database.
        /// </summary>
        /// <param name="commandName">Process name for notification in case of error.</param>
        /// <param name="typeProcess">Process type for creating the response code.</param>
        /// <returns>Result of the action with the http status code..</returns>
        protected async Task<CommandResult> CommitAsync(string commandName, ActionType typeProcess)
        {
            CommandResult commandResult;

            if (HasNotification())
            {
                commandResult = CommandResult.BadRequest("Ação não foi concluída, existem erros.");
            }
            else
            {
                try
                {
                    if (await _unitOfWork.CommitAsync())
                    {
                        commandResult = CommandResult.Ok("Ação concluída com sucesso");
                        if (typeProcess == ActionType.Register)
                        {
                            commandResult = CommandResult.Created("Ação concluída com sucesso");
                        }
                    }
                    else
                    {
                        commandResult = CommandResult.ServiceUnavailable($"Serviço indisponível, tente novamente mais tarde.");
                    }
                }
                catch (Exception ex)
                {
                    NotifyError(commandName, ex.Message);
                    commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                }
            }

            return commandResult;
        }

        /// <summary>
        /// Determines whether there is an error.
        /// </summary>
        /// <returns><c>true</c> if there is an error, otherwise <c>false</c>.</returns>
        protected bool HasNotification()
        {
            return _notificationHandler.HasNotification();
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
        /// Notifies the occurrence of an error.
        /// </summary>
        /// <param name="validationResult">A collection of errors.</param>
        protected void NotifyError(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                NotifyError(error.PropertyName, error.ErrorMessage);
            }
        }

        /// <summary>
        /// Determines whether an object's instance is null.
        /// </summary>
        /// <param name="entity">Instance to be validated.</param>
        /// <returns><c>true</c> if instance is <c>null</c>, otherwise false.</returns>
        protected bool InstanceNotNull(object entity)
        {
            if (entity == null)
            {
                NotifyError(entity.GetType().Name, $"{ entity.GetType().Name } parâmetro está nulo.");
            }
            return entity != null;
        }

        /// <summary>
        /// Determines whether an object's instance is valid.
        /// </summary>
        /// <typeparam name="TValidator">Validator type.</typeparam>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="validator">Validator with business rules to be tested at the entity.</param>
        /// <param name="entity">Instance of entity to be validated.</param>
        /// <returns><c>true</c> if the entity is valid, otherwise <c>false</c>.</returns>
        protected async Task<bool> IsValidEntityAsync<TValidator, TEntity>(TValidator validator, TEntity entity) where TValidator : AbstractValidator<TEntity> where TEntity : Entity
        {
            ValidationResult validationResult = await validator.ValidateAsync(entity);

            if (!validationResult.IsValid)
            {
                NotifyError(validationResult);
            }

            return validationResult.IsValid;
        }

        /// <summary>
        /// Determines whether an object's instance is valid.
        /// </summary>
        /// <typeparam name="TValidator">Validator type.</typeparam>
        /// <typeparam name="TRequest">Request type.</typeparam>
        /// <param name="validator">Validator with business rules to be tested at the request.</param>
        /// <param name="request">Instance of request to be validated.</param>
        /// <returns><c>true</c> if the request is valid, otherwise <c>false</c>.</returns>
        protected async Task<bool> IsValidRequestAsync<TValidator, TRequest>(TValidator validator, TRequest request) where TValidator : AbstractValidator<TRequest> where TRequest : Request
        {
            ValidationResult validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                NotifyError(validationResult);
            }

            return validationResult.IsValid;
        }

        /// <summary>
        /// This method reads a CSV file and returns its contents in an array of rows and columns.
        /// </summary>
        /// <param name="fileName">Path containing the name of the CSV file to read.</param>
        /// <returns>Returns a multidimensional array with the rows and columns that were read from the CSV file. If you are unable to read the file or if it is empty, then return null.</returns>
        protected async Task<string[,]> ImportCsvFileAsync(string fileName)
        {
            return await CsvHelper.LoadCsvAsync(fileName);
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns><c>true</c> if the caller has the required permissions and path contains the name of an existing file, otherwise <c>false</c>.</returns>
        protected bool FileExist(string filename)
        {
            if (!filename.HasValue())
            {
                return false;
            }

            return File.Exists(filename);
        }

        #endregion
    }
}
