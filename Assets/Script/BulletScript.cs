using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float blastRadius;
    private float speed;
    private int damage;

    private bool isHitted;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2d;
    private Vector2 direction;

    [HideInInspector]
    public GameObject Owner;

    public LayerMask WhatIsTerrain;
    public LayerMask WhatIsGunner;

    [HideInInspector]
    public float BlastRadius { get => blastRadius; set => blastRadius = value; }
    [HideInInspector]
    public bool IsHitted { get => isHitted; set => isHitted = value; }
    [HideInInspector]
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
    [HideInInspector]
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
    public int Damage { get => damage; set => damage = value; }

    void Start()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        IsHitted = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collider2d.IsTouchingLayers(WhatIsTerrain) || 
            collider2d.IsTouchingLayers(WhatIsGunner) && collision.tag != "ControlPlayer")
        {
            Explose();
            Destroy(gameObject);
        }
    }

    private void Explose()
    {
        Collider2D[] hitTerrains = Physics2D.OverlapCircleAll(transform.position, BlastRadius, WhatIsTerrain);
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, BlastRadius, WhatIsGunner);

        List<Collider2D> listHitPlayer = new List<Collider2D>(hitPlayers);
        foreach(Collider2D hitPlayer in listHitPlayer)
        {
            if (hitPlayer == Owner.GetComponent<Collider2D>())
            {
                listHitPlayer.Remove(hitPlayer);
                break;
            }
        }
        hitPlayers = listHitPlayer.ToArray();

        foreach(Collider2D hitTerrain in hitTerrains)
        {
            hitTerrain.GetComponent<TerrainScript>().MakeAHole(BlastRadius, transform.position);
        }
        foreach (Collider2D hitPlayer in hitPlayers)
        {
            hitPlayer.GetComponent<GunnerController>().HealthPoint -= Damage;
            //print(hitPlayer.name);
        }
    }
}
