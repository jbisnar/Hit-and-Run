using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Runner : MonoBehaviour
{
    GameObject player;
    float aggroRange = 5f;
    float disengRange = 10f;
    Enemy_Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<Enemy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((Vector2)transform.position - (Vector2)player.transform.position).magnitude < aggroRange)
        {
            movement.direction = Mathf.CeilToInt((transform.position - player.transform.position).x);
        }
        if ((transform.position - player.transform.position).magnitude > disengRange)
        {
            movement.direction = 0;
        }
        if (Physics2D.Raycast(transform.position - new Vector3(0,movement.height),new Vector2(movement.direction, 0), 1f, movement.layerGround))
        {
            movement.jumping = true;
        }
        else
        {
            movement.jumping = false;
        }
    }
}
