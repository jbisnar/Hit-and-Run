using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_C4 : MonoBehaviour
{
    GameObject player;
    public GameObject C4;
    GameObject currentC4;
    float aggroRange = 8f;
    float watchRange = 4f;
    int waitStage = 0; //0 = Not waiting, 1 = Waiting for C4 to land, 2 = Walking, 3 = Watching, 4 = Reloading
    float disengRange = 10f;
    Enemy_Movement movement;
    Vector3 toPlayer;
    float expRange = 3f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        movement = GetComponent<Enemy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {

        //Throw C4 on sight
        toPlayer = transform.position - player.transform.position;
        if (toPlayer.magnitude < aggroRange && waitStage == 0)
        {
            movement.direction = 0;
            currentC4 = GameObject.Instantiate(C4,transform.position, transform.rotation);
            currentC4.transform.parent = null;
            currentC4.GetComponent<Proj_C4>().watcher = gameObject;
            waitStage = 1;
            currentC4.GetComponent<Rigidbody2D>().velocity = new Vector2(-Mathf.Sign(toPlayer.x), 1)*3;
        }

        //Move out of range, then arm the C4
        if (waitStage == 2)
        {
            if (Physics2D.Raycast(transform.position - new Vector3(0, movement.height), new Vector2(movement.direction, 0), 1f, movement.layerGround))
            {
                movement.jumping = true;
            }
            else
            {
                movement.jumping = false;
            }
            if (currentC4 != null && (currentC4.transform.position - transform.position).magnitude > watchRange)
            {
                GetComponent<Enemy_Movement>().direction = 0;
                currentC4.GetComponent<Proj_C4>().armed = true;
                waitStage = 3;
            }
        }

        //Watch
    }

    private void OnDestroy()
    {
        if (currentC4 != null)
        {
            Destroy(currentC4);
        }
    }

    public void C4Landed()
    {
        if (waitStage == 1)
        {
            waitStage = 2;
            StartCoroutine("GiveUp", 5f);
            GetComponent<Enemy_Movement>().direction = (int) -Mathf.Sign((currentC4.transform.position - transform.position).x);
        }
    }

    public void TryAgain()
    {
        if (waitStage == 3)
        {
            waitStage = 4;
            StartCoroutine("Reload", .5f);
        }
    }

    public IEnumerator GiveUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Enemy_Movement>().direction *= -1;
    }

    public IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);
        waitStage = 0;
    }
}
