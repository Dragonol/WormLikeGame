using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript: MonoBehaviour
{
    [SerializeField]
    RoleStatus Role;
    NetworkClient myClient;

    private void Awake()
    {
        Role = RoleStatus.Unset;
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
            Role = RoleStatus.Host;
        else if (GUILayout.Button("Client"))
            Role = RoleStatus.Client;
    }
    void Update()
    {
        if (Role == RoleStatus.Host)
        {
            SetupHost();
            Role = RoleStatus.Unset;
        }
        else if(Role == RoleStatus.Client)
        {
            SetupClient();
            Role = RoleStatus.Unset;
        }
    }

    bool SetupHost()
    {
        if (!NetworkServer.Listen(0904))
            return false;

        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);

        myClient = new NetworkClient();
        myClient.Connect("localhost", 0904);
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        
        Debug.Log(NetworkServer.connections.Count);
        return true;
    }
    
    bool SetupClient()
    {
        myClient = new NetworkClient();
        myClient.Connect("localhost",0904);
        myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
        return myClient.isConnected;
    }
    void OnClientConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
    void OnServerConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to: " + NetworkServer.connections.Count);
        foreach (var i in NetworkServer.connections)
        {
            if (i == null)
                Debug.Log("#");
            else
                Debug.Log(i.address);
        }
    }
    enum RoleStatus
    {
        Unset,
        Host,
        Client
    }
}
