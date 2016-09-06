using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerData;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;



namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Welcome!";
            HandleClient handleClient = new HandleClient();
            handleClient.StartChat();
        }

    }
}
