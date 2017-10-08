using System;
using Abp.Notifications;

namespace Library.LibraryService.Dto
{
    public class MyNotificationDto
    {
        public Guid UserNotificationId { get; set; }

        public UserNotificationState State { get; set; }

        public string Publisher { get; set; }

        public DateTime PublishTime { get; set; }

        public string Content { get; set; }
    }
}