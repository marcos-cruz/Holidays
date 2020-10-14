using Bigai.Holidays.Core.Domain.Requests.Holidays.Abstracts;
using System;

namespace Bigai.Holidays.Core.Domain.Requests.Holidays
{
    public class GetHolidaysByDateRequest : HolidaysRequest
    {
        public DateTime HolidayDate { get; set; }
    }
}
