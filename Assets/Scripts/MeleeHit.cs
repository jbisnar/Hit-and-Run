using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    float liveTime = .2f;
    float deacTime;
    public float damage;
    public int weapon;
    public AudioClip[] swordSounds;
    public AudioClip[] hammerSounds;
    public AudioClip[] funnySounds;
    public AudioSource ASour;

    // Start is called before the first frame update
    void Start()
    {
        deacTime = Time.time + liveTime;
        Debug.Log("MeleeHit: "+damage);
    }

    private void OnEnable()
    {
        deacTime = Time.time + liveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deacTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            var enemy = collision.gameObject.GetComponent<Enemy_Base>();
            if (enemy != null)
            {
                enemy.Damage(damage);
                if (weapon == 1)
                {
                    ASour.clip = funnySounds[Random.Range(0, funnySounds.Length)];
                }
                else if (weapon == 2)
                {
                    ASour.clip = swordSounds[Random.Range(0, swordSounds.Length)];
                }
                else
                {
                    ASour.clip = hammerSounds[Random.Range(0, hammerSounds.Length)];
                }
                ASour.Play();
            }
        }
    }
}
