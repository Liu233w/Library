using System;

namespace Library.LibraryService.Dto
{
    /// <summary>
    /// 某一项通知
    /// </summary>
    public class BroadcastNotificationDto
    {
        public Guid NotificationId { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishTime { get; set; }

        public string Content { get; set; }
    }
}