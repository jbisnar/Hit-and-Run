using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI_MissDrone : MonoBehaviour
{
    GameObject player;
    float aggroRange = 15f;
    float disengRange = 16f;
    Enemy_Movement movement;
    bool canAttack = true;
    bool canMove = true;
    bool lineOfSight;
    bool unbrokenAim = false;
    bool justReloaded = false;
    float aimTime = 1.5f;
    float reloadTime = 3f;
    Vector3 toPlayer;
    public GameObject Missile;
    public LayerMask playerLayer;
    public LayerMask GroundAndPlayer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<Enemy_Movement>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        toPlayer = player.transform.position - transform.position;
        if (canMove && ((Vector2)toPlayer).magnitude < aggroRange)
        {
            GetComponent<Enemy_Movement>().directionF = (Vector2)(path.vectorPath[currentWaypoint] - transform.position).normalized;
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < .5f)
        {
            currentWaypoint++;
        }

        var hit = Physics2D.Raycast(transform.position, toPlayer, Mathf.Infinity, GroundAndPlayer);
        if (hit.transform != null && hit.transform.gameObject.layer == 8)
        {
            GetComponent<Enemy_Movement>().directionF = Vector2.zero;
            if ((!lineOfSight && canAttack) || justReloaded)
            {
                unbrokenAim = true;
                StartCoroutine("Aim", aimTime);
            }
            lineOfSight = true;
        }
        else
        {
            lineOfSight = false;
            unbrokenAim = false;
        }
        justReloaded = false;
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && toPlayer.magnitude < aggroRange)
        {
            seeker.StartPath(transform.position, player.transform.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public IEnumerator Aim(float delay)
    {
        Debug.Log("Starting Aim");
        yield return new WaitForSeconds(delay);
        if (unbrokenAim && canAttack)
        {
            //Spawn missile
            GameObject spawnedMiss = GameObject.Instantiate(Missile, transform.position, transform.rotation);
            spawnedMiss.transform.parent = null;
            spawnedMiss.transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg), Vector3.forward);
            Debug.Log("Shooting missile");
            canAttack = false;
            StartCoroutine("Reload", reloadTime);
        }
    }

    public IEnumerator Reload(float delay)
    {
        canMove = false;
        yield return new WaitForSeconds(delay);
        //Spawn missile
        canAttack = true;
        canMove = true;
        justReloaded = true;
    }
}
