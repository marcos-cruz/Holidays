using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.Holidays
{
    public class RuleHolidayRepository : Repository<RuleHoliday>, IRuleHolidayRepository
    {
        public RuleHolidayRepository(HolidaysContext dbContext) : base(dbContext)
        {
        }
    }
}
