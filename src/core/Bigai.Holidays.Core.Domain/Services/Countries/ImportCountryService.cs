using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Mappers.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Countries
{
    /// <summary>
    /// <see cref="ImportCountryService"/> implements a contract to perform domain operations with CSV files for <see cref="Country"/>.
    /// </summary>
    public class ImportCountryService : CountryService, IImportCountryService
    {
        #region Private Variables

        private string _file = "";
        private const int _columns = 13;
        private readonly IAddCountryService _addCountryService;

        #endregion

        #region Constructor

        /// <summary>
        /// Returna instance of <see cref="ImportCountryService"/>.
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing countries.</param>
        /// <param name="addCountryService">Service do add countries in database.</param>
        public ImportCountryService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged, IAddCountryService addCountryService) : base(notificationHandler, unitOfWork, userLogged)
        {
            _addCountryService = addCountryService ?? throw new ArgumentNullException(nameof(addCountryService));
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
                            List<List<Country>> list = content.ToListOfCountryList(GetUserLogged());
                            commandResult = _addCountryService.AddRange(list);
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
                            List<List<Country>> list = await content.ToListOfCountryListAsync(GetUserLogged());
                            commandResult = _addCountryService.AddRange(list);
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
    }
}
