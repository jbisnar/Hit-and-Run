using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI_LaserDrone : MonoBehaviour
{
    GameObject player;
    LineRenderer lineR;
    float aggroRange = 15f;
    float disengRange = 16f;
    float fireRange = 5f;
    float DPS = 40f;
    float laserRad = .5f;
    float laserOffset = 1f;
    float range;
    float abovePlayerOffset = 6f;
    Vector3 toPlayer;
    Enemy_Movement movement;
    int state = 0; //0 = Idle, 1 = Pursuit, 2 = On/Off
    bool laserOn = false;
    public LayerMask ground;
    public LayerMask playerLayer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<Enemy_Movement>();
        lineR = GetComponent<LineRenderer>();
        lineR.positionCount = 2;
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
            GetComponent<Enemy_Movement>().directionF = Vector2.zero;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        if (((Vector2)toPlayer).magnitude < aggroRange)
        {
            GetComponent<Enemy_Movement>().directionF = (Vector2)(path.vectorPath[currentWaypoint] - transform.position).normalized;
        }
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < .5f)
        {
            currentWaypoint++;
        }

        toPlayer = player.transform.position - transform.position;

        var visual = Physics2D.Raycast(transform.position + new Vector3(0, -laserOffset, 0), Vector2.down, Mathf.Infinity, ground);
        range = visual.distance;

        if (laserOn)
        {
            var hit = Physics2D.CircleCast(transform.position + new Vector3(0, -laserOffset, 0), laserRad, Vector2.down, range, playerLayer);
            if (hit.transform != null)
            {
                hit.transform.GetComponent<Melee>().TakeDamage(DPS * Time.deltaTime);
            }
        }
        lineR.SetPosition(0, transform.position + new Vector3(0, -laserOffset, 0));
        lineR.SetPosition(1, visual.point);
        if (state == 0)
        {
            if (toPlayer.magnitude < aggroRange)
            {
                state = 1;
            }
        }
        else if (state == 1)
        {
            if (toPlayer.x < fireRange)
            {
                state = 2;
                StartCoroutine("Charge", 2f);
            }
        }
        else
        {
            if (toPlayer.x > fireRange)
            {
                state = 1;
            }
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && toPlayer.magnitude < aggroRange)
        {
            seeker.StartPath(transform.position, player.transform.position + new Vector3(0,abovePlayerOffset,0), OnPathComplete);
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

    IEnumerator Charge(float delay)
    {
        lineR.startWidth = .05f;
        lineR.endWidth = .05f;
        lineR.startColor = Color.red;
        lineR.endColor = Color.red;
        yield return new WaitForSeconds(delay / 2);
        lineR.startColor = Color.clear;
        lineR.endColor = Color.clear;
        yield return new WaitForSeconds(delay / 4);
        lineR.startColor = Color.red;
        lineR.endColor = Color.red;
        yield return new WaitForSeconds(delay / 8);
        lineR.startColor = Color.clear;
        lineR.endColor = Color.clear;
        yield return new WaitForSeconds(delay / 16);
        lineR.startColor = Color.red;
        lineR.endColor = Color.red;
        yield return new WaitForSeconds(delay / 16);
        lineR.startColor = Color.clear;
        lineR.endColor = Color.clear;
        laserOn = true;
        if (state == 2)
        {
            StartCoroutine("Fire", 2f);
        }
        else
        {
            lineR.startWidth = .05f;
            lineR.endWidth = .05f;
            lineR.startColor = Color.green;
            lineR.endColor = Color.green;
        }
    }

    IEnumerator Fire(float delay)
    {
        lineR.startWidth = laserRad*2;
        lineR.endWidth = laserRad*2;
        lineR.startColor = Color.red;
        lineR.endColor = Color.red;
        yield return new WaitForSeconds(delay);
        laserOn = false;
        if (state == 2)
        {
            StartCoroutine("Charge", 2f);
        }
        else
        {
            lineR.startWidth = .05f;
            lineR.endWidth = .05f;
            lineR.startColor = Color.green;
            lineR.endColor = Color.green;
        }
    }
}
