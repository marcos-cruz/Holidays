using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Infra.Data.Mappings.Countries;
using Bigai.Holidays.Core.Infra.Data.Mappings.Holidays;
using Bigai.Holidays.Core.Infra.Data.Mappings.States;
using Microsoft.EntityFrameworkCore;

namespace Bigai.Holidays.Core.Infra.Data.Contexts
{
    /// <summary>
    /// <see cref="HolidaysContext"/> represents the database context of holidays.
    /// </summary>
    public class HolidaysContext : DbContext
    {
        #region Public Variables

        public const string KeyConnectionString = "HolidayConnection";

        #endregion

        #region Properites

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<RuleHoliday> RulesHolidays { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        /// Return a instance of <see cref="HolidaysContext"/>
        /// </summary>
        /// <param name="options"></param>
        public HolidaysContext(DbContextOptions<HolidaysContext> options) : base(options) { }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Maps domain classes to tables in the database.
        /// </summary>
        /// <param name="modelBuilder">API for configuring entities and relationships to the database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CountryMapping());
            modelBuilder.ApplyConfiguration(new StateMapping());
            modelBuilder.ApplyConfiguration(new RuleHolidayMapping());
            modelBuilder.ApplyConfiguration(new HolidayMapping());

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
