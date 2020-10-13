using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Services.Abstracts;
using Bigai.Holidays.Core.Domain.Validators.States;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.States
{
    /// <summary>
    /// <see cref="AddStateService"/> implements a contract to validate business rules and add <see cref="State"/> to the database.
    /// </summary>
    public class AddStateService : HolidayBaseService, IAddStateService
    {
        #region Private Variables

        private readonly AddStateValidator _addStateValidator;
        private readonly AddStateValidator _addStateValidatorRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddStateService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddStateService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _commandName = "Adicionar estado";
            _addStateValidator = new AddStateValidator();
            _addStateValidatorRepository = new AddStateValidator(CountryRepository, StateRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> AddAsync(State state)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanAddAsync(state, true))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    state = await StateRepository.AddAsync(state);
                    commandResult = await CommitAsync(_commandName, state.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = state;
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<State> listOfStates, bool validateRepository = true)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfStates);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!await CanAddAsync(listOfStates, validateRepository))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        await StateRepository.AddRangeAsync(listOfStates);
                        commandResult = await CommitAsync(_commandName, ActionType.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfStates;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<List<State>> listOfListStates, bool validateRepository = true)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListStates);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!await CanAddAsync(listOfListStates, validateRepository))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListStates.Count; i < j; i++)
                        {
                            var list = listOfListStates[i];

                            await StateRepository.AddRangeAsync(list);
                            result = await CommitAsync(_commandName, ActionType.Register);

                            if (result.Success)
                            {
                                recordsSaved += list.Count;
                            }
                            else
                            {
                                i = j;
                            }
                        }

                        commandResult = result;
                        if (commandResult.Success && recordsSaved == recordsToSave)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsSaved } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfListStates;
                        }
                        else if (!commandResult.Success)
                        {
                            commandResult.Message = $"Ação não foi concluída. Salvos { recordsSaved } registros de um total de { recordsToSave }";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        #endregion

        #region Private Methods

        private async Task<bool> CanAddAsync(State state, bool validateRepository)
        {
            var validator = validateRepository == true ? _addStateValidatorRepository : _addStateValidator;

            return InstanceNotNull(state) && (await IsValidEntityAsync(validator, state));
        }

        private async Task<bool> CanAddAsync(List<State> states, bool validateRepository)
        {
            bool canAdd = true;

            for (int i = 0, j = states.Count; i < j; i++)
            {
                bool result = await CanAddAsync(states[i], validateRepository);
                if (!result && canAdd)
                {
                    canAdd = result;
                }
            }

            return canAdd;
        }

        private async Task<bool> CanAddAsync(List<List<State>> listOfListStates, bool validateRepository)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfListStates.Count; i < j; i++)
            {
                bool result = await CanAddAsync(listOfListStates[i], validateRepository);
                if (!result && canAdd)
                {
                    canAdd = result;
                }
            }

            return canAdd;
        }

        #endregion
    }
}
