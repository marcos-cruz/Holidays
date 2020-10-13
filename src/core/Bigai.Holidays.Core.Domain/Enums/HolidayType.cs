using Bigai.Holidays.Shared.Domain.Enums.Abstracts;

namespace Bigai.Holidays.Core.Domain.Enums
{
    /// <summary>
    /// <see cref="HolidayType"/> represents Holiday range type.
    /// </summary>
    public abstract class HolidayType : EnumBase<HolidayType>
    {
        /// <summary>
        /// Indicates that it is a local holiday.
        /// </summary>
        public static HolidayType Local { get; } = new HolidayLocal();

        /// <summary>
        /// Indicates that it is a national holiday.
        /// </summary>
        public static HolidayType National { get; } = new HolidayNational();

        /// <summary>
        /// Indicates that it is a regional holiday.
        /// </summary>
        public static HolidayType Regional { get; } = new HolidayRegional();

        /// <summary>
        /// Indicates that it is a regional holiday.
        /// </summary>
        public static HolidayType Observance { get; } = new HolidayObservance();

        /// <summary>
        /// Return a instance of <see cref="HolidayType"/>
        /// </summary>
        /// <param name="key">Key code to identify status.</param>
        /// <param name="name">Name of the status.</param>
        private HolidayType(int key, string name) : base(key, name) { }

        private sealed class HolidayNational : HolidayType
        {
            public HolidayNational() : base(0, "National") { }
        }

        private sealed class HolidayRegional : HolidayType
        {
            public HolidayRegional() : base(1, "Regional") { }
        }
        
        private sealed class HolidayLocal : HolidayType
        {
            public HolidayLocal() : base(2, "Local") { }
        }
        
        private sealed class HolidayObservance : HolidayType
        {
            public HolidayObservance() : base(3, "Observance") { }
        }
    }

}
