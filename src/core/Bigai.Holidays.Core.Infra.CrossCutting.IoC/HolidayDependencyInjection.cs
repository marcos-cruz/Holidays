using Bigai.Holidays.Core.Domain.Interfaces.Repositories;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Services.Countries;
using Bigai.Holidays.Core.Domain.Services.Holidays;
using Bigai.Holidays.Core.Domain.Services.States;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.UnitOfWork;
using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using Bigai.Holidays.Shared.Domain.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Bigai.Holidays.Core.Infra.CrossCutting.IoC
{
    /// <summary>
    /// <see cref="HolidayDependencyInjection"/> Setup dependency injection.
    /// </summary>
    public class HolidayDependencyInjection
    {
        /// <summary>
        /// Configure dependency injections.
        /// </summary>
        /// <param name="services">Represents a collection of services.</param>
        public static void RegisterDependencies(IServiceCollection services)
        {
            ServicesDependencyInjection(services);
            
            RepositoryDependencyInjection(services);
        }

        private static void ServicesDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddScoped<IAddCountryService, AddCountryService>();
            services.AddScoped<IImportCountryService, ImportCountryService>();
            
            services.AddScoped<IAddStateService, AddStateService>();
            services.AddScoped<IImportStateService, ImportStateService>();

            services.AddScoped<IAddRuleHolidayService, AddRuleHolidayService>();
            services.AddScoped<IImportRuleHolidayService, ImportRuleHolidayService>();

            services.AddScoped<IQueryHolidayService, QueryHolidayService>();
        }

        private static void RepositoryDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<HolidaysContext>();

            //services.AddScoped<ICountryRepository, CountryRepository>();

            //services.AddScoped<IStateRepository, StateRepository>();

            //services.AddScoped<IRuleHolidayRepository, RuleHolidayRepository>();

            services.AddScoped<IUnitOfWorkCore, UnitOfWork>();
        }
    }
}
