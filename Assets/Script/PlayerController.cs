using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Bullet;
    public float Speed;
    public float JumpForce;
    public bool Grounded;

    private Rigidbody2D PlayerRB2D;
    private CircleCollider2D CircleCollider2D;

    private LayerMask TerrainLayer;
    
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        TerrainLayer = LayerMask.GetMask("Terrain");

        Grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGrounded();

        // Moving horizontal
        if (Input.GetKey(KeyCode.D))
        {
            UnStop();
            PlayerRB2D.velocity = new Vector2(Speed, PlayerRB2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            UnStop();
            PlayerRB2D.velocity = new Vector2(-Speed, PlayerRB2D.velocity.y);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            Stop();
            PlayerRB2D.velocity = new Vector2(0, PlayerRB2D.velocity.y);
        }

        // Moving vertical
        if (Input.GetKey(KeyCode.W) && Grounded)
        {
            PlayerRB2D.AddForce(new Vector2(0, JumpForce));
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
    private void UpdateGrounded()
    {
        Grounded = CircleCollider2D.IsTouchingLayers(TerrainLayer);

        // Set velocity to zero when touch ground
        if (Grounded)
            PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x, 0);
    }
}
