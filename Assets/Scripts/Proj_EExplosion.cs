using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_EExplosion : MonoBehaviour
{
    float liveTime = .15f;
    float deleteTime;
    public float damage;

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
                player.Knockback((collision.transform.position - transform.position), 15f);
            }
            var playhealth = collision.gameObject.GetComponent<Melee>();
            if (playhealth != null)
            {
                playhealth.TakeDamage(damage);
            }
        }
    }
}
