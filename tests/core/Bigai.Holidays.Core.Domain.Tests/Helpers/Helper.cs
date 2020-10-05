using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Core.Domain.Services.Holidays;
using Bigai.Holidays.Core.Domain.Services.States;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Countries;
using Bigai.Holidays.Core.Infra.Data.Repositories.Holidays;
using Bigai.Holidays.Core.Infra.Data.Repositories.States;
using Bigai.Holidays.Core.Infra.Data.UnitOfWork;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace Bigai.Holidays.Core.Domain.Tests.Helpers
{
    public static class Helper
    {
        private static INotificationHandler _notificationHandler = null;
        private static HolidaysContext _holidaysContext = null;
        private static ICountryRepository _countryRepository = null;
        private static IStateRepository _stateRepository = null;
        private static IRuleHolidayRepository _ruleHolidayRepository = null;
        private static IHolidayRepository _holidayRepository = null;
        private static IUnitOfWorkCore _unitOfWorkCore = null;
        private static IImportCountryService _importCountryService = null;
        private static IImportStateService _importStateService = null;
        private static IImportRuleHolidayService _importRuleHolidayService = null;
        private static IAddCountryService _addCountryService = null;
        private static IAddStateService _addStateService = null;
        private static IAddRuleHolidayService _addRuleHolidayService = null;

        public static string GetFilePath(string fileName)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] countriesFile = { startupPath, "Helpers", "Data", fileName };
            string file = Path.Combine(countriesFile);

            return file;
        }

        public static IAddCountryService GetAddCountryService()
        {
            if (_addCountryService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();

                _addCountryService = new AddCountryService(notificationHandler, unitOfWork);
            }
            return _addCountryService;
        }

        public static IAddStateService GetAddStateService()
        {
            if (_addStateService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();

                _addStateService = new AddStateService(notificationHandler, unitOfWork);
            }
            return _addStateService;
        }

        public static IAddRuleHolidayService GetAddRuleHolidayService()
        {
            if (_addRuleHolidayService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();

                _addRuleHolidayService = new AddRuleHolidayService(notificationHandler, unitOfWork);
            }
            return _addRuleHolidayService;
        }

        public static IImportCountryService GetImportCountryService()
        {
            if (_importCountryService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();
                var addCountryService = GetAddCountryService();

                _importCountryService = new ImportCountryService(notificationHandler, unitOfWork, addCountryService);
            }

            return _importCountryService;
        }

        public static IImportStateService GetImportStateService()
        {
            if (_importStateService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();
                var addStateService = GetAddStateService();

                _importStateService = new ImportStateService(notificationHandler, unitOfWork, addStateService);
            }

            return _importStateService;
        }

        public static IImportRuleHolidayService GetImportRuleHolidayService()
        {
            if (_importRuleHolidayService == null)
            {
                var notificationHandler = GetNotificationHandler();
                var unitOfWork = GetUnitOfWorkCore();
                var addRuleHolidayService = GetAddRuleHolidayService();

                _importRuleHolidayService = new ImportRuleHolidayService(notificationHandler, unitOfWork, addRuleHolidayService);
            }

            return _importRuleHolidayService;
        }

        public static IUnitOfWorkCore GetUnitOfWorkCore()
        {
            if (_unitOfWorkCore == null)
            {
                HolidaysContext dbContext = GetInMemoryContext();

                //
                // TODO: Melhoar
                //
                ICountryRepository countryRepository = GetCountryRepository(dbContext);
                IStateRepository stateRepository = GetStateRepository(dbContext);
                IRuleHolidayRepository ruleHolidayRepository = GetRuleHolidayRepository(dbContext);
                IHolidayRepository holidayRepository = GetHolidayRepository(dbContext);

                _unitOfWorkCore = new UnitOfWork(dbContext);

            }

            return _unitOfWorkCore;
        }

        private static HolidaysContext GetInMemoryContext()
        {
            if (_holidaysContext == null)
            {
                DbContextOptions<HolidaysContext> options;
                var builder = new DbContextOptionsBuilder<HolidaysContext>();
                builder.UseInMemoryDatabase("HolidaysContextTest");
                options = builder.Options;

                _holidaysContext = new HolidaysContext(options);
                _holidaysContext.Database.EnsureDeleted();
                _holidaysContext.Database.EnsureCreated();
            }

            return _holidaysContext;
        }

        private static INotificationHandler GetNotificationHandler()
        {
            if (_notificationHandler == null)
            {
                _notificationHandler = new NotificationHandler();
            }

            return _notificationHandler;
        }

        private static ICountryRepository GetCountryRepository(HolidaysContext dbContext)
        {
            if (_countryRepository == null)
            {
                _countryRepository = new CountryRepository(dbContext);
            }

            return _countryRepository;
        }

        private static IStateRepository GetStateRepository(HolidaysContext dbContext)
        {
            if (_stateRepository == null)
            {
                _stateRepository = new StateRepository(dbContext);
            }

            return _stateRepository;
        }

        private static IRuleHolidayRepository GetRuleHolidayRepository(HolidaysContext dbContext)
        {
            if (_ruleHolidayRepository == null)
            {
                _ruleHolidayRepository = new RuleHolidayRepository(dbContext);
            }

            return _ruleHolidayRepository;
        }

        private static IHolidayRepository GetHolidayRepository(HolidaysContext dbContext)
        {
            if (_holidayRepository == null)
            {
                _holidayRepository = new HolidayRepository(dbContext);
            }

            return _holidayRepository;
        }
    }
}
