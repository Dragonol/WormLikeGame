using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private GameObject healthBar;
    private float maxHealthScale;
    private float fillRate;
    public float FillRate
    {
        get => fillRate;
        set
        {
            fillRate = value;
            healthBar.transform.localScale =
                new Vector3(fillRate * maxHealthScale, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }
    }

    void Awake()
    {
        healthBar = transform.GetChild(0).gameObject;
        maxHealthScale = healthBar.transform.localScale.x;
    }

    void Update()
    {
        
    }
}
