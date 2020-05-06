using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_ALaser : MonoBehaviour
{
    GameObject player;
    LineRenderer lineR;
    float DPS = 40f;
    float laserRad = .5f;
    Vector2 toPlayer;
    public GameObject arm;
    float laserOffset = 1.5f;
    float range;
    int state = 0; //0 = Idle, 1 = Charging, 2 = Firing, 3 = Cooldown
    public LayerMask ground;
    public LayerMask playerLayer;
    public LayerMask groundAndPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineR = GetComponent<LineRenderer>();
        lineR.positionCount = 2;
        lineR.SetPosition(0, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            toPlayer = player.transform.position - transform.position;
            arm.transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg), Vector3.forward);
            lineR.startWidth = .05f;
            lineR.endWidth = .05f;
            lineR.startColor = Color.green;
            lineR.endColor = Color.green;
            var visual = Physics2D.Raycast(transform.position + laserOffset * (Vector3)toPlayer.normalized, toPlayer, Mathf.Infinity, ground);
            lineR.SetPosition(0, transform.position + laserOffset * (Vector3)toPlayer.normalized);
            lineR.SetPosition(1, visual.point);
            range = visual.distance;
            var actual = Physics2D.Raycast(transform.position + laserOffset * (Vector3)toPlayer.normalized, toPlayer, Mathf.Infinity, groundAndPlayer);
            if (actual.transform.gameObject.layer == 8)
            {
                StartCoroutine("Charge", 2f);
                state = 1;
            }
        }
        else if (state == 1)
        {

        }
        else if (state == 2)
        {
            lineR.startWidth = 2 * laserRad;
            lineR.endWidth = 2 * laserRad;
            var hit = Physics2D.CircleCast(transform.position + laserOffset * (Vector3)toPlayer.normalized, laserRad, toPlayer, range, playerLayer);
            if (hit.transform != null)
            {
                hit.transform.GetComponent<Melee>().TakeDamage(DPS * Time.deltaTime);
            }
        }
        else
        {
            lineR.startWidth = 0;
            lineR.endWidth = 0;
        }
    }

    IEnumerator Charge(float delay)
    {
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
        state = 2;
        StartCoroutine("Stopfiring", 2f);
    }
    IEnumerator Stopfiring(float delay)
    {
        yield return new WaitForSeconds(delay / 8);
        lineR.startColor = Color.red;
        lineR.endColor = Color.red;
        yield return new WaitForSeconds(delay);
        state = 3;
        StartCoroutine("Cooldown", 1f);
    }
    IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        state = 0;
    }
}
