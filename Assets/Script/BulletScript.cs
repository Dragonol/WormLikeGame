using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Texture2D BulletShape;
    [HideInInspector]
    public float Radius;
    [HideInInspector]
    public bool IsTrigged;
    // Start is called before the first frame update
    void Start()
    {
        IsTrigged = false;
        Radius = Mathf.Max((BulletShape.width / 2 + 1) / 100f, (BulletShape.height / 2 + 1) / 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
