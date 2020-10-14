using Bigai.Holidays.Shared.Domain.Requests;

namespace Bigai.Holidays.Core.Domain.Requests.Holidays.Abstracts
{
    public abstract class HolidaysRequest : Request
    {
        public string CountryIsoCode { get; set; }
    }
}
