using Bigai.Holidays.Shared.Domain.Enums.Abstracts;

namespace Bigai.Holidays.Shared.Domain.Enums.Entities
{
    /// <summary>
    /// <see cref="TypeProcess"/> represents the type of process that was requested.
    /// </summary>
    public abstract class TypeProcess : EnumBase<TypeProcess>
    {

        /// <summary>
        /// Indicates that the action to be taken is to Update.
        /// </summary>
        public static TypeProcess Update { get; } = new UpdateRecord();

        /// <summary>
        /// Indicates that the action to be taken is to Register.
        /// </summary>
        public static TypeProcess Register { get; } = new RegisterRecord();

        /// <summary>
        /// Indicates that the action to be taken is to Seed.
        /// </summary>
        public static TypeProcess Seed { get; } = new SeedRecord();

        /// <summary>
        /// Return a instance of <see cref="TypeProcess"/>
        /// </summary>
        /// <param name="key">Key code to identify status.</param>
        /// <param name="name">Name of the status.</param>
        private TypeProcess(int key, string name) : base(key, name) { }

        private sealed class UpdateRecord : TypeProcess
        {
            public UpdateRecord() : base(0, "Update") { }
        }

        private sealed class RegisterRecord : TypeProcess
        {
            public RegisterRecord() : base(1, "Register") { }
        }

        private sealed class SeedRecord : TypeProcess
        {
            public SeedRecord() : base(2, "Seed") { }
        }
    }
}
