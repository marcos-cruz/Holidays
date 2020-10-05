using System;

namespace Bigai.Holidays.Shared.Domain.Notifications
{
    /// <summary>
    /// <see cref="DomainNotification"/> Represents a notification message in response to an action.
    /// </summary>
    public class DomainNotification
    {
        /// <summary>
        /// Id that identifies the notification message.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Notification message.
        /// </summary>
        public Notification Notification { get; private set; }

        /// <summary>
        /// Return a instance of <see cref="DomainNotification"/>.
        /// </summary>
        /// <param name="key">Who the message refers to.</param>
        /// <param name="value">Message text.</param>
        public DomainNotification(string key, string value)
        {
            Id = Guid.NewGuid();

            Notification = new Notification(key, value);
        }
    }
}
