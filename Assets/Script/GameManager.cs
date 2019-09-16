using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Canvas DisplayResult;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DisplayResult.enabled = false;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void Endgame(GameObject gameObject)
    {
        DisplayResult.enabled = true;
        Destroy(gameObject);
    }
}
