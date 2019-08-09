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

    
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        CircleCollider2D = GetComponent<CircleCollider2D>();

        Grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    private void FixedUpdate()
    {
        UpdateGrounded();
        if (Input.GetKey(KeyCode.W) && Grounded)
        {
            PlayerRB2D.AddForce(new Vector2(0, JumpForce));
        }
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
        LayerMask layer = LayerMask.GetMask("Terrain");
        //var currPos = CircleCollider2D.bounds.center;
        //var colliderRadius = CircleCollider2D.radius;
        //float dist = 0.01f;
        //var ray1 = Physics2D.Raycast(currPos, new Vector2(0, -1), colliderRadius + dist, layer).collider;
        //var ray2 = Physics2D.Raycast(new Vector2(currPos.x - (colliderRadius/2), currPos.y), new Vector2(0, -1), Mathf.Sqrt(3) * colliderRadius/2 + dist, layer).collider;
        //var ray3 = Physics2D.Raycast(new Vector2(currPos.x + (colliderRadius/2), currPos.y), new Vector2(0, -1), Mathf.Sqrt(3) * colliderRadius / 2 + dist, layer).collider;

        //Grounded = ray1 || ray2 || ray3;

        if ((Grounded = CircleCollider2D.IsTouchingLayers(layer)))
            PlayerRB2D.velocity = new Vector2(PlayerRB2D.velocity.x, 0);
    }
}
