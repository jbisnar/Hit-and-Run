using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp_Tele : MonoBehaviour
{
    public float liveTime;
    float deleteTime;

    // Start is called before the first frame update
    void Start()
    {
        //deleteTime = Time.time + liveTime;
        //StartCoroutine("Flash", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (deleteTime - Time.time < .3f && deleteTime - Time.time > .225f)
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
        }
        else if (deleteTime - Time.time < .15f && deleteTime - Time.time > .075f)
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        if (Time.time > deleteTime && liveTime > 0)
        {
            StartCoroutine("Flash", 0f);
            Destroy(gameObject);
        }
        */
    }

    public IEnumerator Flash(float delay)
    {
        yield return new WaitForSeconds(delay - .3f);
        GetComponent<SpriteRenderer>().color = Color.clear;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().color = Color.clear;
        yield return new WaitForSeconds(.075f);
        GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(.075f);
        Destroy(gameObject);
    }
}
