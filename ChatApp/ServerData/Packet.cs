using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace ServerData
{
    [Serializable]
    public class Packet
    {
        public List<string> Data;
        public int packetInt;
        public bool packetBool;
        public string senderId;
        public PacketType packetType;

        public Packet(PacketType type, string senderId)
        {
            Data = new List<string>();
            this.senderId = senderId;
            this.packetType = type;
        }

        public Packet(byte[] packetBytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetBytes);

            Packet packet = (Packet)bf.Deserialize(ms);
            ms.Close();
            Data = packet.Data;
            packetInt = packet.packetInt;
            packetBool = packet.packetBool;
            senderId = packet.senderId;
            packetType = packet.packetType;
        }

        public byte[] toBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, this);
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }


        public static string getIp4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress i in ips)
            {
                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return i.ToString();
                }
            }
            return "127.0.0.1";
        }
    }

    public enum PacketType
    {
        Registration,
        chat
    }
}
