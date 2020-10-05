using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Countries;
using Bigai.Holidays.Core.Infra.Data.Repositories.Holidays;
using Bigai.Holidays.Core.Infra.Data.Repositories.States;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Infra.Data.UnitOfWork
{
    /// <summary>
    /// <see cref="IUnitOfWork"/> implements a contract to save all changes to database.
    /// </summary>
    public class UnitOfWork : IUnitOfWorkCore
    {
        #region Variáveis privadas

        private readonly HolidaysContext _holidaysContext;
        private ICountryRepository _countryRepository;
        private IStateRepository _stateRepository;
        private IRuleHolidayRepository _ruleHolidayRepository;
        private IHolidayRepository _holidayRepository;

        #endregion

        #region Public Properties

        /// <summary>
        /// Performs read and write operations in the country repository.
        /// </summary>
        public ICountryRepository CountryRepository
        {
            get { return _countryRepository; }
        }

        /// <summary>
        /// Performs read and write operations in the state repository.
        /// </summary>
        public IStateRepository StateRepository
        {
            get { return _stateRepository; }
        }

        /// <summary>
        /// Performs read and write operations in the rule holiday repository.
        /// </summary>
        public IRuleHolidayRepository RuleHolidayRepository
        {
            get { return _ruleHolidayRepository; }
        }

        /// <summary>
        /// Performs read and write operations in the holiday repository.
        /// </summary>
        public IHolidayRepository HolidayRepository
        {
            get { return _holidayRepository; }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Return a instance of <see cref="UnitOfWork"/>.
        /// </summary>
        /// <param name="context">Contexto to database.</param>
        public UnitOfWork(HolidaysContext context)
        {
            _holidaysContext = context ?? throw new ArgumentNullException(nameof(context));
            _countryRepository = new CountryRepository(_holidaysContext);
            _stateRepository = new StateRepository(_holidaysContext);
            _ruleHolidayRepository = new RuleHolidayRepository(_holidaysContext);
            _holidayRepository = new HolidayRepository(_holidaysContext);
        }

        #endregion

        #region Métodos Commit

        public bool Commit()
        {
            try
            {
                return _holidaysContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                //
                // TODO: [UnitOfWork] - Disparar envio de e-mail para suporte/desenvolvimento.
                //
                throw ex;
            }
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                return await _holidaysContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                //
                // TODO: [UnitOfWork] - Disparar envio de e-mail para suporte/desenvolvimento.
                //
                throw ex;
            }
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Releases the allocated resources for this context.
        /// </summary>
        public void Dispose()
        {
            if (_holidaysContext != null)
            {
                _holidaysContext.Dispose();
            }
        }

        #endregion
    }
}
