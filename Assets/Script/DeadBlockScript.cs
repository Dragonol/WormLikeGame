﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBlockScript : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gunner"))
            collision.GetComponent<GunnerController>().HealthPoint = 0;
    }
}
