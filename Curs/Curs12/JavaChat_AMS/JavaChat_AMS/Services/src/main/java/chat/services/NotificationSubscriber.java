package chat.services;

import chat.notification.Notification;


public interface NotificationSubscriber {
    void notificationReceived(Notification notif);
}
