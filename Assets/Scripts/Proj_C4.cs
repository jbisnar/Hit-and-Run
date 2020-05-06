using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_C4 : MonoBehaviour
{
    public GameObject watcher;
    public GameObject exp;
    public bool armed;
    public LayerMask layerGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, .25f + .02f, layerGround))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (armed)
        {
            transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        if (GetComponent<Rigidbody2D>().velocity.magnitude < .1f && watcher != null)
        {
            watcher.GetComponent<AI_C4>().C4Landed();
        }
        //If the watcher dies, fizzle out
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("You are in the explosion radius");
        if (watcher != null && armed)
        {
            //explode in .3 seconds
            GetComponentInChildren<Exp_Tele>().StartCoroutine("Flash",.3f);
            watcher.GetComponent<AI_C4>().TryAgain();
            StartCoroutine("Detonate", .3f);
        } else
        {
            Debug.Log("This line in Proj_C4 should be unreachable");
        }
    }

    public IEnumerator Detonate(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject spawnedExp = GameObject.Instantiate(exp,transform.position, transform.rotation);
        spawnedExp.transform.localScale = new Vector3(3.5f, 3.5f, 1f);
        spawnedExp.GetComponent<Proj_EExplosion>().damage = 10f;
        Destroy(gameObject);
    }
}
