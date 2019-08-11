using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private Texture2D bulletShape;
    [SerializeField]
    private float blastRadius;
    [SerializeField]
    private float speed;

    private bool isHitted;
    private Rigidbody2D rigidbody2D;
    private Vector2 direction;

    public Texture2D BulletShape { get => bulletShape; set => bulletShape = value; }
    public float BlastRadius { get => blastRadius; set => blastRadius = value; }
    public bool IsHitted { get => isHitted; set => isHitted = value; }
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            if (rigidbody2D == null)
                rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = Direction * Speed;
        }
    }
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value.normalized;
            Speed = Speed;
        }
    }

    void Start()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        IsHitted = false;
        BlastRadius = Mathf.Max((BulletShape.width / 2 + 1) / 100f, (BulletShape.height / 2 + 1) / 100f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
