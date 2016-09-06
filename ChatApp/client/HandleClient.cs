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
    class HandleClient
    {
        public static Socket master;
        public static string name;
        public static string id;


        public void StartChat()
        {
            Console.Write("Enter Your Name: ");
            name = Console.ReadLine();

            A: Console.Clear();
            Console.Write("Please enter IP: ");
            string ip = Console.ReadLine();
            Console.Clear();
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), 4242);

            try
            {
                master.Connect(ipEnd);
            }
            catch
            {
                Console.WriteLine("Could not connect to server");
                Thread.Sleep(1000);
                goto A;
            }

            Thread t = new Thread(DataIn);
            t.Start();

            for (;;)
            {
                Console.Write(DateTime.Now.ToLongTimeString() + " (You) ");
                string input = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                ClearCurrentConsoleLine();
                Packet packet = new Packet(PacketType.chat, id);
                packet.Data.Add(name);
                packet.Data.Add(input);
                master.Send(packet.toBytes());

            }
        }

        static void DataIn()
        {
            byte[] buffer;
            int readBytes;

            for (;;)
            {
                try
                {
                    buffer = new byte[master.SendBufferSize];
                    readBytes = master.Receive(buffer);

                    if (readBytes > 0)
                    {
                        DataManager(new Packet(buffer));
                    }
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Server Lost!");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }

        }

        static void DataManager(Packet packet)
        {
            switch (packet.packetType)
            {
                case PacketType.Registration:
                    Console.Clear();
                    Console.WriteLine("Connected to Server!");
                    Console.WriteLine("");
                    id = packet.Data[0];
                    break;
                case PacketType.chat:
                    Console.WriteLine(packet.Data[0] + " > " + packet.Data[1]);
                    break;
            }
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
