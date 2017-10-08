using Abp.Notifications;

namespace Library.LibraryService.Dto
{
    public class GetMyNotificationsInput
    {
        /// <summary>
        /// 通知的状态，为 null 时表示同时选中 Read 和 Unread 的通知（所有通知）
        /// </summary>
        public UserNotificationState? NotificationState { get; set; }
    }
}