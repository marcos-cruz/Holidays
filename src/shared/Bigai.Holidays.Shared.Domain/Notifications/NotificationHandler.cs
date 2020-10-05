using Bigai.Holidays.Shared.Domain.Interfaces.Notifications;
using System.Collections.Generic;

namespace Bigai.Holidays.Shared.Domain.Notifications
{
    /// <summary>
    /// <see cref="NotificationHandler"/> implements a contract for handling error notification messages.
    /// </summary>
    public class NotificationHandler : INotificationHandler
    {
        #region Private Variables

        private List<DomainNotification> _notifications;

        #endregion

        #region Constructor

        /// <summary>
        /// Return a instance of <see cref="NotificationHandler"/>.
        /// </summary>
        public NotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        #endregion

        #region Public Methods

        public bool HasNotification()
        {
            return _notifications != null && _notifications.Count > 0;
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public void NotifyError(DomainNotification notificacao)
        {
            _notifications.Add(notificacao);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }

        #endregion
    }
}
