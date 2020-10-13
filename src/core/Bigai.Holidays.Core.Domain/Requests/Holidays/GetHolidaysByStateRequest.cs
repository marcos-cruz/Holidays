namespace Bigai.Holidays.Core.Domain.Requests.Holidays
{
    public class GetHolidaysByStateRequest : GetHolidaysByCountryRequest
    {
        public string StateIsoCode { get; set; }
    }
}
