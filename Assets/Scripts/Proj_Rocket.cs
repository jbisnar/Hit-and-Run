using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proj_Rocket : MonoBehaviour
{
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 11)
        {
            GameObject spawnedExp = GameObject.Instantiate(Explosion, transform.position, transform.rotation);
            spawnedExp.transform.parent = null;
            spawnedExp.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            Destroy(gameObject);
        }
    }
}
