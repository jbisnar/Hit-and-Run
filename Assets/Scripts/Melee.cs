using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Melee : MonoBehaviour
{
    float maxhealth = 100f;
    float curhealth = 100f;
    public RectTransform healthBar;
    public RectTransform fuelBar;
    public RectTransform fuelBGBar;
    float healthbarWidth;
    float fuelbarWidth;
    public Camera cam;
    Vector2 mousePos;
    public Vector2 aim;
    float swingdelay = .15f;
    float swingrate = .8f;
    float swingtime;
    public GameObject hitbox;
    float hitboxDist = 1f;
    public int weapon = 3; // 0 = nothing, 1 = drill?, 2 = sword, 3 = hammer
    public Sprite sword;
    public Sprite hammer;
    float exp = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (weapon == 1)
        {
            exp = 2;
        }
        else if (weapon == 2)
        {
            exp = 1.5f;
        }
        else
        {
            exp = 2;
        }
        Debug.Log(weapon);
        healthbarWidth = healthBar.rect.width;
        fuelbarWidth = fuelBar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.sizeDelta = new Vector2((curhealth / maxhealth) * healthbarWidth, healthBar.sizeDelta.y);
        if (GetComponent<Mod_mvmt>().abilityAct == 1)
        {
            fuelBar.gameObject.SetActive(true);
            fuelBGBar.gameObject.SetActive(true);
            fuelBar.sizeDelta = new Vector2((GetComponent<Mod_mvmt>().jetFuelcur / GetComponent<Mod_mvmt>().jetFuelMax) * fuelbarWidth, fuelBar.sizeDelta.y);
        }
        else
        {
            fuelBar.gameObject.SetActive(false);
            fuelBGBar.gameObject.SetActive(false);
        }

        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z));
        aim = mousePos - new Vector2(transform.position.x, transform.position.y);
        aim = aim.normalized;

        if (Input.GetMouseButtonDown(0) && Time.time > swingtime + swingrate)
        {
            swingtime = Time.time;
            StartCoroutine("Swing", swingdelay);
        }

        if (weapon == 1)
        {
            exp = 2.5f;
        }
        else if (weapon == 2)
        {
            exp = 2f;
        }
        else
        {
            exp = 2.5f;
        }
    }

    public void TakeDamage(float damage)
    {
        curhealth -= damage;
    }

    IEnumerator Swing(float delay)
    {
        yield return new WaitForSeconds(delay);
        hitbox.transform.localPosition = (Vector3)(aim * hitboxDist);
        hitbox.transform.localRotation = Quaternion.AngleAxis((Mathf.Atan2(hitbox.transform.localPosition.y, hitbox.transform.localPosition.x) * Mathf.Rad2Deg) - 90, Vector3.forward);
        if (weapon == 1 || weapon == 3)
        {
            hitbox.GetComponent<SpriteRenderer>().sprite = hammer;
        }
        else
        {
            hitbox.GetComponent<SpriteRenderer>().sprite = sword;
        }
        hitbox.GetComponent<MeleeHit>().damage = GetComponent<Rigidbody2D>().velocity.magnitude * Mathf.Pow(Mathf.Log(GetComponent<Rigidbody2D>().velocity.magnitude+1), exp);
        hitbox.GetComponent<MeleeHit>().weapon = weapon;
        hitbox.SetActive(true);
        if (weapon == 1 || weapon ==3)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        //Debug.Log("Velocity: "+ GetComponent<Rigidbody2D>().velocity);
    }
}
