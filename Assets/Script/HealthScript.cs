using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public GameObject WhatIsGunner;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = WhatIsGunner.transform.position;
    }
}
