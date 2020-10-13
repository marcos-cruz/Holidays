namespace Bigai.Holidays.Core.Domain.Requests.Holidays
{
    public class GetHolidaysByMonthRequest: GetHolidaysByCountryRequest
    {
        public int Month { get; set; }
    }
}
