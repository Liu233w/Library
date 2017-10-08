using System;
using Abp.Notifications;

namespace Library.LibraryService.Dto
{
    /// <summary>
    /// 通知对用户的映射
    /// </summary>
    public class BroadcastUserNotificationDto
    {
        public Guid UserNotificationId { get; set; }

        public UserNotificationState State { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishTime { get; set; }

        public string Content { get; set; }
    }
}