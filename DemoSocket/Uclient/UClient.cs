using System;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Uclient
{
    public class UClient
    {
        TcpClient client = new TcpClient();
        Stream stream;
        StreamReader reader;
        StreamWriter writer;
        PlayerModel playerData;
        public void Connect(string host, int port)
        {
            client.Connect(host, port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
        }
        public void Loggin(PlayerModel player)
        {
            string data = JsonConvert.SerializeObject(player);
            writer.WriteLine(data);
        }
        public void Getdata(out PlayerModel player)
        {
            player = playerData;
            playerData = new PlayerModel();
        }
        /// <summary>
        /// gọi ở hàm start
        /// </summary>
        public void StartEvent()
        {
            Thread thread = new Thread(Run);
        }
        private void Run()
        {
            while (true)
            {
                
            }
        }
    }
}
