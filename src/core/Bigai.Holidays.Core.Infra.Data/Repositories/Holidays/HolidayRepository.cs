using Bigai.Holidays.Core.Domain.Enums;
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

        public async Task<IEnumerable<Holiday>> GetHolidaysByCountryAsync(string countryIsoCode, int year)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                return await DbContext.Holidays
                    .Where(h => h.CountryCode == countryIsoCode && h.HolidayDate >= startDate && h.HolidayDate <= endDate)
                    .AsNoTracking()
                    .OrderBy(h => h.HolidayDate)
                    .ToListAsync();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysByMonthAsync(string countryIsoCode, int year, int month)
        {
            int day = DateTime.DaysInMonth(year, month);
            DateTime startDate = new DateTime(year, month, 01);
            DateTime endDate = new DateTime(year, month, day);

            try
            {
                return await DbContext.Holidays
                    .Where(h => h.CountryCode == countryIsoCode && 
                           h.HolidayDate >= startDate &&
                           h.HolidayDate <= endDate &&
                           h.HolidayType != HolidayType.Local)
                    .AsNoTracking()
                    .OrderBy(h => h.HolidayDate)
                    .ToListAsync();
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysByStateAsync(string countryIsoCode, string stateIsoCode, int year)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                return await DbContext.Holidays
                    .Where(holiday =>
                          (holiday.CountryCode == countryIsoCode && holiday.HolidayType == HolidayType.National && holiday.HolidayDate >= startDate && holiday.HolidayDate <= endDate) ||
                          (holiday.CountryCode == countryIsoCode && holiday.StateCode == stateIsoCode && holiday.HolidayType != HolidayType.Local && holiday.HolidayDate >= startDate && holiday.HolidayDate <= endDate))
                    .AsNoTracking()
                    .OrderBy(h => h.HolidayDate)
                    .ToListAsync();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
