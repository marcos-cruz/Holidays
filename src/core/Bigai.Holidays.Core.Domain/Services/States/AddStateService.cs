using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Core.Domain.Validators.States;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.States
{
    /// <summary>
    /// <see cref="AddStateService"/> implements a contract to validate business rules and add <see cref="State"/> to the database.
    /// </summary>
    public class AddStateService : CountryService, IAddStateService
    {
        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddStateService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddStateService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork) : base(notificationHandler, unitOfWork)
        {
            _commandName = "Adicionar estado";
        }

        #endregion

        #region Public Methods

        public CommandResult Add(State state)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(state))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    state = StateRepository.Add(state);
                    commandResult = Commit(_commandName, state.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = state;
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddAsync(State state)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(state))
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
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public CommandResult AddRange(List<State> listOfStates)
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
                    if (!CanAdd(listOfStates))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        StateRepository.AddRange(listOfStates);
                        commandResult = Commit(_commandName, TypeProcess.Register);
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
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<State> listOfStates)
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
                    if (!CanAdd(listOfStates))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        await StateRepository.AddRangeAsync(listOfStates);
                        commandResult = await CommitAsync(_commandName, TypeProcess.Register);
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
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public CommandResult AddRange(List<List<State>> listOfListStates)
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
                    if (!CanAdd(listOfListStates))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListStates.Count; i < j; i++)
                    {
                        var list = listOfListStates[i];

                        StateRepository.AddRange(list);
                        result = Commit(_commandName, TypeProcess.Register);

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
            catch (Exception ex)
            {
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<List<State>> listOfListStates)
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
                    if (!CanAdd(listOfListStates))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListStates.Count; i < j; i++)
                    {
                        var list = listOfListStates[i];

                        await StateRepository.AddRangeAsync(list);
                        result = await CommitAsync(_commandName, TypeProcess.Register);

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
            catch (Exception ex)
            {
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
                commandResult.Data = GetNotifications();
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        #endregion

        #region Private Methods

        private bool CanAdd(State state)
        {
            AddStateValidator validator = new AddStateValidator(CountryRepository, StateRepository);

            return InstanceNotNull(state) && IsValid(validator, state);
        }

        private bool CanAdd(List<State> states)
        {
            AddStateValidator validator = new AddStateValidator(CountryRepository, StateRepository);
            bool instanceNotNull = true;
            bool isValid = true;

            for (int i = 0, j = states.Count; i < j; i++)
            {
                bool result = InstanceNotNull(states[i]);
                if (!result && instanceNotNull)
                {
                    instanceNotNull = result;
                }

                result = IsValid(validator, states[i]);
                if (!result && isValid)
                {
                    isValid = result;
                }
            }

            return instanceNotNull && isValid;
        }

        private bool CanAdd(List<List<State>> listOfListStates)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfListStates.Count; i < j; i++)
            {
                bool result = CanAdd(listOfListStates[i]);
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
