using Bigai.Holidays.Shared.Domain.Enums.Abstracts;

namespace Bigai.Holidays.Shared.Domain.Enums.Entities
{
    /// <summary>
    /// <see cref="ActionType"/> represents the type of action that was requested.
    /// </summary>
    public abstract class ActionType : EnumBase<ActionType>
    {

        /// <summary>
        /// Indicates that the action to be taken is to Update.
        /// </summary>
        public static ActionType Update { get; } = new UpdateRecord();

        /// <summary>
        /// Indicates that the action to be taken is to Register.
        /// </summary>
        public static ActionType Register { get; } = new RegisterRecord();

        /// <summary>
        /// Indicates that the action to be taken is to Seed.
        /// </summary>
        public static ActionType Seed { get; } = new SeedRecord();

        /// <summary>
        /// Return a instance of <see cref="ActionType"/>
        /// </summary>
        /// <param name="key">Key code to identify status.</param>
        /// <param name="name">Name of the status.</param>
        private ActionType(int key, string name) : base(key, name) { }

        private sealed class UpdateRecord : ActionType
        {
            public UpdateRecord() : base(0, "Update") { }
        }

        private sealed class RegisterRecord : ActionType
        {
            public RegisterRecord() : base(1, "Register") { }
        }

        private sealed class SeedRecord : ActionType
        {
            public SeedRecord() : base(2, "Seed") { }
        }
    }
}
