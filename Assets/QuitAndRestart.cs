using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitAndRestart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
