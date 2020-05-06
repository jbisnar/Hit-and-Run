using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Missile : MonoBehaviour
{
    GameObject player;
    public GameObject exp;
    float expRange = 2f;
    float chaseTime;
    float turnRate = 60;
    float velocity = 4;
    Vector3 toPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        toPlayer = transform.position - player.transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis((Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg) - 180, Vector3.forward), turnRate * Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad)) * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<Exp_Tele>().StartCoroutine("Flash", .3f);
        StartCoroutine("Detonate", .3f);
        velocity = 0;
    }

    IEnumerator Detonate(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject spawnedExp = GameObject.Instantiate(exp, transform.position, transform.rotation);
        spawnedExp.transform.localScale = Vector3.one * expRange;
        spawnedExp.GetComponent<Proj_EExplosion>().damage = 10f;
        spawnedExp.transform.parent = null;
        Destroy(gameObject);
    }
}
