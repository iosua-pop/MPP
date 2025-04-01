package chat.services;

import chat.model.Message;
import chat.model.User;

public interface IChatNotificationService {
    void userLoggedIn(User user);
    void userLoggedOut(User user);
    void newMessage(Message message);
}
