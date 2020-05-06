using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dam_Num : MonoBehaviour
{
    public Text text;
    float lifetime = 2f;
    float spawnedtime;
    Vector3 offset = new Vector3(0,100,0);
    public float number;

    // Start is called before the first frame update
    void Start()
    {
        spawnedtime = Time.time;
        text.text = ((int)number).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnedtime + lifetime)
        {
            Destroy(gameObject);
        }
        text.transform.localPosition = Vector3.Lerp(Vector3.zero, offset,1-Mathf.Pow(.5f, 10*(Time.time - spawnedtime) / lifetime));
        text.color = Color.Lerp(Color.yellow, new Color(1,1,0,0), (Time.time - spawnedtime)/lifetime);
    }
}
