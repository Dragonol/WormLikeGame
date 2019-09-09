using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript: MonoBehaviour
{
    [SerializeField]
    GameObject Gunner;

    RoleStatus Role;
    bool IsGameStart;
    NetworkClient myClient;

    private void Awake()
    {
        Role = RoleStatus.Unset;
    }
    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            Role = RoleStatus.Host;
            IsGameStart = true;
        }
        else if (GUILayout.Button("Client"))
        {
            Role = RoleStatus.Client;
            IsGameStart = true;
        }
    }
    void Update()
    {
        if (!IsGameStart)
        {
            if (Role == RoleStatus.Host)
            {
                SetupHost();
            }
            else if (Role == RoleStatus.Client)
            {
                SetupClient();
            }
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

        NetworkServer.Spawn(Gunner);
        
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
        Debug.Log("Connected to: " + (NetworkServer.connections.Count - 1));
    }
    enum RoleStatus
    {
        Unset,
        Host,
        Client
    }
}
