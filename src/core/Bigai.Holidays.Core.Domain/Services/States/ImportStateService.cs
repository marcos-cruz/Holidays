using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Mappers.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Services.Abstracts;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.States
{
    /// <summary>
    /// <see cref="ImportStateService"/> implements a contract to perform domain operations with CSV files for <see cref="State"/>.
    /// </summary>
    public class ImportStateService : HolidayBaseService, IImportStateService
    {
        #region Private Variables

        private string _file = "";
        private const int _columns = 5;
        private readonly IAddStateService _addStateService;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="AddCountryService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        /// <param name="addStateService">Service do add countries in database.</param>
        public ImportStateService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddStateService addStateService) : base(notificationHandler, unitOfWork, userLogged)
        {
            _addStateService = addStateService ?? throw new ArgumentNullException(nameof(addStateService));
        }

        #endregion

        #region Public Methods

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
                            List<List<State>> list = await content.ToListOfStatesListAsync(CountryRepository, GetUserLogged());
                            bool validateRepository = await MustBeValidateAsync((list[0])[0]);
                            commandResult = await _addStateService.AddRangeAsync(list, validateRepository);
                            if (commandResult.Success)
                            {
                                commandResult.Message = commandResult.Message.Replace("Ação concluída", $"{_file} importado");
                                commandResult.Data = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                commandResult = CommandResult.InternalServerError($"Ocorreu um erro ao salvar.");
            }

            CsvHelper.DeleteFile(filename);

            watch.Stop();
            commandResult.ElapsedTime = watch.ElapsedMilliseconds;

            return commandResult;
        }

        #endregion

        #region Private Methods

        private async Task<bool> MustBeValidateAsync(State state)
        {
            return (await StateRepository.GetCountAsync(s => s.CountryIsoCode == state.CountryIsoCode && s.StateIsoCode == state.StateIsoCode)) > 0;
        }

        #endregion
    }
}
