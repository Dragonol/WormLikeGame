using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyNetworkScript: NetworkLobbyManager
{
    public GameObject[] StartPositions;

    private int numPlayer;
    private int currentPlayer;

    private void Start()
    {
        numPlayer = maxPlayers;
        currentPlayer = 0;
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return Instantiate(lobbyPlayerPrefab, StartPositions[currentPlayer].transform.position, Quaternion.identity).gameObject;
    }
}
