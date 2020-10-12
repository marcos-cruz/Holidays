using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Models;
using Bigai.Holidays.Shared.Domain.Services;
using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Services.Abstracts
{
    public abstract class HolidayBaseService : DomainService
    {
        #region Private Variables

        protected readonly IUnitOfWorkCore _unitOfWork;
        protected string _commandName;

        #endregion

        #region Properties

        /// <summary>
        /// Context for reading and writing countries.
        /// </summary>
        public ICountryRepository CountryRepository
        {
            get { return _unitOfWork.CountryRepository; }
        }

        /// <summary>
        /// Context for reading and writing States.
        /// </summary>
        public IStateRepository StateRepository
        {
            get { return _unitOfWork.StateRepository; }
        }

        /// <summary>
        /// Context for reading and writing rules.
        /// </summary>
        public IRuleHolidayRepository RuleHolidayRepository
        {
            get { return _unitOfWork.RuleHolidayRepository; }
        }

        /// <summary>
        /// Context for reading and holidays.
        /// </summary>
        public IHolidayRepository HolidayRepository
        {
            get { return _unitOfWork.HolidayRepository; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="HolidayBaseService"/>
        /// </summary>
        /// <param name="notificationHandler">Handling error notification messages.</param>
        /// <param name="unitOfWork">Context to read and writing.</param>
        /// <param name="userLogged">User who is logged in.</param>
        public HolidayBaseService(INotificationHandler notificationHandler, IUnitOfWorkCore unitOfWork, IUserLogged userLogged) : base(notificationHandler, unitOfWork, userLogged)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of elements contained in the List.
        /// </summary>
        /// <param name="listOfLists">List of lists to be counted.</param>
        /// <returns>The number of elements contained in the List.</returns>
        public int Count<T>(List<List<T>> listOfLists) where T : Entity
        {
            int count = 0;

            if (listOfLists != null && listOfLists.Count > 0)
            {
                for (int i = 0, j = listOfLists.Count; i < j; i++)
                {
                    count += listOfLists[i].Count;
                }
            }

            return count;
        }

        /// <summary>
        /// Gets the number of elements contained in the List.
        /// </summary>
        /// <param name="list">List of lists to be counted.</param>
        /// <returns>The number of elements contained in the List.</returns>
        public int Count<T>(List<T> list) where T : Entity
        {
            return list.Count;
        }

        /// <summary>
        /// Gets the <see cref="Country"/> by country ISO 3 code.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <returns>Instance of <see cref="Country"/> or null if country does not exist.</returns>
        public async Task<Country> GetCountryByIsoCodeAsync(string countryIsoCode)
        {
            return (await CountryRepository.FindAsync(c => c.CountryIsoCode3 == countryIsoCode)).FirstOrDefault();
        }

        /// <summary>
        /// Gets the <see cref="State"/> by state ISO code.
        /// </summary>
        /// <param name="countryIsoCode">State ISO code.</param>
        /// <returns>Instance of <see cref="State"/> or null if country does not exist.</returns>
        public async Task<State> GetStateByIsoCodeAsync(string countryIsoCode, string stateIsoCode)
        {
            return (await StateRepository.FindAsync(c => c.CountryIsoCode == countryIsoCode && c.StateIsoCode == stateIsoCode)).FirstOrDefault();
        }

        #endregion
    }
}
