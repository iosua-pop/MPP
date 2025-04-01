using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;

using System.Threading;
using chat.persistence.repository.mock;
using chat.persistence;
using chat.services;
using chat.network.client;
using chat.persistence.repository.db;
using ServerTemplate;
namespace chat
{
    using server;
    class StartServer
    {
        private static int DEFAULT_PORT=55556;
        private static String DEFAULT_IP="127.0.0.1";
        static void Main(string[] args)
        {
            
           // IUserRepository userRepo = new UserRepositoryMock();
           Console.WriteLine("Reading properties from app.config ...");
           int port = DEFAULT_PORT;
           String ip = DEFAULT_IP;
           String portS= ConfigurationManager.AppSettings["port"];
           if (portS == null)
           {
               Console.WriteLine("Port property not set. Using default value "+DEFAULT_PORT);
           }
           else
           {
               bool result = Int32.TryParse(portS, out port);
               if (!result)
               {
                   Console.WriteLine("Port property not a number. Using default value "+DEFAULT_PORT);
                   port = DEFAULT_PORT;
                   Console.WriteLine("Portul "+port);
               }
           }
           String ipS=ConfigurationManager.AppSettings["ip"];
           
           if (ipS == null)
           {
               Console.WriteLine("Port property not set. Using default value "+DEFAULT_IP);
           }
           Console.WriteLine("Configuration Settings for database {0}",GetConnectionStringByName("chatDB"));
           IDictionary<String, string> props = new SortedList<String, String>();
           props.Add("ConnectionString", GetConnectionStringByName("chatDB"));
           IUserRepository userRepo=new UserRepositoryDb(props);
            IMessageRepository messageRepository=new MessageRepositoryDb(props);
           // IUserRepository userRepo=new UserRepositoryMock();
           // IMessageRepository messageRepository=new MessageRepositoryMock();
            IChatServices serviceImpl = new ChatServerImpl(userRepo, messageRepository);

         
           Console.WriteLine("Starting server on IP {0} and port {1}", ip, port);
			SerialChatServer server = new SerialChatServer(ip,port, serviceImpl);
            server.Start();
            Console.WriteLine("Server started ...");
            //Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();
            
        }
        
        
		
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }

    public class SerialChatServer: ConcurrentServer 
    {
        private IChatServices server;
        private ChatClientWorker worker;
        public SerialChatServer(string host, int port, IChatServices server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialChatServer...");
        }
        protected override Thread createWorker(TcpClient client)
        {
            worker = new ChatClientWorker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }
    
}
