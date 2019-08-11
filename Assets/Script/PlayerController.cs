using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Bullet;
    public int HealthPoint;
    public float HorizontalSpeed;
    public float JumpForce;
    public bool IsGrounded;
    public bool IsJumpable;

    private Rigidbody2D PlayerRB2D;
    private CircleCollider2D CircleCollider2D;

    private LayerMask TerrainLayer;
    
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        TerrainLayer = LayerMask.GetMask("Terrain");

        IsGrounded = false;
        IsJumpable = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();

        // Moving horizontal
        if (Input.GetKey(KeyCode.D))
        {
            UnStop();
            PlayerRB2D.velocity = new Vector2(HorizontalSpeed, PlayerRB2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            UnStop();
            PlayerRB2D.velocity = new Vector2(-HorizontalSpeed, PlayerRB2D.velocity.y);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Stop();
            PlayerRB2D.velocity = new Vector2(0, PlayerRB2D.velocity.y);
        }

        
        // Moving vertical
        if (Input.GetKey(KeyCode.W) && IsGrounded && IsJumpable)
        {
            PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x, 0);
            IsJumpable = false;
            PlayerRB2D.AddForce(new Vector2(0, JumpForce));
        }

        // Shooting
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - transform.position;
            Shoot(transform.position, direction, 10, 10);
        }    
            
    }

    private void FixedUpdate()
    {

    }
    
    private void Stop()
    {
        PlayerRB2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    private void UnStop()
    {
        PlayerRB2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void UpdateValues()
    {
        IsGrounded = CircleCollider2D.IsTouchingLayers(TerrainLayer);

        if (!IsGrounded)
            IsJumpable = true;
    }
    private void Shoot(Vector2 origin, Vector2 direction, float blastRadius, float speed)
    {
        BulletScript bullet = Instantiate(Bullet, origin, Quaternion.identity).GetComponent<BulletScript>();
        bullet.Direction = direction;
        bullet.Speed = speed;
        bullet.BlastRadius = blastRadius;
    }
}
