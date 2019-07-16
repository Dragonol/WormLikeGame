using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Texture2D BulletShape;
    [HideInInspector]
    public float Radius;
    public bool IsTrigged;
    // Start is called before the first frame update
    void Start()
    {
        IsTrigged = false;
        Radius = Mathf.Max((BulletShape.width / 2 + 1) / 128f, (BulletShape.height / 2 + 1) / 128f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
