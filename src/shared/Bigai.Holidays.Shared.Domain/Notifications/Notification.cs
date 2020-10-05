namespace Bigai.Holidays.Shared.Domain.Notifications
{
    /// <summary>
    /// <see cref="Notification"/> represents a notification message.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Name of the field or action to which the message refers.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Message text.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Return a instance of <see cref="Notification"/>
        /// </summary>
        /// <param name="key">Who the message refers to.</param>
        /// <param name="value">Message text.</param>
        public Notification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
