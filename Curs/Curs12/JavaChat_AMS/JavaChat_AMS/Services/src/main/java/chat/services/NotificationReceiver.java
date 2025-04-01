package chat.services;

import chat.notification.Notification;


public interface NotificationReceiver {
    void start(NotificationSubscriber subscriber);
    void stop();
}
