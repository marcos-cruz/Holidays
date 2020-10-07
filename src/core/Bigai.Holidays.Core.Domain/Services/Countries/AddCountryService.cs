using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Validators.Countries;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Countries
{
    /// <summary>
    /// <see cref="AddCountryService"/> implements a contract to validate business rules and add <see cref="Country"/> to the database.
    /// </summary>
    public class AddCountryService : CountryService, IAddCountryService
    {
        #region Private Variables

        private readonly AddCountryValidator _addCountryValidator;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddCountryService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        public AddCountryService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _commandName = "Adicionar país";
            _addCountryValidator = new AddCountryValidator(CountryRepository);
        }

        #endregion

        #region Public Methods

        public CommandResult Add(Country country)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(country))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    country = CountryRepository.Add(country);
                    commandResult = Commit(_commandName, country.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = country;
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

        public async Task<CommandResult> AddAsync(Country country)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                if (!CanAdd(country))
                {
                    commandResult = CommandResult.BadRequest("Registro não pode ser salvo, existem erros.");
                }
                else
                {
                    country = await CountryRepository.AddAsync(country);
                    commandResult = await CommitAsync(_commandName, country.Action);
                    if (commandResult.Success)
                    {
                        commandResult.Data = country;
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

        public CommandResult AddRange(List<Country> listOfCountries)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfCountries);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfCountries))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        CountryRepository.AddRange(listOfCountries);
                        commandResult = Commit(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfCountries;
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

        public async Task<CommandResult> AddRangeAsync(List<Country> listOfCountries)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfCountries);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfCountries))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        await CountryRepository.AddRangeAsync(listOfCountries);
                        commandResult = await CommitAsync(_commandName, TypeProcess.Register);
                        if (commandResult.Success)
                        {
                            commandResult.Message = $"Ação concluída com sucesso. Salvos { recordsToSave } registros de um total de { recordsToSave }";
                            commandResult.Data = listOfCountries;
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

        public CommandResult AddRange(List<List<Country>> listOfListCountries)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListCountries);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListCountries))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListCountries.Count; i < j; i++)
                        {
                            var list = listOfListCountries[i];

                            CountryRepository.AddRange(list);
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
                            commandResult.Data = listOfListCountries;
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

        public async Task<CommandResult> AddRangeAsync(List<List<Country>> listOfListCountries)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                int recordsToSave = Count(listOfListCountries);
                if (recordsToSave == 0)
                {
                    commandResult = CommandResult.BadRequest("Nenhum registro salvo, a lista está vazia.");
                }
                else
                {
                    if (!CanAdd(listOfListCountries))
                    {
                        commandResult = CommandResult.BadRequest("Nenhum registro salvo, existem erros.");
                    }
                    else
                    {
                        int recordsSaved = 0;
                        CommandResult result = CommandResult.Ok("");

                        for (int i = 0, j = listOfListCountries.Count; i < j; i++)
                        {
                            var list = listOfListCountries[i];

                            await CountryRepository.AddRangeAsync(list);
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
                            commandResult.Data = listOfListCountries;
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

        private bool CanAdd(Country country)
        {
            return InstanceNotNull(country) && IsValid(_addCountryValidator, country);
        }

        private bool CanAdd(List<Country> countries)
        {
            bool canAdd = true;

            for (int i = 0, j = countries.Count; i < j; i++)
            {
                bool result = CanAdd(countries[i]);
                if (!result && canAdd)
                {
                    canAdd = result;
                }
            }

            return canAdd;
        }

        private bool CanAdd(List<List<Country>> listOfCountries)
        {
            bool canAdd = true;

            for (int i = 0, j = listOfCountries.Count; i < j; i++)
            {
                bool result = CanAdd(listOfCountries[i]);
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
