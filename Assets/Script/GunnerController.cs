using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunnerController : NetworkBehaviour
{
    public GameObject Bullet;

    public bool isLocal;

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
    [HideInInspector]
    public bool IsGrounded;


    private Rigidbody2D PlayerRB2D;
    private CircleCollider2D CircleCollider2D;

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
        }
    }


    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();
        healthScript = transform.GetChild(0).GetComponent<HealthScript>();

        IsGrounded = CircleCollider2D.IsTouchingLayers(WhatIsTerrain);
        HealthPoint = maxHealthPoint;
        isLocal = isLocalPlayer;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        IsGrounded = CircleCollider2D.IsTouchingLayers(WhatIsTerrain);
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
    void CmdMovement(Vector2 position)
    {
        RpcMove(position);
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
    void RpcMove(Vector2 position)
    {
        transform.position = position;

        if (InputHorizontal < 0 && FacingRight)
            Flip();
        else if (InputHorizontal > 0 && !FacingRight)
            Flip();

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

}
