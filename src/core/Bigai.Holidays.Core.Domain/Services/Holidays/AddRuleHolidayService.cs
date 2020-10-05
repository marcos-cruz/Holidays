using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Core.Domain.Validators.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Holidays
{
    /// <summary>
    /// <see cref="AddRuleHolidayService"/> implements a contract to validate business rules and add <see cref="RuleHoliday"/> to the database.
    /// </summary>
    public class AddRuleHolidayService : CountryService, IAddRuleHolidayService
    {
        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddRuleHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddRuleHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork) : base(notificationHandler, unitOfWork)
        {
            _commandName = "Adicionar regras de feriados";
        }

        #endregion

        #region Public Methods

        public CommandResult Add(RuleHoliday ruleHoliday)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(ruleHoliday))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    ruleHoliday = RuleHolidayRepository.Add(ruleHoliday);
                    commandResult = Commit(_commandName, ruleHoliday.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = ruleHoliday;
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

        public async Task<CommandResult> AddAsync(RuleHoliday ruleHoliday)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(ruleHoliday))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    ruleHoliday = await RuleHolidayRepository.AddAsync(ruleHoliday);
                    commandResult = await CommitAsync(_commandName, ruleHoliday.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = ruleHoliday;
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

        public CommandResult AddRange(List<RuleHoliday> listOfRuleHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfRuleHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfRuleHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        RuleHolidayRepository.AddRange(listOfRuleHolidays);
                        commandResult = Commit(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfRuleHolidays;
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

        public async Task<CommandResult> AddRangeAsync(List<RuleHoliday> listOfRuleHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfRuleHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfRuleHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        await RuleHolidayRepository.AddRangeAsync(listOfRuleHolidays);
                        commandResult = await CommitAsync(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfRuleHolidays;
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

        public CommandResult AddRange(List<List<RuleHoliday>> listOfListRuleHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListRuleHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListRuleHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListRuleHolidays.Count; i < j; i++)
                    {
                        var list = listOfListRuleHolidays[i];

                        RuleHolidayRepository.AddRange(list);
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
                        commandResult.Data = listOfListRuleHolidays;
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

        public async Task<CommandResult> AddRangeAsync(List<List<RuleHoliday>> listOfListRuleHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListRuleHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListRuleHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListRuleHolidays.Count; i < j; i++)
                    {
                        var list = listOfListRuleHolidays[i];

                        await RuleHolidayRepository.AddRangeAsync(list);
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
                        commandResult.Data = listOfListRuleHolidays;
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

        private bool CanAdd(RuleHoliday ruleHoliday)
        {
            AddRuleHolidayValidator validator = new AddRuleHolidayValidator(CountryRepository, StateRepository, RuleHolidayRepository);

            return InstanceNotNull(ruleHoliday) && IsValid(validator, ruleHoliday);
        }

        private bool CanAdd(List<RuleHoliday> rulesHolidays)
        {
            AddRuleHolidayValidator validator = new AddRuleHolidayValidator(CountryRepository, StateRepository, RuleHolidayRepository);
            bool instanceNotNull = true;
            bool isValid = true;

            for (int i = 0, j = rulesHolidays.Count; i < j; i++)
            {
                bool result = InstanceNotNull(rulesHolidays[i]);
                if (!result && instanceNotNull)
                {
                    instanceNotNull = result;
                }

                result = IsValid(validator, rulesHolidays[i]);
                if (!result && isValid)
                {
                    isValid = result;
                }
            }

            return instanceNotNull && isValid;
        }

        private bool CanAdd(List<List<RuleHoliday>> listOfListRulesHolidays)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfListRulesHolidays.Count; i < j; i++)
            {
                bool result = CanAdd(listOfListRulesHolidays[i]);
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
