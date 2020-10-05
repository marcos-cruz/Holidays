﻿using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
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
    /// <see cref="AddHolidayService"/> implements a contract to validate business rules and add <see cref="Holiday"/> to the database.
    /// </summary>
    public class AddHolidayService : CountryService, IAddHolidayService
    {
        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork) : base(notificationHandler, unitOfWork)
        {
            _commandName = "Adicionar feriados";
        }

        #endregion

        #region Public Methods

        public CommandResult Add(Holiday holiday)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(holiday))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    holiday = HolidayRepository.Add(holiday);
                    commandResult = Commit(_commandName, holiday.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = holiday;
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

        public async Task<CommandResult> AddAsync(Holiday holiday)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(holiday))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    holiday = await HolidayRepository.AddAsync(holiday);
                    commandResult = await CommitAsync(_commandName, holiday.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = holiday;
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

        public CommandResult AddRange(List<Holiday> listOfHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        HolidayRepository.AddRange(listOfHolidays);
                        commandResult = Commit(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfHolidays;
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

        public async Task<CommandResult> AddRangeAsync(List<Holiday> listOfHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }
                    else
                    {
                        await HolidayRepository.AddRangeAsync(listOfHolidays);
                        commandResult = await CommitAsync(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfHolidays;
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

        public CommandResult AddRange(List<List<Holiday>> listOfListHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListHolidays.Count; i < j; i++)
                    {
                        var list = listOfListHolidays[i];

                        HolidayRepository.AddRange(list);
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
                        commandResult.Data = listOfListHolidays;
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

        public async Task<CommandResult> AddRangeAsync(List<List<Holiday>> listOfListHolidays)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListHolidays);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Lista não pode ser salva, existem erros.");
                    }

                    int recordsSaved = 0;
                    CommandResult result = CommandResult.Ok("");

                    for (int i = 0, j = listOfListHolidays.Count; i < j; i++)
                    {
                        var list = listOfListHolidays[i];

                        await HolidayRepository.AddRangeAsync(list);
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
                        commandResult.Data = listOfListHolidays;
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

        private bool CanAdd(Holiday holiday)
        {
            AddHolidayValidator validator = new AddHolidayValidator(CountryRepository, StateRepository, HolidayRepository);

            return InstanceNotNull(holiday) && IsValid(validator, holiday);
        }

        private bool CanAdd(List<Holiday> rulesHolidays)
        {
            AddHolidayValidator validator = new AddHolidayValidator(CountryRepository, StateRepository, HolidayRepository);
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

        private bool CanAdd(List<List<Holiday>> listOfListRulesHolidays)
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
