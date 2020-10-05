using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.Countries
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(HolidaysContext dbContext) : base(dbContext)
        {
        }
    }
}
