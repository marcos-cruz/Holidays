using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.States
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(HolidaysContext dbContext) : base(dbContext)
        {
        }
    }
}
