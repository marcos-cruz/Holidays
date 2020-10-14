using Bigai.Holidays.Core.Domain.Requests.Holidays.Abstracts;
using Bigai.Holidays.Shared.Domain.Requests;

namespace Bigai.Holidays.Core.Domain.Requests.Holidays
{
    public class GetHolidaysByCountryRequest : HolidaysRequest
    {
        public int Year { get; set; }
    }
}
