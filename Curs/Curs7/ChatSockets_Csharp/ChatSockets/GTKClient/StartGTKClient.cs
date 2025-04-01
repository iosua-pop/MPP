using System;
using System.Configuration;
using Gtk;
using chat.services;
using chat.network.server;
namespace GTKClient
{
    internal class StartGTKClient
    
    {
        private static int DEFAULT_PORT=55556;
        private static String DEFAULT_IP="127.0.0.1";
        public static void Main(string[] args)
        {
            Application.Init ();
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
            
            Console.WriteLine("Using  server on IP {0} and port {1}", ip, port);
            IChatServices server = new ChatServerProxy(ip, port);
            GTKClientCtrl ctrl=new GTKClientCtrl(server);
           Window w=new LoginWindow(ctrl);
          // Window w = new ChatWindow();
            w.ShowAll();
            Application.Run ();
                        
        }
    }
}