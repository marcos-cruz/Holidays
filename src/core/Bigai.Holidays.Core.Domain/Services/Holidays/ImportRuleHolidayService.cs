using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Mappers.Holidays;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Holidays
{
    /// <summary>
    /// <see cref="ImportRuleHolidayService"/> implements a contract to perform domain operations with CSV files for <see cref="RuleHoliday"/>.
    /// </summary>
    public class ImportRuleHolidayService : CountryService, IImportRuleHolidayService
    {
        #region Private Variables

        private string _file = "";
        private const int _columns = 14;
        private readonly IAddRuleHolidayService _addRuleHolidayService;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="ImportRuleHolidayService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        /// <param name="addRuleHolidayService">Service do add rules holidays in database.</param>
        public ImportRuleHolidayService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IAddRuleHolidayService addRuleHolidayService) : base(notificationHandler, unitOfWork)
        {
            _addRuleHolidayService = addRuleHolidayService ?? throw new ArgumentNullException(nameof(addRuleHolidayService));
        }

        #endregion

        #region Public Methods

        public CommandResult Import(string filename)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();
            _file = filename.GetFileNameFromPath();

            try
            {
                if (!FileExist(filename))
                {
                    NotifyError(_file, $"{filename} não existe.");
                    commandResult = CommandResult.BadRequest($"{ _file } não foi localizado.");
                }
                else
                {
                    string[,] content = ImportCsvFile(filename);

                    if (content == null)
                    {
                        NotifyError(_file, $"{filename} está vazio.");
                        commandResult = CommandResult.BadRequest($"{ _file } não tem registros.");
                    }
                    else
                    {
                        int items = content.GetLength(0);
                        int columns = content.GetLength(1);

                        if (items == 0 || columns != _columns)
                        {
                            NotifyError(_file, $"Formato inválido. Forneça um arquivo no formato CSV com { _columns } colunas.");
                            commandResult = CommandResult.BadRequest($"Importação do arquivo { _file } não foi concluída, existem erros.");
                        }
                        else
                        {
                            List<List<RuleHoliday>> list = content.ToListOfRuleHolidayList();
                            commandResult = _addRuleHolidayService.AddRange(list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            if (!commandResult.Success)
            {
                commandResult.Data = GetNotifications();
            }
            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        public async Task<CommandResult> ImportAsync(string filename)
        {
            CommandResult commandResult;
            Stopwatch watch = Stopwatch.StartNew();
            _file = filename.GetFileNameFromPath();

            try
            {
                if (!FileExist(filename))
                {
                    NotifyError(_file, $"{filename} arquivo não existe.");
                    commandResult = CommandResult.BadRequest($"{ _file } não foi localizado.");
                }
                else
                {
                    string[,] content = await ImportCsvFileAsync(filename);

                    if (content == null)
                    {
                        NotifyError(_file, $"{filename} está vazio.");
                        commandResult = CommandResult.BadRequest($"{ _file } não tem registros.");
                    }
                    else
                    {
                        int items = content.GetLength(0);
                        int columns = content.GetLength(1);

                        if (items == 0 || columns != _columns)
                        {
                            NotifyError(_file, $"Formato inválido. Forneça um arquivo no formato CSV com { _columns } colunas.");
                            commandResult = CommandResult.BadRequest($"Importação do arquivo { _file } não foi concluída, existem erros.");
                        }
                        else
                        {
                            List<List<RuleHoliday>> list = await content.ToListOfRuleHolidayListAsync();
                            commandResult = _addRuleHolidayService.AddRange(list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NotifyError(_commandName, ex.Message);
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            if (!commandResult.Success)
            {
                commandResult.Data = GetNotifications();
            }
            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        #endregion

        #region Private Methods

        private void CreateRelationship(List<List<RuleHoliday>> list)
        {
            Country country = null;
            State state = null;

            for (int i = 0, j = list.Count; i < j; i++)
            {
                var ruleHolidays = list[i];
                if (country == null)
                {
                    country = GetCountryByIsoCode(ruleHolidays[0].CountryIsoCode);
                }
                if (state == null)
                {
                    state = GetStateByIsoCode(ruleHolidays[0].CountryIsoCode, ruleHolidays[0].StateIsoCode);
                }

                for (int x = 0, y = ruleHolidays.Count; x < y; x++)
                {
                    if (country != null && ruleHolidays[x].CountryIsoCode != country.CountryIsoCode3)
                    {
                        country = GetCountryByIsoCode(ruleHolidays[0].CountryIsoCode);
                    }
                    if (state != null && ruleHolidays[x].StateIsoCode != state.StateIsoCode)
                    {
                        state = GetStateByIsoCode(ruleHolidays[x].CountryIsoCode, ruleHolidays[x].StateIsoCode);
                    }

                    if (country != null && ruleHolidays[x].CountryIsoCode == country.CountryIsoCode3)
                    {
                        ruleHolidays[x].CreateCountryRelationship(country.Id);
                    }
                    if (state != null && ruleHolidays[x].StateIsoCode == state.CountryIsoCode)
                    {
                        ruleHolidays[x].CreateStateRelationship(state.Id);
                    }
                }
            }
        }

        private async Task CreateRelationshipAsync(List<List<RuleHoliday>> list)
        {
            Country country = null;
            State state = null;

            for (int i = 0, j = list.Count; i < j; i++)
            {
                var ruleHolidays = list[i];
                if (country == null)
                {
                    country = await GetCountryByIsoCodeAsync(ruleHolidays[0].CountryIsoCode);
                }
                if (state == null)
                {
                    state = await GetStateByIsoCodeAsync(ruleHolidays[0].CountryIsoCode, ruleHolidays[0].StateIsoCode);
                }

                for (int x = 0, y = ruleHolidays.Count; x < y; x++)
                {
                    if (country != null && ruleHolidays[x].CountryIsoCode != country.CountryIsoCode3)
                    {
                        country = await GetCountryByIsoCodeAsync(ruleHolidays[0].CountryIsoCode);
                    }
                    if (state != null && ruleHolidays[x].StateIsoCode != state.StateIsoCode)
                    {
                        state = await GetStateByIsoCodeAsync(ruleHolidays[x].CountryIsoCode, ruleHolidays[x].StateIsoCode);
                    }

                    if (country != null && ruleHolidays[x].CountryIsoCode == country.CountryIsoCode3)
                    {
                        ruleHolidays[x].CreateCountryRelationship(country.Id);
                    }
                    if (state != null && ruleHolidays[x].StateIsoCode == state.CountryIsoCode)
                    {
                        ruleHolidays[x].CreateStateRelationship(state.Id);
                    }
                }
            }
        }

        #endregion
    }
}
