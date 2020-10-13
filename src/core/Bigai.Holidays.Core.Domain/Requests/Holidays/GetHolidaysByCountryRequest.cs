using Bigai.Holidays.Shared.Domain.Requests;

namespace Bigai.Holidays.Core.Domain.Requests.Holidays
{
    public class GetHolidaysByCountryRequest : Request
    {
        public string CountryIsoCode { get; set; }
        public int Year { get; set; }
    }
}
