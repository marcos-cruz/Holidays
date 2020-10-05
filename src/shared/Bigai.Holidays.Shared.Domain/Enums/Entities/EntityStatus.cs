using Bigai.Holidays.Shared.Domain.Enums.Abstracts;

namespace Bigai.Holidays.Shared.Domain.Enums.Entities
{
    /// <summary>
    /// <see cref="EntityStatus"/> represents the current status of a record.
    /// </summary>
    public abstract class EntityStatus : EnumBase<EntityStatus, string>
    {
        #region Region Size Configuration

        public static readonly int SizeCodeStatus = 1;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates that the record is Active.
        /// </summary>
        public static EntityStatus Active { get; } = new RecordActive();

        /// <summary>
        /// Indicates that the record is Locked.
        /// </summary>
        public static EntityStatus Locked { get; } = new RecordLocked();

        /// <summary>
        /// Indicates that the record has been deleted.
        /// </summary>
        public static EntityStatus Deleted { get; } = new RecordDeleted();

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="EntityStatus"/>
        /// </summary>
        /// <param name="key">Key code to identify status.</param>
        /// <param name="name">Name of the status.</param>
        private EntityStatus(string key, string name) : base(key, name) { }

        #endregion

        #region Private Methods

        private sealed class RecordActive : EntityStatus
        {
            public RecordActive() : base("A", "Active") { }
        }

        private sealed class RecordLocked : EntityStatus
        {
            public RecordLocked() : base("L", "Locked") { }
        }

        private sealed class RecordDeleted : EntityStatus
        {
            public RecordDeleted() : base("D", "Deleted") { }
        }

        #endregion
    }
}
