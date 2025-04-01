package mpp.hibernate.example.chat;

public class ChatMessageMain {
    public static void main(String[] args) {
        System.out.println("Starting ChatMessage main");
        ChatMessageRepository repo=new ChatMessageHibernateRepository();

        ChatMessage message=new ChatMessage("maria", "ana", "Ce mai faci");

        System.out.println("Salvam mesajul  "+message);
        repo.save(message);

        ChatMessage message1=repo.findOne(1);
        if (message1!=null)
            System.out.println("Am gasit mesajul cu id-ul 1 "+message1);

        System.out.println("Stergem mesajul cu id-ul 1");
        repo.delete(1);

        System.out.println("Actualizam  mesajul cu id-ul 2");
        ChatMessage message2=repo.findOne(2);
        message2.setMessage("Mesaj actualizat");

        repo.update(2, message2);
        System.out.println("Mesajele ramanse in tabela");
        for(ChatMessage m: repo.getAll())
            System.out.println(m);
        System.out.println("-------------------------");


        System.out.println("Mesajele de la maria");
        for(ChatMessage m: repo.getMessagesFrom("maria"))
            System.out.println(m);

        HibernateUtils.closeSessionFactory();
    }
}
