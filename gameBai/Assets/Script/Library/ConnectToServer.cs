
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ConnectToServer
{
    PlayerModel player = new PlayerModel();
    StreamWriter writer;

    StreamReader reader;
    TcpClient client = new TcpClient();
    public void Connect(string host, int port)
    {
        client.Connect(host, port);
        Stream stream = client.GetStream();
        Start(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;

        writer.WriteLine(JsonUtility.ToJson(player));


    }
    public void Start(Stream stream)
    {
        reader = new StreamReader(stream);
        Thread thread = new Thread(Room);
        thread.Start();
    }

    public void Room()
    {
        JsonUtility.FromJson<PlayerModel>(reader.ReadLine());
    }


    public void Send(PlayerModel player)
    {

        writer.WriteLine(JsonUtility.ToJson(player));
        Debug.Log(JsonUtility.ToJson(player));
    }
}
