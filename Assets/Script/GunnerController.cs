﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunnerController : NetworkBehaviour
{
    public GameObject Bullet;
    public GameObject GameManager;

    [SerializeField]
    private int maxHealthPoint;
    private int healthPoint;
    private float healthRatio;
    private HealthScript healthScript;
    [SerializeField]
    private int damage;

    public float HorizontalSpeed;
    public float JumpForce;
    public float BulletSpeed;
    public float BulletRadius;
    public LayerMask WhatIsTerrain;
    
    public bool IsGrounded;

    private Rigidbody2D PlayerRB2D;
    private Collider2D Collider;

    private float InputHorizontal;
    private float InputVertical;
    private bool FacingRight;
    public int Damage
    {
        get => damage;
        set
        {
            damage = Mathf.Max(value, 0);
        }
    }
    public int HealthPoint
    {
        get => healthPoint;
        set
        {
            healthPoint = Mathf.Min(Mathf.Max(value, 0), maxHealthPoint);
            healthScript.FillRate = (float)healthPoint / maxHealthPoint;
            if (isLocalPlayer && healthPoint == 0) 
            {
                GameManager.GetComponent<GameManager>().GameOver();
                Destroy(gameObject);
            }
        }
    }

    private void Awake()
    {
        GameManager = GameObject.Find("NetworkManager");
    }

    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        healthScript = transform.GetChild(0).GetComponent<HealthScript>();

        
        HealthPoint = maxHealthPoint;

        if (isLocalPlayer)
            Camera.main.GetComponent<CameraController>().TrackingObject = gameObject;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        GroundCheck();
        InputVertical = Input.GetAxis("Vertical");
        InputHorizontal = Input.GetAxis("Horizontal");
        float newHorizontalVelocity = InputHorizontal * HorizontalSpeed;
        float newVerticalVelocity = (IsGrounded && InputVertical > 0) ? JumpForce : PlayerRB2D.velocity.y;
        PlayerRB2D.velocity = new Vector2(newHorizontalVelocity, newVerticalVelocity);

    }
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            CmdShoot(mousePosition, direction);
        }
    }
    
    [Command]
    void CmdShoot(Vector3 mousePosition, Vector2 direction)
    {
        RpcShoot(transform.position, direction, BulletRadius, BulletSpeed);
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    [ClientRpc]
    private void RpcShoot(Vector2 origin, Vector2 direction, float blastRadius, float speed)
    {
        GameObject bullet = Instantiate(Bullet, origin, Quaternion.identity);
        BulletScript bulletScript = bullet.GetComponent<BulletScript>();

        bulletScript.Direction = direction;
        bulletScript.Speed = speed;
        bulletScript.BlastRadius = blastRadius;
        bulletScript.Damage = Damage;
        bulletScript.Owner = gameObject;
    }
    void GroundCheck()
    {
        bool isRayHit1 = Physics2D.Raycast(transform.position - new Vector3(Collider.bounds.size.x / 3, 0, 0), Vector2.down, 1, WhatIsTerrain);
        bool isRayHit2 = Physics2D.Raycast(transform.position + new Vector3(Collider.bounds.size.x / 3, 0, 0), Vector2.down, 1, WhatIsTerrain);
        bool isTouchTerrain = Collider.IsTouchingLayers(WhatIsTerrain);
        IsGrounded = (isRayHit1 || isRayHit2) && isTouchTerrain;
    }
}
