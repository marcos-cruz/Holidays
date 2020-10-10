using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.Holidays
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(HolidaysContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Holiday> GetHolidays(string countryIsoCode, int year)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                return DbContext.Holidays.Where(h => h.CountryCode == countryIsoCode && h.HolidayDate >= startDate && h.HolidayDate <= endDate).AsNoTracking().ToList();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysAsync(string countryIsoCode, int year)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                return await DbContext.Holidays.Where(h => h.CountryCode == countryIsoCode && h.HolidayDate >= startDate && h.HolidayDate <= endDate).AsNoTracking().ToListAsync();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
