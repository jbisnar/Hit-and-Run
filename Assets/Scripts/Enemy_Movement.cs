using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public int type; //0 = Grounded, 1 = Flying

    public float accelH;
    public float velH;
    public float grav;
    public float velJump;
    public bool grounded;
    public int direction;
    public bool jumping;
    public float height;

    public float accelF;
    public float accelSlowF;
    public float velF;
    public Vector2 directionF;

    public LayerMask layerGround;
    public Vector2 temp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        temp = transform.GetComponent<Rigidbody2D>().velocity;

        grounded = Physics2D.Raycast(transform.position, Vector2.down, height + .02f, layerGround);

        if (type == 0)
        {
            if (direction == 0)
            {
                if (Mathf.Abs(temp.x) < accelH * Time.deltaTime)
                {
                    temp.x = 0;
                }
                else
                {
                    temp.x += accelH * Time.deltaTime * Mathf.Sign(-temp.x);
                }
            }
            else
            {
                if (Mathf.Abs(temp.x) > velH)
                {
                    temp.x = velH * Mathf.Sign(direction);
                }
                else
                {
                    temp.x += accelH * Time.deltaTime * Mathf.Sign(direction);
                }
            }

            if (grounded)
            {
                temp.y = 0;
                if (jumping)
                {
                    temp.y = velJump;
                }
            }
            else
            {
                temp.y -= grav * Time.deltaTime;
            }
        }
        else
        {
            temp = (temp.magnitude - accelSlowF * Time.deltaTime) * temp.normalized;
            temp += directionF.normalized * accelF * Time.deltaTime;
            if (temp.magnitude > velF)
            {
                temp = temp.normalized * velF;
            }
        }

        transform.GetComponent<Rigidbody2D>().velocity = temp;
    }
}
