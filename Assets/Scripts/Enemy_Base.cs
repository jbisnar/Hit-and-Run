using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float health;
    public GameObject damageNumber;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log(damage);
        GameObject spawnedDN = GameObject.Instantiate(damageNumber, transform.position, transform.rotation);
        spawnedDN.transform.parent = null;
        spawnedDN.GetComponent<Dam_Num>().number = damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
