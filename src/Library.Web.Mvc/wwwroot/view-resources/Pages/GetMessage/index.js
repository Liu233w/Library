(function () {
    $(function () {
        var _notificationService = abp.services.app.library;

        $('.mark-notification').click(function (e) {
            //alert("notification-id", notificationid);
            var notificationid = $(this).attr("data-usernotification-id");
            markNotification(notificationid);
        });

        function refreshNotificationList() {
            location.reload(true); //reload page to see new user!
        }

        function markNotification(notificationId) {
            abp.message.confirm(
                "Mark this Message?",
                function (isConfirmed) {
                    if (isConfirmed) {
                        _notificationService.markNotificationAsRead({
                            userNotificationId: notificationId
                        }).done(function () {
                            refreshNotificationList();
                        })
                    }
                }
            );
        }
    });
})();
