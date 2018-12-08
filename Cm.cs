using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cm : MonoBehaviour {
    public Image face;
    public Image rewardSprite;
    public bool faced = false;
    public static Text sc;
    public static int p = 5;
    public static bool animating;
    public static Cm c1;
    public Animator anim;
    public AudioSource openZap;
    public AudioSource closeZap;
    public AudioSource fail;
    public AudioSource succes;
    public static GameObject lens;
    public static int pairsThisLevel = 0;

    public static Text lentsCharges;
    public static Text callColorCharges;
    public static Text callShapeCharges;
    public static Text bombCharges;

    public delegate void onTool(Cm cm);
    public static event onTool OnToolUsed;

    public delegate Cm onCardClick();
    public static onCardClick CardClicked;

    void GiveCharge(int tool, Text toolText)
    {
        toolText.text = tool.ToString();
    }

    void Stopa()
    {
        animating = false;
        anim.speed = 0;
        faced = true;
        if (CardClicked != null)
        {
            CardClicked.Invoke();
        }
       
    }
    public void Resuma()
    {
        anim.speed = 1;
        closeZap.Play();
        animating = true;
    }
    void HIde()
    {
        faced = false;
        anim.SetBool("show", false);
        animating = false;
    }

    public void Clic()
    {
        if(OnToolUsed != null && !animating)
        {
            animating = true;
            OnToolUsed(this);
        }
        else if(OnToolUsed == null && !animating)
        {
            if(CardClicked == null && c1 ==null)
            {
                OpenCard();
                CardClicked += SetCard1;
            }
            else if(CardClicked ==null && c1!=null)
            {
                OpenCard();
                CardClicked += SetCard2;
            }
        }
    }

    void Reward(Cm cm)
    {
        if (cm.reward==global::Reward.lents)
        {
            Gm.chargeLents++;
            lentsCharges.text = Gm.chargeLents.ToString();

        }else if (cm.reward == global::Reward.callColor)
        {
            Gm.chargeCallColor++;
            callColorCharges.text = Gm.chargeCallColor.ToString();
        }
        else if (cm.reward == global::Reward.callShape)
        {
            Gm.chargeCallShape++;
            callShapeCharges.text = Gm.chargeCallShape.ToString();
        }
        else if(cm.reward == global::Reward.bomb)
        {
            Gm.chargeBomb++;
            bombCharges.text = Gm.chargeBomb.ToString();
        }

        if(cm.rewardSprite !=null) cm.rewardSprite.gameObject.SetActive(false);
    }

    void Solve()
    {
        if(c1.face.sprite == face.sprite)
        {
            p += 3;
            succes.Play();
            pairsThisLevel++;
            Reward(c1);
            Reward(this);
            Gm.CounterMinusOne(face.sprite);
            if (pairsThisLevel == Progress.PairsToEndLevel)
            {
                StartCoroutine(Gm.NextScene());
            }
        }
        else
        {
            p -= 1;
            fail.Play();
            c1.Resuma();
            Resuma();
        }
        c1 = null;
        sc.text = p.ToString();
    }

    public void  OpenCard()
    {
        anim.SetBool("show", true);
        openZap.Play();
    }

    Cm SetCard1()
    {
        CardClicked -= SetCard1;
        return c1 =this;
    }
    Cm SetCard2()
    {
        CardClicked -= SetCard2;
        Solve();
        return this;
    }



    public static void AllRetry()
    {
        c1 = null;
        animating = false;
        pairsThisLevel = 0;
    }

    public CardColor color;
    public CardShape shape;
    public Reward reward;
    public int position;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

    }
}
public enum CardColor {red, yellow, blue, green };
public enum CardShape {star, circle, diamond };
public enum Reward {nothing, lents, callColor, callShape, bomb };