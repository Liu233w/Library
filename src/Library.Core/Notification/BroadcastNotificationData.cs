using System;
using Abp.Notifications;

namespace Library.Notification
{
    [Serializable]
    public class BroadcastNotificationData : NotificationData
    {
        public string Content { get; set; }

        public string Publisher { get; set; }

        public BroadcastNotificationData(string content, string publisher)
        {
            Content = content;
            Publisher = publisher;
        }

        public BroadcastNotificationData()
        {
        }
    }
}