using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Services.Abstracts;
using Bigai.Holidays.Core.Domain.Validators.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Holidays
{
    /// <summary>
    /// <see cref="AddRuleHolidayService"/> implements a contract to validate business rules and add <see cref="RuleHoliday"/> to the database.
    /// </summary>
    public class AddRuleHolidayService : HolidayBaseService, IAddRuleHolidayService
    {
        #region Private Variables

        private readonly AddRuleHolidayValidator _addRuleHolidayValidator;
        private readonly AddRuleHolidayValidator _addRuleHolidayValidatorRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddRuleHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddRuleHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _commandName = "Adicionar regras de feriados";
            _addRuleHolidayValidator = new AddRuleHolidayValidator();
            _addRuleHolidayValidatorRepository = new AddRuleHolidayValidator(CountryRepository, StateRepository, RuleHolidayRepository);
        }

        #endregion

        #region Public Methods

        public async Task<CommandResult> AddAsync(RuleHoliday ruleHoliday)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!await CanAddAsync(ruleHoliday, true))
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<RuleHoliday> listOfRuleHolidays, bool validateRepository = true)
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
                    if (!await CanAddAsync(listOfRuleHolidays, validateRepository))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        await RuleHolidayRepository.AddRangeAsync(listOfRuleHolidays);
                        commandResult = await CommitAsync(_commandName, ActionType.Register);
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<List<RuleHoliday>> listOfListRuleHolidays, bool validateRepository = true)
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
                    if (!await CanAddAsync(listOfListRuleHolidays, validateRepository))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListRuleHolidays.Count; i < j; i++)
                        {
                            var list = listOfListRuleHolidays[i];

                            await RuleHolidayRepository.AddRangeAsync(list);
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
                            commandResult.Data = listOfListRuleHolidays;
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

        private async Task<bool> CanAddAsync(RuleHoliday ruleHoliday, bool validateRepository)
        {
            var validator = validateRepository == true ? _addRuleHolidayValidatorRepository : _addRuleHolidayValidator;

            return InstanceNotNull(ruleHoliday) && (await IsValidAsync(validator, ruleHoliday));
        }

        private async Task<bool> CanAddAsync(List<RuleHoliday> rulesHolidays, bool validateRepository)
        {
            bool canAdd = true;

            for (int i = 0, j = rulesHolidays.Count; i < j; i++)
            {
                bool result = await CanAddAsync(rulesHolidays[i], validateRepository);
                if (!result && canAdd)
                {
                    canAdd = result;
                }
            }

            return canAdd;
        }

        private async Task<bool> CanAddAsync(List<List<RuleHoliday>> listOfListRulesHolidays, bool validateRepository)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfListRulesHolidays.Count; i < j; i++)
            {
                bool result = await CanAddAsync(listOfListRulesHolidays[i], validateRepository);
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
