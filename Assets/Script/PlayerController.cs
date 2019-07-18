using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Bullet;
    public float ForcePerSecondSpaceHold;
    public float Speed;

    private float Angle;
    private float DeltaTime;
    private Rigidbody2D PlayerRB2D;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        Angle = 45;
        DeltaTime = 0;
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
        else if(Input.GetKey(KeyCode.W))
        {
            Angle++;
            print(Angle);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            Angle--;
            print(Angle);
        }
        else if(Input.GetKey(KeyCode.Space))
        {
            DeltaTime += Time.deltaTime;
            print(DeltaTime);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            LowFriction();
            PlayerRB2D.velocity = new Vector2(Speed, PlayerRB2D.velocity.y);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            LowFriction();
            PlayerRB2D.velocity = new Vector2(-Speed, PlayerRB2D.velocity.y);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            RaiseFriction();
            PlayerRB2D.velocity = new Vector2(0, 0);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (DeltaTime != 0)
            {
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                float force = DeltaTime * ForcePerSecondSpaceHold;
                float angleInRadian = Mathf.Deg2Rad * Angle;
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angleInRadian),
                                                                        force * Mathf.Sin(angleInRadian)));
                DeltaTime = 0;
            }
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
