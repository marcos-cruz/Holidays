using Bigai.Holidays.Shared.Domain.Notifications;
using System.Collections.Generic;

namespace Bigai.Holidays.Shared.Domain.Interfaces.Notifications
{
    /// <summary>
    /// <see cref="INotificationHandler"/> represents a contract for handling error notification messages.
    /// </summary>
    public interface INotificationHandler
    {
        /// <summary>
        /// Determines whether there is an error.
        /// </summary>
        /// <returns><c>true</c> if there is an error, otherwise <c>false</c>.</returns>
        bool HasNotification();

        /// <summary>
        /// Returns a list of errors.
        /// </summary>
        /// <returns>List of errors</returns>
        List<DomainNotification> GetNotifications();

        /// <summary>
        /// Notifies the error handler that an error has occurred.
        /// </summary>
        /// <param name="notificacao">Notification message.</param>
        void NotifyError(DomainNotification notificacao);
    }
}
