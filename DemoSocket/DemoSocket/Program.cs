using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using DemoSocket.Models;


namespace DemoSocket
{
    class Program
    {
        public static Dictionary<int, Room> inRooms = new Dictionary<int, Room>();
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        static Socket socket;
        [Obsolete]
        static void Main(string[] args)
        {
            int count = 0;
            TcpListener listener = new TcpListener(8080);
            listener.Start();
            Console.WriteLine("Dm da bat dau server tai:" + listener.LocalEndpoint);
            Console.WriteLine("dang cho ket noi......");
            while (true)
            {
                socket = listener.AcceptSocket();
                Thread thread = new Thread(wait);
                thread.Start();
                //Console.WriteLine(socket.RemoteEndPoint + " da ket noi");
                //Client client = new Client();
                //Stream stream = new NetworkStream(socket);
                //StreamReader reader = new StreamReader(stream);                
                //string name = reader.ReadLine();
                //PlayerModel player = JsonConvert.DeserializeObject<PlayerModel>(name);
                //Console.WriteLine(name);
                //client.Start(player.ID_player, socket);
                //clients.Add(player.ID_player, client);
                count++;
            }

            //StreamWriter writer = new StreamWriter(stream);
            //writer.AutoFlush = true;            
        }
        private static void wait()
        {
            Socket socketTemp = socket;
            try
            {
                Console.WriteLine(socketTemp.RemoteEndPoint + " da ket noi");
                Client client = new Client();
                //Stream stream = new NetworkStream(socketTemp);
                ////stream.ReadTimeout = 10000;
                //StreamReader reader = new StreamReader(stream);
                //string name = reader.ReadLine();
                //Console.WriteLine(name);
                //PlayerModel player = JsonConvert.DeserializeObject<PlayerModel>(name);                
                client.Start(0, socket);
                //clients.Add(player.ID_player, client);
                //Console.Write(" "+player.ID_player);
            }
            catch (Exception)
            {

                Console.WriteLine("Timeout:");
            }
        }
        //lưu lỗi
        public static void ErrorLogs(object e)
        {
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }
            if (!File.Exists("logs/error.txt"))
            {
                File.Create("logs/error.txt").Close();
            }
            using (StreamReader reader = new StreamReader("logs/error.txt"))
            {
                string data = reader.ReadToEnd() + "\n";
                reader.Close();
                StreamWriter writer = new StreamWriter("logs/error.txt");
                data += DateTime.Now.ToString() + "\n" + e + "\n";
                writer.AutoFlush = true;
                writer.WriteLine(data);
                writer.Close();
                //Console.WriteLine(data);
            }
        }
        //lưu các log lại
        public static void Logs(object e)
        {
            try
            {
                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }
                if (!File.Exists("logs/logs.txt"))
                {
                    File.Create("logs/logs.txt").Close();
                }
                using (StreamWriter writer = new StreamWriter("logs/logs.txt"))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(DateTime.Now);
                    writer.WriteLine(e);
                    writer.Close();
                }
            }
            catch (Exception)
            {

            }
        }
    }
    class Client
    {
        int ID;
        int id_room;
        Socket socket;
        Stream stream;
        StreamReader reader;
        StreamWriter writer;
        public void Start(int ID, Socket socket)
        {
            this.ID = ID;
            this.socket = socket;
            stream = new NetworkStream(this.socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            stream.ReadTimeout = 10000;
            writer.AutoFlush = true;
            if (Program.clients.Count(e => e.Key == ID) > 0)
            {
                writer.WriteLine("tài khoản đã được đăng nhập");
                socket.Close();
                return;
            }
            Thread thread = new Thread(DoInBackground);
            thread.Start();
            //Send(ID, "đã tham gia phòng");

        }
        public void Send(string input)
        {
            writer.WriteLine(input);
        }
        /// <summary>
        /// gửi dữ liệu cho tất cả người chơi k có trong phòng
        /// </summary>
        /// <param name="nhan"></param>
        public void SendAll(string nhan)
        {
            foreach (var item in Program.clients)
            {
                if (item.Key != ID)
                {
                    item.Value.Send(nhan);
                }
            }
        }
        //gửi dữ liệu cho tất cả ngươi chơi trong phòng
        public void SendAllRoom(int id_Room, string nhan)
        {
            try
            {
                List<int> players = Program.inRooms.Where(e => e.Key == id_Room).FirstOrDefault().Value.listPlayer;
                if (players == null)
                {
                    return;
                }
                foreach (var item in players)
                {
                    //Console.WriteLine("đã gửi cho:"+ item);
                    Program.clients[item].Send(nhan);
                }
            }
            catch (Exception)
            {
                //hổng có gì hết
            }
           
        }
        //gửi dữ liệu cho tất cả ngươi chơi trong phòng
        public void SendAllNotInRoom(string nhan)
        {            
            foreach (var item in Program.clients)
            {
                if (!Program.inRooms.ContainsKey(item.Value.id_room))
                {
                    item.Value.Send(nhan);
                }
            }
        }
        private void DoInBackground()
        {
            DatabaseGameBaiEntities db = new DatabaseGameBaiEntities();
            try
            {
                while (true)
                {
                    string nhan = reader.ReadLine();                   
                    if (nhan.ToLower() == "cmd_exit")
                    {
                        //var client = new RestClient("");
                        Console.WriteLine("Disconnect " + socket.RemoteEndPoint);
                        socket.Close();
                        break;
                    }
                    PlayerModel playerModel = JsonConvert.DeserializeObject<PlayerModel>(nhan);
                    UServer server = new UServer();
                    room_list room = db.room_list.Where(e => e.id == id_room).FirstOrDefault();
                    switch (playerModel.cmd.ToLower())
                    {
                        //làm mới lại danh sách phòng chơi
                        case "refresh_listroom":
                            server.ID = ID;
                            server.key = "refresh_listroom";
                            server.value = JsonConvert.SerializeObject(playerModel);
                            SendAllNotInRoom(JsonConvert.SerializeObject(server));
                            break;
                        case "join_room":
                            if (Program.inRooms.ContainsKey(playerModel.ID_room))
                            {
                                Program.inRooms[playerModel.ID_room].listPlayer.Add(ID);
                                Console.WriteLine(ID + " da vào phong " + playerModel.ID_room);
                            }
                            else
                            {
                                //List<int> players = new List<int>();
                                Program.inRooms.Add(playerModel.ID_room, new Room(new List<int>() { ID}));
                                Console.WriteLine(ID + " da tao phong " + playerModel.ID_room);
                            }
                            server.ID = ID;
                            server.key = "join_room";
                            server.value = JsonConvert.SerializeObject(playerModel);
                            id_room = playerModel.ID_room;
                            SendAllRoom(playerModel.ID_room, JsonConvert.SerializeObject(server));
                            
                            server.key = "refresh_listroom";
                            server.value = JsonConvert.SerializeObject(playerModel);
                            SendAllNotInRoom(JsonConvert.SerializeObject(server));
                            break;
                        //case "update":
                        //    server.ID = ID;
                        //    server.key = "update";
                        //    server.value = JsonConvert.SerializeObject(playerModel);
                        //    SendAllRoom(id_room, JsonConvert.SerializeObject(server));
                        //    break;
                        case "leave_room":
                            if (Program.inRooms.ContainsKey(playerModel.ID_room))
                            {
                                Program.inRooms[playerModel.ID_room].listPlayer.Remove(playerModel.ID_player);
                                server.ID = ID;
                                                               
                                server.value = JsonConvert.SerializeObject(playerModel);
                                id_room = playerModel.ID_room;

                                if (Program.inRooms[playerModel.ID_room].isPlaying)
                                {
                                    db.truTien(room.bet_money * room.current_player, playerModel.ID_player);
                                    db.SaveChanges();
                                    foreach (var item in Program.inRooms[id_room].listPlayer)
                                    {
                                        db.congTien(room.bet_money +(int)(room.bet_money * 10 /100), item);
                                        db.SaveChanges();
                                        player player = db.players.Where(e => e.id == item).FirstOrDefault();                                        
                                    }
                                    server.key = "end_game";                                                                       
                                }
                                else
                                {
                                    server.key = "leave_room";
                                }
                                SendAllRoom(playerModel.ID_room, JsonConvert.SerializeObject(server));

                                server.key = "refresh_listroom";
                                server.value = JsonConvert.SerializeObject(playerModel);
                                SendAllNotInRoom(JsonConvert.SerializeObject(server));
                            }
                            if (Program.inRooms[playerModel.ID_room].listPlayer.Count < 1)
                            {
                                Program.inRooms.Remove(playerModel.ID_room);
                            }
                            break;
                        case "login":
                            if (!Program.clients.ContainsKey(ID))
                            {
                                Console.WriteLine("login succses!: " + socket.RemoteEndPoint);
                                ID = playerModel.ID_player;
                                Program.clients.Add(ID, this);
                                Console.WriteLine(ID + ":" + nhan);
                                //Console.WriteLine("pos:" + Program.clients[ID].ID);
                            }
                            server.ID = ID;
                            server.key = "succses";
                            server.value = "true";
                            Send(JsonConvert.SerializeObject(server));
                            //Console.WriteLine(JsonConvert.SerializeObject(server));
                            //SendAll(JsonConvert.SerializeObject(server));
                            stream.ReadTimeout = -1;
                            break;
                        case "chat_all":
                            server.ID = ID;
                            server.key = "chat_all";
                            server.value = JsonConvert.SerializeObject(playerModel);
                            Send(JsonConvert.SerializeObject(server));
                            SendAll(JsonConvert.SerializeObject(server));
                            break;
                        case "checkroom":                           
                            server.ID = ID;
                            server.key = "checkroom";
                            server.value = "true";
                            Send(JsonConvert.SerializeObject(server));
                            //Console.WriteLine(JsonConvert.SerializeObject(server));
                            //SendAll(JsonConvert.SerializeObject(server));                            
                            break;
                        case "win":
                            if (Program.inRooms[id_room].isPlaying)
                            {
                                db.congTien(room.bet_money * room.current_player, playerModel.ID_player);
                                db.SaveChanges();
                            }                           
                            server.ID = ID;
                            server.key = playerModel.cmd.ToLower();
                            server.value = JsonConvert.SerializeObject(playerModel);
                            SendAllRoom(id_room, JsonConvert.SerializeObject(server));

                            Program.inRooms[playerModel.ID_room].isPlaying = false;

                            //List<PlayerMoneyModel> listMoneywin = new List<PlayerMoneyModel>();
                            //foreach (var item in Program.inRooms[id_room].listPlayer)
                            //{
                            //    player player = db.players.Where(e => e.id == item).FirstOrDefault();
                            //    listMoneywin.Add(new PlayerMoneyModel(player.id, player.money));
                            //}
                            DataPackedPlayerMoney dataPackedWin = new DataPackedPlayerMoney();
                            //dataPackedWin.message = listMoneywin;
                            dataPackedWin.sumMoneyWin = (double)(room.bet_money * room.current_player);
                            
                            server.ID = ID;
                            server.key = "update";
                            server.value = JsonConvert.SerializeObject(dataPackedWin);
                            SendAllRoom(id_room, JsonConvert.SerializeObject(server));
                            break;
                            
                        case "start_game":                   
                            if (ID == room.owner_id && Program.inRooms.ContainsKey(id_room))
                            {
                                List<PlayerMoneyModel> listMoney = new List<PlayerMoneyModel>();
                                foreach (var item in Program.inRooms[id_room].listPlayer)
                                {
                                    db.truTien(room.bet_money, item);
                                    db.SaveChanges();
                                    player player =  db.players.Where(e => e.id == item).FirstOrDefault();
                                    listMoney.Add(new PlayerMoneyModel(player.id, player.money));                                    
                                }
                                //playerModel.message = JsonConvert.SerializeObject(listMoney);
                                server.ID = ID;
                                server.key = playerModel.cmd.ToLower();
                                server.value = JsonConvert.SerializeObject(playerModel);
                                SendAllRoom(id_room, JsonConvert.SerializeObject(server));
                                Program.inRooms[playerModel.ID_room].isPlaying = true;
                                //cập nhập tiền của player
                                DataPackedPlayerMoney dataPacked = new DataPackedPlayerMoney();
                                dataPacked.message = listMoney;                                

                                server.ID = ID;
                                server.key = "update";
                                server.value = JsonConvert.SerializeObject(dataPacked);
                                SendAllRoom(id_room, JsonConvert.SerializeObject(server));
                            }
                            break;                        
                        default:
                            server.ID = ID;
                            server.key = playerModel.cmd.ToLower();
                            server.value = JsonConvert.SerializeObject(playerModel);
                            if (playerModel.cmd.StartsWith("all_"))
                            {
                                SendAll(JsonConvert.SerializeObject(server));
                            }
                            else if(playerModel.cmd.StartsWith("lby_"))
                            {
                                SendAllNotInRoom(JsonConvert.SerializeObject(server));
                            }
                            else
                            {
                                SendAllRoom(id_room, JsonConvert.SerializeObject(server));
                            }                                                  
                            break;
                    }
                    //SendAll(nhan);
                    Program.Logs(JsonConvert.SerializeObject(Program.inRooms));
                    //writer.WriteLine("hello " + nhan);
                }
            }
            catch (Exception e)
            {
                Program.ErrorLogs(e);

                Program.clients.Remove(ID);
                Program.inRooms.Remove(id_room);
                Console.WriteLine("Disconnect " + socket.RemoteEndPoint);               
                if (socket.Connected)
                {
                    socket.Close();
                    Console.WriteLine("stop:" + ID);
                }
                Program.Logs(JsonConvert.SerializeObject(Program.inRooms));
            }
        }
    }
}

