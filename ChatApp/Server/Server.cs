using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerData;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace Server
{
    class Server
    {

        static Socket listenerSocket;
        static List<ClientData> _clients;
        


        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Starting Server...");
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ClientData>();

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.getIp4Address()), 4242);
            listenerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();
            Console.WriteLine("Listening IP: " + Packet.getIp4Address() + "  Listening Port: 4242");
        }

        static void ListenThread()
        {
            for (; ; )
            {
                listenerSocket.Listen(0);
                _clients.Add(new ClientData(listenerSocket.Accept()));
            }
        }

        public static void Data_IN(object cSocket)
        {
            Socket clientSocket = (Socket)cSocket;

            byte[] Buffer;
            int readBytes;

            for (;;)
            {
                try
                {
                    Buffer = new byte[clientSocket.SendBufferSize];
                    readBytes = clientSocket.Receive(Buffer);

                    if (readBytes > 0)
                    {
                        Packet packet = new Packet(Buffer);
                        dataManager(packet);
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Client Disconnected.");
                    clientSocket.Close();
                    
                }
            }
        }

        public static void dataManager(Packet packet)
        {
            switch (packet.packetType)
            {
                case PacketType.chat:
                    foreach (ClientData clientData in _clients)
                    {
                        clientData.clientSocket.Send(packet.toBytes());
                    }
                    break;
            }
        }

        
        }
    }

