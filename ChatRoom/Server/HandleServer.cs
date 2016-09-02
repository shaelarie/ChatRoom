//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace Server
//{
//    public class HandleServer
//    {
//        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        private static List<Socket> clientSockets = new List<Socket>();
//        private const int BUFFER_SIZE = 2048;
//        private const int PORT = 100;
//        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

//        public void SetupServer()
//        {
//            Console.WriteLine("Setting up server...");
//            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
//            serverSocket.Listen(0);
//            serverSocket.BeginAccept(AcceptCallback, null);
//            Console.WriteLine("Server setup complete");
//        }


//        public void CloseAllSockets()
//        {
//            foreach (Socket socket in clientSockets)
//            {
//                socket.Shutdown(SocketShutdown.Both);
//                socket.Close();
//            }

//            serverSocket.Close();
//        }

//        public void AcceptCallback(IAsyncResult AR)
//        {
//            Socket socket;
            
//            try
//            {
//                socket = serverSocket.EndAccept(AR);
//            }
//            catch (ObjectDisposedException)
//            {
//                return;
//            }
            
//            clientSockets.Add(socket);
//            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveSingleCallback, socket);
//            Console.WriteLine("Client connected, waiting for request...");
//            serverSocket.BeginAccept(AcceptCallback, null);
//        }

//        private static void ReceiveSingleCallback(IAsyncResult AR)
//        {
            
//            Socket current = (Socket)AR.AsyncState;
//            Socket[] allClients = new Socket[clientSockets.Count];
            
//            int received;
            
//                try
//                {
//                    received = current.EndReceive(AR);
//                }
//                catch (SocketException)
//                {
//                    Console.WriteLine("Client forcefully disconnected");
//                    current.Close();
//                    clientSockets.Remove(current);
//                    return;
//                }
            

//            byte[] recBuf = new byte[received];
//            Array.Copy(buffer, recBuf, received);
//            string text = Encoding.ASCII.GetString(recBuf);
//            Console.WriteLine("Received Text: " + text);
            
//                if (text.ToLower() == "get time")
//                {
//                    Console.WriteLine("Text is a get time request");
//                    byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
//                    current.Send(data);
//                    Console.WriteLine("Time sent to client");
//                return;

//                }
//                else if (text.ToLower() == "exit")
//                {
//                    current.Shutdown(SocketShutdown.Both);
//                    current.Close();
//                    clientSockets.Remove(current);
//                    Console.WriteLine("Client disconnected");
//                    return;
//                }
//                else
//                {
//                    Console.WriteLine(text);
//                    byte[] data = Encoding.ASCII.GetBytes(text);
//                    current.Send(data);
//                    Console.WriteLine("Text Sent");
//                return;

//                }
                
            
//        }
//        }
//}
