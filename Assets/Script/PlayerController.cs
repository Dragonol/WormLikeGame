using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Bullet;
    public float ForcePerSecondSpaceHold;
    public float AnglePerSecondControlling;
    public float Speed;

    private float Angle;
    private float DeltaTimeBullet;
    private float DeltaTimeAngle;
    private float AngleMul;
    private Rigidbody2D PlayerRB2D;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRB2D = GetComponent<Rigidbody2D>();
        Angle = 45;
        AngleMul = 1;
        DeltaTimeBullet = 0;
        RaiseFriction();
    }

    // Update is called once per frame
    void Update()
    {
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
        else if(Input.GetKey(KeyCode.Space))
        {
            DeltaTimeBullet += Time.deltaTime;
            print(DeltaTimeBullet);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            DeltaTimeAngle += Time.deltaTime;
            Angle += (Time.deltaTime * AnglePerSecondControlling) * AngleMul;
            AngleMul += 0.03f;
            print(Angle);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DeltaTimeAngle += Time.deltaTime;
            Angle -= (Time.deltaTime * AnglePerSecondControlling) * AngleMul;
            AngleMul += 0.03f;
            print(Angle);
        }
        
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            RaiseFriction();
            PlayerRB2D.velocity = new Vector2(0, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (DeltaTimeBullet != 0)
            {
                GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
                float force = DeltaTimeBullet * ForcePerSecondSpaceHold;
                float angleInRadian = Mathf.Deg2Rad * Angle;
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angleInRadian),
                                                                        force * Mathf.Sin(angleInRadian)));
                DeltaTimeBullet = 0;
            }
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (DeltaTimeAngle * AnglePerSecondControlling < 1)
                Angle = Angle - DeltaTimeAngle * AnglePerSecondControlling + 1;
            DeltaTimeAngle = 0;
            AngleMul = 1;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (DeltaTimeAngle * AnglePerSecondControlling < 1)
                Angle = Angle + DeltaTimeAngle * AnglePerSecondControlling - 1;
            DeltaTimeAngle = 0;
            AngleMul = 1;
        }
    }

    private void FixedUpdate()
    {

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
