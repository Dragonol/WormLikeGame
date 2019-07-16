using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    private Rigidbody2D PlayerRB2D;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        RaiseFriction();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            LowFriction();
            PlayerRB2D.velocity = new Vector2(Speed, PlayerRB2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            LowFriction();
            PlayerRB2D.velocity = new Vector2(-Speed, PlayerRB2D.velocity.y);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            RaiseFriction();
            PlayerRB2D.velocity = new Vector2(0, 0);
        }
    }
    void RaiseFriction()
    {
        PlayerRB2D.constraints = RigidbodyConstraints2D.FreezePositionX;
    }
    void LowFriction()
    {
        PlayerRB2D.constraints = RigidbodyConstraints2D.None;

    }
}
