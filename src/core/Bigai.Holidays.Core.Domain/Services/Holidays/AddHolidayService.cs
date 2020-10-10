using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Mappers.Holidays;
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
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Holidays
{
    /// <summary>
    /// <see cref="AddHolidayService"/> implements a contract to validate business rules and add <see cref="Holiday"/> to the database.
    /// </summary>
    public class AddHolidayService : HolidayBaseService, IAddHolidayService
    {
        #region Private Variables

        private readonly AddHolidayValidator _addHolidayValidator;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _commandName = "Adicionar feriados";
            _addHolidayValidator = new AddHolidayValidator(CountryRepository, StateRepository, HolidayRepository);
        }

        #endregion

        #region Public Methods

        public CommandResult Add(string countryIsoCode, int year)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                var rulesHolidays = RuleHolidayRepository.Find(r => r.CountryIsoCode == countryIsoCode).ToList();
                if (rulesHolidays == null || rulesHolidays.Count() == 0)
                {
                    commandResult = CommandResult.BadRequest($"Não existe feriados cadastrados para {countryIsoCode}.");
                }
                else
                {
                    var holidays = rulesHolidays.ToHolidayList(GetNotificationHandler(), year, GetUserLogged());
                    commandResult = AddRange(holidays, false);
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

        public async Task<CommandResult> AddAsync(string countryIsoCode, int year)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                var rulesHolidays = (await RuleHolidayRepository.FindAsync(r => r.CountryIsoCode == countryIsoCode)).ToList();
                if (rulesHolidays == null || rulesHolidays.Count() == 0)
                {
                    commandResult = CommandResult.BadRequest($"Não existe feriados cadastrados para {countryIsoCode}.");
                }
                else
                {
                    var holidays = await Task.Run(() => rulesHolidays.ToHolidayList(GetNotificationHandler(), year, GetUserLogged()));
                    commandResult = AddRange(holidays, false);
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
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
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        HolidayRepository.AddRange(listOfHolidays);
                        commandResult = Commit(_commandName, ActionType.Register);
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
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
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        await HolidayRepository.AddRangeAsync(listOfHolidays);
                        commandResult = await CommitAsync(_commandName, ActionType.Register);
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
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public CommandResult AddRange(List<List<Holiday>> listOfListHolidays, bool validate = true)
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
                    if (validate && !CanAdd(listOfListHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListHolidays.Count; i < j; i++)
                        {
                            var list = listOfListHolidays[i];

                            HolidayRepository.AddRange(list);
                            result = Commit(_commandName, ActionType.Register);

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
            }
            catch (Exception ex)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> AddRangeAsync(List<List<Holiday>> listOfListHolidays, bool validate = true)
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
                    if (validate && !CanAdd(listOfListHolidays))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListHolidays.Count; i < j; i++)
                        {
                            var list = listOfListHolidays[i];

                            await HolidayRepository.AddRangeAsync(list);
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
                            commandResult.Data = listOfListHolidays;
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

        private bool CanAdd(Holiday holiday)
        {
            return InstanceNotNull(holiday) && IsValid(_addHolidayValidator, holiday);
        }

        private bool CanAdd(List<Holiday> holidays)
        {
            bool canAdd = true;

            for (int i = 0, j = holidays.Count; i < j; i++)
            {
                bool result = CanAdd(holidays[i]);
                if (!result && canAdd)
                {
                    canAdd = result;
                }
            }

            return canAdd;
        }

        private bool CanAdd(List<List<Holiday>> listOfListHolidays)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfListHolidays.Count; i < j; i++)
            {
                bool result = CanAdd(listOfListHolidays[i]);
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
