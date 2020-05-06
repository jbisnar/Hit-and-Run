using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RJ_Explosion : MonoBehaviour
{
    float liveTime = .15f;
    float deleteTime;

    // Start is called before the first frame update
    void Start()
    {
        deleteTime = Time.time + liveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deleteTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            var player = collision.gameObject.GetComponent<Mod_mvmt>();
            if (player != null)
            {
                Debug.Log(1.15 - (collision.transform.position - transform.position).magnitude/2);
                player.Knockback((collision.transform.position - transform.position), 10f);
            }
        }
    }
}
