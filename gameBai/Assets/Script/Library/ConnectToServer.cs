using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ConnectToServer
{
    public PlayerModel player = new PlayerModel();
    StreamWriter writer;
    StreamReader reader;
    TcpClient client = new TcpClient();
    List<UServer> serverData = new List<UServer>();
    PlayerModel playerData = new PlayerModel();
    private bool connected;
    public bool isNew = false;
    public bool Connected { get => client.Connected; }

    public void Connect(string host, int port)
    {
        client.Connect(host, port);
        Stream stream = client.GetStream();
        Start(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        //writer.WriteLine(JsonUtility.ToJson(player));
        Thread thread = new Thread(DoBackground);
        thread.Start();
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
        try
        {
            this.player = player;
            writer.WriteLine(JsonUtility.ToJson(player));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }

    }
    private UServer getValue(string key)
    {
        if (key == null)
        {
            return new UServer();
        }
        foreach (var item in serverData)
        {
            if (item.key == key)
            {
                return item;
            }
        }
        return new UServer();
    }
    private void AddOrUpdate(UServer server)
    {
        if (server == null || server.key == "")
        {
            return;
        }
        for (int i = 0; i < serverData.Count; i++)
        {
            try
            {
                if (serverData[i].key == server.key)
                {
                    serverData[i].value = server.value;
                    return;
                }
            }
            catch (System.Exception)
            {
                Debug.Log("lỗi lưu key");
            }
        }
        //Debug.Log("đã lưu key:"+ server.value);
        serverData.Add(server);
    }
    private void DoBackground()
    {
        while (true)
        {
            try
            {
                //Debug.Log("run");
                string data = reader.ReadLine();
                // Debug.Log(data);
                UServer _serverData = JsonUtility.FromJson<UServer>(data);
                AddOrUpdate(_serverData);
                //playerData = JsonUtility.FromJson<PlayerModel>(data);
                isNew = true;
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                if (!client.Connected)
                {
                    break;
                }
            }
            //isNew = true;
        }
        Debug.Log("stop");
    }

    public PlayerModel GetPlayerModel()
    {
        if (playerData != default)
        {
            PlayerModel temp = playerData;
            playerData = default;
            return temp;
        }
        return null;
    }
    public UServer GetUServer(string key)
    {
        UServer temp = getValue(key);
        isNew = false;
        //Debug.Log(temp.value);
        return temp;
    }
    public void Disconnected()
    {
        writer.Close();
        reader.Close();
        client.Close();
        client.Dispose();
    }
}
