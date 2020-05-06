using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod_mvmt : MonoBehaviour
{

    float gravNormal = 20f;
    float gravJump = 20f;
    float gravDown = 15f;
    float gravWall = 0f;
    float gravGlide = 2f;
    float velWalk = 7.5f;
    float accelWalk = 40f;
    float accelSwitch = 60f;
    float accelSlow = 30f;
    float velAir = 5f;
    float accelAir = 20f;
    float accelAirSwitch = 30f;
    float accelAirSlow = 2f;
    float velSlide = 3f;
    float accelSwing = 10f;
    float accelSlideSwitch = 15f;
    float accelSlideSlow = 5f;
    float velGlide = 1f;
    float velJumpGround = 5f;
    float velJumpWallH = 4f;
    float velJumpWallV = 4f;
    float velWallSlideDown = 3f;

    public bool slidepants = false;
    public bool wallshoes = true;
    public bool glidesuit = false;
    public bool swingtie = false;
    bool swinging = false;
    float velWalkNorm = 6f;
    float velWalkRun = 10f;
    float velJumpGNorm = 8f;
    float velJumpGShoes = 12f;
    bool iceskates = false;

    public int abilityPass = 0; // 0 = nothing, 1 = running shoes, 2 = jump shoes, 3 = ice skates
    public int abilityAct = 0; // 0 = nothing, 1 = jetpack, 2 = grapple hook, 3 = rocket jumper
    //public int weapon = 0; // 0 = nothing, 1 = drill?, 2 = sword, 3 = hammer
    float acceljet = 15f;
    public float jetFuelcur = 2f;
    public float jetFuelMax = 2f;
    public GameObject hook;
    public DistanceJoint2D ropeJoint;
    public SliderJoint2D ropeSlider;
    public LineRenderer ropeLine;
    float grappleRange = 15f;
    float velReelStart = 0;
    float velReelCur;
    float accelReel = 10f;
    float velGrappleBoost = 5f;
    public GameObject rocket;
    float rocketshottime;
    float rocketFireRate = .5f;
    public Sprite sprJet;
    public Sprite sprGrap;
    public Sprite sprRJ;
    public GameObject Shoes;
    public Sprite sprAth;
    public Sprite sprSkates;

    float savedVel = 0;
    float savedVelAir = 0;
    float savedVelWallKick = 0;

    public bool grounded = false;
    public bool jumping = false;
    public bool walledL = false;
    public bool wallslideL = false;
    public bool walledR = false;
    public bool wallslideR = false;
    float walljumpgrace = .05f;
    float walljumpcontrol = .5f;
    float perfectkickgrace = .15f;
    float walljumpgracetime;
    float walljumpcontroltime;
    float perfectkicktime;
    public LayerMask layerGround;
    public Vector2 temp;

    // Start is called before the first frame update
    void Start()
    {
        velWalk = velWalkNorm;
        velJumpGround = velJumpGNorm;
        GetComponent<LineRenderer>().positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        temp = transform.GetComponent<Rigidbody2D>().velocity;
        grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .16f, transform.position.y - .36f),
            new Vector2(transform.position.x + .16f, transform.position.y - .38f), layerGround);
        if ((!walledL && !walledR) && (Physics2D.OverlapArea(new Vector2(transform.position.x - .2f, transform.position.y + .36f),
            new Vector2(transform.position.x - .18f, transform.position.y - .30f), layerGround) || Physics2D.OverlapArea(new Vector2(transform.position.x + .18f, transform.position.y + .36f),
            new Vector2(transform.position.x + .2f, transform.position.y - .30f), layerGround)))
        {
            perfectkicktime = Time.time + perfectkickgrace;
        }
        walledL = Physics2D.OverlapArea(new Vector2(transform.position.x - .2f, transform.position.y + .36f),
            new Vector2(transform.position.x - .18f, transform.position.y - .30f), layerGround);
        walledR = Physics2D.OverlapArea(new Vector2(transform.position.x + .18f, transform.position.y + .36f),
            new Vector2(transform.position.x + .2f, transform.position.y - .30f), layerGround);
        if (walledL || walledR)
        {
            walljumpgracetime = Time.time + walljumpgrace;
        }

        //HORIZONTAL CONTROL
        if (Input.GetAxisRaw("Horizontal") == 0)
        { //Slow down
            if (Mathf.Abs(temp.x) < accelSlow * Time.deltaTime)
            {
                temp.x = 0;
            }
            else if (iceskates)
            {
                temp.x = temp.x;
            }
            else if (temp.x > 0)
            {
                if (grounded)
                {
                    temp.x -= accelSlow * Time.deltaTime;
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x -= accelAirSlow * Time.deltaTime;
                }
            }
            else
            {
                if (grounded)
                {
                    temp.x += accelSlow * Time.deltaTime;
                }
                else if (Time.time > walljumpcontroltime)
                {
                    temp.x += accelAirSlow * Time.deltaTime;
                }
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        { //Right
            if (walledR)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (temp.x > velWalk && iceskates)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > velWalk)
                {
                    temp.x = velWalk;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelWalk * Time.deltaTime;
                }
                savedVelAir = temp.x;
            }
            else
            {
                if (swinging)
                {
                    temp.x += accelSwing * Time.deltaTime;
                }
                else if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < 0)
                {
                    temp.x += accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x += accelAir * Time.deltaTime;
                }
            }
        }
        else
        { //Left
            if (walledL)
            {
                temp.x = 0;
            }
            else if (grounded)
            {
                if (temp.x < -velWalk && iceskates)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < -velWalk)
                {
                    temp.x = -velWalk;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelWalk * Time.deltaTime;
                }
            }
            else
            {
                if (swinging)
                {
                    temp.x -= accelSwing * Time.deltaTime;
                }
                else if (Time.time < walljumpcontroltime)
                {
                    temp.x = temp.x;
                }
                else if (temp.x < -velAir)
                {
                    temp.x = temp.x;
                }
                else if (temp.x > 0)
                {
                    temp.x -= accelAirSwitch * Time.deltaTime;
                }
                else
                {
                    temp.x -= accelAir * Time.deltaTime;
                }
            }
        }
        if (temp.x != 0)
        {
            savedVel = temp.x;
        }

        //GRAVITY
        if (grounded)
        {
            jumping = false;
            temp.y = 0;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                temp.y = velJumpGround;
                jumping = true;
            }
        }
        else if (walledL)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                if (savedVel < -velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                }
                else
                {
                    temp.x = velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideL && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetAxisRaw("Vertical") > 0 && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (walledR)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                if (savedVel > velJumpWallH && Time.time < perfectkicktime)
                {
                    temp.x = -savedVel;
                }
                else
                {
                    temp.x = -velJumpWallH;
                }
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
            if (wallslideR && temp.y < -velWallSlideDown)
            {
                temp.y = -velWallSlideDown;
            }
            else if (Input.GetKey("w") && temp.y > 0)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }
        else if (Time.time < walljumpgracetime)
        {
            jumping = false;
            if (Input.GetKeyDown("w") && wallshoes)
            {
                //temp.x = -velJumpWallH;
                temp.y = velJumpWallV;
                jumping = true;
                walljumpcontroltime = Time.time + walljumpcontrol;
            }
        }
        else
        {
            if (Input.GetKey("w") && temp.y > 0 && jumping)
            {
                temp.y -= gravJump * Time.deltaTime;
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                jumping = false;
                temp.y -= gravDown * Time.deltaTime;
            }
            else
            {
                jumping = false;
                temp.y -= gravNormal * Time.deltaTime;
            }
        }

        //ABILITIES
        if (Input.GetMouseButtonDown(1))
        {
            if (abilityAct == 2 && !swinging)
            {
                //Grapple
                //Raycast, put hook at hit
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, .1f, GetComponent<Melee>().aim, grappleRange, layerGround);
                if (hit.collider != null)
                {
                    velReelCur = Mathf.Max(Vector2.Dot(temp, GetComponent<Melee>().aim), 0);
                    swinging = true;
                    hook.transform.position = hit.point;
                    hook.transform.position += Vector3.back;
                    hook.transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(GetComponent<Melee>().aim.y, GetComponent<Melee>().aim.x) * Mathf.Rad2Deg), Vector3.forward);
                    hook.SetActive(true);
                    ropeSlider.enabled = true;
                    ropeLine.enabled = true;
                }
            }
            else if (abilityAct == 2 && swinging)
            {
                //Ungrapple
                swinging = false;
                hook.SetActive(false);
                ropeSlider.enabled = false;
                temp += GetComponent<Melee>().aim * velGrappleBoost;
                temp.y += velGrappleBoost;
                ropeLine.enabled = false;
            }
            else if (abilityAct == 3 && Time.time > rocketshottime + rocketFireRate)
            {
                //Shoot rocket
                GameObject spawnedRocket = GameObject.Instantiate(rocket, transform.position, transform.rotation);
                spawnedRocket.transform.parent = null;
                spawnedRocket.transform.rotation = Quaternion.AngleAxis((Mathf.Atan2(GetComponent<Melee>().aim.y, GetComponent<Melee>().aim.x) * Mathf.Rad2Deg), Vector3.forward);
                spawnedRocket.GetComponent<Rigidbody2D>().velocity = GetComponent<Melee>().aim * 15f;
                rocketshottime = Time.time;
            }
        }

        if (Input.GetMouseButton(1) && abilityAct == 1 && jetFuelcur > 0)
        {
            //Jetpack push
            var cancelgrav = new Vector2(0,gravNormal);
            temp += cancelgrav * Time.deltaTime + GetComponent<Melee>().aim * acceljet * Time.deltaTime;
            jetFuelcur -= Time.deltaTime;
        }
        else if (!Input.GetMouseButton(1) && jetFuelcur < jetFuelMax)
        {
            jetFuelcur += Time.deltaTime;
        }

        if (swinging)
        {
            temp = (Vector2)(Quaternion.Euler(0, 0, ropeSlider.angle) * Vector2.right) * velReelCur;
            velReelCur += (accelReel * Time.deltaTime);
            ropeLine.SetPosition(0, transform.position);
            ropeLine.SetPosition(1, hook.transform.position);
        }

        transform.GetComponent<Rigidbody2D>().velocity = temp;
    }

    public void Knockback(Vector2 direction, float mag)
    {
        transform.GetComponent<Rigidbody2D>().velocity += direction.normalized * mag;
    }

    public void changeLoadout()
    {
        if (abilityPass == 1)
        {
            velWalk = velWalkRun;
            velJumpGround = velJumpGShoes;
            iceskates = false;
            Shoes.GetComponent<SpriteRenderer>().sprite = sprAth;
        }
        else if (abilityPass == 2)
        {
            velWalk = velWalkNorm;
            velJumpGround = velJumpGNorm;
            iceskates = true;
            Shoes.GetComponent<SpriteRenderer>().sprite = sprSkates;
        }

        if (abilityAct == 1)
        {
            GetComponent<SpriteRenderer>().sprite = sprJet;
        }
        else if  (abilityAct == 2)
        {
            GetComponent<SpriteRenderer>().sprite = sprGrap;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = sprRJ;
        }
    }
}
