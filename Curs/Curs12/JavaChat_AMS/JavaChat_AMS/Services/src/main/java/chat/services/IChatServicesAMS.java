package chat.services;

import chat.model.Message;
import chat.model.User;


public interface IChatServicesAMS {
    void login(User user) throws ChatException;
    void sendMessageToAll(Message message) throws ChatException;
    void logout(User user) throws ChatException;
    User[] getLoggedUsers() throws ChatException;
}
