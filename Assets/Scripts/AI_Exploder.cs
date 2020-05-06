using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Exploder : MonoBehaviour
{
    GameObject player;
    float aggroRange = 8f;
    float attackRange = 2f;
    bool attacking = false;
    float disengRange = 10f;
    Enemy_Movement movement;
    Vector3 toPlayer;
    public GameObject telegraph;
    public GameObject explosion;
    float expRange = 4f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<Enemy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        toPlayer = transform.position - player.transform.position;
        if (toPlayer.magnitude < attackRange && !attacking)
        {
            movement.direction = 0;
            attacking = true;
            StartCoroutine("Detonate", 2f);
        }
        else if (toPlayer.magnitude < aggroRange && !attacking)
        {
            movement.direction = -(int)toPlayer.x;
        }
        if (toPlayer.magnitude > disengRange)
        {
            movement.direction = 0;
        }
        if (Physics2D.Raycast(transform.position - new Vector3(0, movement.height), new Vector2(movement.direction, 0), .5f, movement.layerGround) && !attacking)
        {
            movement.jumping = true;
        }
        else
        {
            movement.jumping = false;
        }
    }

    IEnumerator Detonate(float delay)
    {
        GameObject spawnedTele = GameObject.Instantiate(telegraph, transform.position, transform.rotation);
        spawnedTele.transform.localScale = Vector3.one * expRange;
        spawnedTele.transform.parent = transform;
        spawnedTele.transform.localPosition = Vector3.zero;
        //spawnedTele.GetComponent<Exp_Tele>().liveTime = delay;
        spawnedTele.GetComponent<Exp_Tele>().StartCoroutine("Flash",delay);
        yield return new WaitForSeconds(delay);
        GameObject spawnedExp = GameObject.Instantiate(explosion, transform.position, transform.rotation);
        spawnedExp.transform.localScale = Vector3.one * expRange;
        spawnedExp.GetComponent<Proj_EExplosion>().damage = 10f;
        spawnedExp.transform.parent = null;
        attacking = false;
    }
}
