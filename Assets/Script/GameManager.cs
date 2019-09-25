using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public Canvas DisplayResult;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DisplayResult = GameObject.FindWithTag("Result").GetComponent<Canvas>();
        DisplayResult.enabled = false;

    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void GameOver()
    {
        DisplayResult.enabled = true;
        if(NetworkServer.connections.Count == 1)
        {
            DisplayResult.GetComponentInChildren<Text>().text = "You Win!";
        }
    }
}
