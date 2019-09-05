using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunnerController : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject HealthBar;

    
    [SerializeField]
    private int maxHealthPoint;
    private int healthPoint;
    private float healthRatio;
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
    private float NewHorizontalVelocity;
    private float InputVertical;
    private float NewVerticalVelocity;
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
            HealthBar.transform.localScale = new Vector3((float)healthPoint / maxHealthPoint * healthRatio,
                                                         HealthBar.transform.localScale.y,
                                                         HealthBar.transform.localScale.z);
        }
    }
    

    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        IsGrounded = CircleCollider2D.IsTouchingLayers(WhatIsTerrain);
        healthRatio = HealthBar.transform.localScale.x;
        HealthPoint = maxHealthPoint;
    }

    void FixedUpdate()
    {
        CmdProcessMoving();
    }
    void Update()
    {
        CmdProcessAction();
    }
    void Flip()
    {
        FacingRight = !FacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    private void Shoot(Vector2 origin, Vector2 direction, float blastRadius, float speed)
    {
        BulletScript bullet = Instantiate(Bullet, origin, Quaternion.identity).GetComponent<BulletScript>();
        bullet.Direction = direction;
        bullet.Speed = speed;
        bullet.BlastRadius = blastRadius;
        bullet.Damage = Damage;
        bullet.Owner = gameObject;
    }
    void CmdProcessMoving()
    {
        IsGrounded = CircleCollider2D.IsTouchingLayers(WhatIsTerrain);
        InputVertical = Input.GetAxis("Vertical");
        InputHorizontal = Input.GetAxis("Horizontal");
        NewHorizontalVelocity = InputHorizontal * HorizontalSpeed;
        NewVerticalVelocity = (IsGrounded && InputVertical > 0) ? JumpForce : PlayerRB2D.velocity.y;

        if (InputHorizontal < 0 && FacingRight)
            Flip();
        else if (InputHorizontal > 0 && !FacingRight)
            Flip();

        PlayerRB2D.velocity = new Vector2(NewHorizontalVelocity, NewVerticalVelocity);
    }
    void CmdProcessAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            Shoot(transform.position, direction, BulletRadius, BulletSpeed);
        }
    }
    
}
