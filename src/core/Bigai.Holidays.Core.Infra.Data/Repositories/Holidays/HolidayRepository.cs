using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.Holidays
{
    public class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(HolidaysContext dbContext) : base(dbContext)
        {
        }
    }
}
