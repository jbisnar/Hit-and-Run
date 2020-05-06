using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loadout : MonoBehaviour
{
    public Text title;
    public Text desc;
    public Text flav;
    public Mod_mvmt playermove;
    public Melee playermelee;
    public GameObject hud;
    public GameObject spawnloc;
    int selectedAct;
    int selectedPass;
    int selectedMelee;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mouseOverJetpack()
    {
        title.text = "Jetpack";
        desc.text = "Hold right-click to accelerate towards the mouse";
        flav.text = "With a jet on your back, you'll always be ahead of the pack.";
    }
    public void selectJetpack()
    {
        selectedAct = 1;
    }

    public void mouseOverGrapple()
    {
        title.text = "Grappling Hook";
        desc.text = "Right-click to pull yourself towards a surface\nRight-click again to dismount\nReceive a small boost in the direction of the mouse";
        flav.text = "A grapple a day keeps the ground away.";
    }
    public void selectGrapple()
    {
        selectedAct = 2;
    }

    public void mouseOverRocket()
    {
        title.text = "Rocket Jumper";
        desc.text = "Right-click to fire a rocket that knocks you back\nRockets deal no damage";
        flav.text = "Why do the rockets do no damage? Because everyone is required to be vaccinated against rockets.";
    }
    public void selectRocket()
    {
        selectedAct = 3;
    }

    public void mouseOverShoes()
    {
        title.text = "Athletic Shoes";
        desc.text = "Run faster\nJump higher";
        flav.text = "Shoes don't make you more athletic. Anyone who says otherwise is trying to sell you something.";
    }
    public void selectShoes()
    {
        selectedPass = 1;
    }

    public void mouseOverSkates()
    {
        title.text = "Ice Skates";
        desc.text = "Ground is slippery\nNo longer slow down when touching the ground";
        flav.text = "How to avoid falling over? Go to  the inspector and under the Rigidbody component, check Freeze Rotation";
    }
    public void selectSkates()
    {
        selectedPass = 2;
    }

    public void mouseOverSword()
    {
        title.text = "Sword";
        desc.text = "Moderate damage";
        flav.text = "Let's face it, people who say 'the pen is mightier than the sword' are too afraid to back it up.";
    }
    public void selectSword()
    {
        selectedMelee = 2;
    }

    public void mouseOverHammer()
    {
        title.text = "Hammer";
        desc.text = "High damage\nStop all momentum on hit";
        flav.text = "Use a gavel to tell people to be quiet. Use a hammer to make them be quiet.";
    }
    public void selectHammer()
    {
        selectedMelee = 3;
    }

    public void mouseOverSecret()
    {
        title.text = "Funny Hammer";
        desc.text = "Functionally identical to the normal hammer";
        flav.text = "...";
    }
    public void selectSecret()
    {
        selectedMelee = 1;
    }

    public void confirmLoadout()
    {
        if (selectedAct == 0)
        {
            selectedAct = 1;
        }
        if (selectedPass == 0)
        {
            selectedPass = 1;
        }
        if (selectedMelee == 0)
        {
            selectedMelee = 2;
        }

        playermove.abilityAct = selectedAct;
        playermove.abilityPass = selectedPass;
        playermelee.weapon = selectedMelee;
        playermove.changeLoadout();
        playermove.transform.position = spawnloc.transform.position;

        hud.SetActive(true);
        gameObject.SetActive(false);
    }
}
