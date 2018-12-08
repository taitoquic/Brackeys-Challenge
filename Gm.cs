using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Gm : MonoBehaviour {
    public GameObject c;
    public GameObject lens;
    GameObject l;
    public int[] initial;
    public Image[] icons;
    public Text[] xs;
    public Text[] amounts;
    public static Text[] staticAm;
    public static Text[] staticMsgs = new Text[3];
    static Dictionary<Sprite, int> iconsAmount;
    static Dictionary<Sprite, int> iconOrder;
    public List<CardSprites> allSprites = new List<CardSprites>();
    public Sprite[] rewardIcons;
    List<CardSprites> sprites;
    public static List<Cm> cms;
    Progress pro;
    int[] positions = new int[Progress.RowCards * Progress.ColuCards];
    float px, py;
    Vector2 vini = new Vector2(-5.5f, 3.6f);
    public Text score;
    public Text scoreTittle;
    public Text goalTittle;
    public Text goal;
    public Text notCharges;
    public Text needCardFace;
    public Text firstCardHide;
    public Text gameOver;
    public Text win;
    public Text levelClear;
    public AudioSource music;
    public AudioSource bombSound;

    void Shu()
    {
        sprites = new List<CardSprites>();
        iconsAmount = new Dictionary<Sprite, int>();
        iconOrder = new Dictionary<Sprite, int>();
        int order = 0;
        for (int i = 0; i < Progress.RowCards * Progress.ColuCards; i++)
        {
            positions[i] = i;
        }
        for (int i = 0; i < Progress.RowCards * Progress.ColuCards; i++)
        {
            int rand = Random.Range(0, Progress.RowCards * Progress.ColuCards);
            int  temp = positions[i];
            positions[i] = positions[rand];
            positions[rand] = temp;
        }
        for (int g = 0; g < Progress.RowCards * Progress.ColuCards/2; g++)
        {
            int rand = Random.Range(0, allSprites.Count);
            sprites.Add(allSprites[rand]);
            if (!iconsAmount.ContainsKey(allSprites[rand].spriteCard))
            {
                iconsAmount.Add(allSprites[rand].spriteCard, 1);
                ActivateIcon(order, allSprites[rand].spriteCard);
                iconOrder.Add(allSprites[rand].spriteCard, order);
                order++;
            }
            else
            {
                iconsAmount[allSprites[rand].spriteCard]++;                
                amounts[iconOrder[allSprites[rand].spriteCard]].text = iconsAmount[allSprites[rand].spriteCard].ToString();
            }

        }
    }

    void ActivateIcon(int order, Sprite sp)
    {
        icons[order].gameObject.SetActive(true);
        icons[order].sprite = sp;
        xs[order].gameObject.SetActive(true);
        amounts[order].gameObject.SetActive(true);
        amounts[order].text = iconsAmount[sp].ToString();
    }

    public static void CounterMinusOne(Sprite sp)
    {
        iconsAmount[sp]--;
        staticAm[iconOrder[sp]].text = iconsAmount[sp].ToString();
    }

    void Initial(int[] valors)
    {
        valors[0] = Cm.p;
        valors[1] = chargeLents;
        valors[2] = chargeCallColor;
        valors[3] = chargeCallShape;
        valors[4] = chargeBomb;
        valors[5] = Progress.level;
    }
    float pLent = 35.0f;
    float pCallCo = 60.0f;
    float pCallSh = 85.0f;
    Sprite sRewa;


    Reward Cardreward
    {
        get
        {
            float pReward = Random.Range(0, 100);
            Reward temp = Reward.nothing;
            if (pReward <= 33.0f)
            {
                float chooseReward = Random.Range(0, 100);
                if (chooseReward>=0 && chooseReward <pLent)
                {
                    temp = Reward.lents;
                    sRewa = rewardIcons[0];
                }
                else if (chooseReward >=pLent && chooseReward<pCallCo)
                {
                    temp = Reward.callColor;
                    sRewa = rewardIcons[1];
                }
                else if (chooseReward >=pCallCo && chooseReward<pCallSh)
                {
                    temp = Reward.callShape;
                    sRewa = rewardIcons[2];
                }
                else
                {
                    temp = Reward.bomb;
                    sRewa = rewardIcons[3];
                }
            }
            if (temp == Reward.nothing) sRewa = null;
            return temp;
        }
    }

    public delegate void onTextDisplay(Text text, bool isShow);
    public onTextDisplay TextDisplayed;

    void ShowMessageText(Text message, bool isShow)
    {
        message.gameObject.SetActive(isShow);

        goal.gameObject.SetActive(!isShow);
        goalTittle.gameObject.SetActive(!isShow);
        score.gameObject.SetActive(!isShow);
        scoreTittle.gameObject.SetActive(!isShow);
        if (!isShow) TextDisplayed -= ShowMessageText;
    }

    IEnumerator ShowAndHide(Text message)
    {
        TextDisplayed += ShowMessageText;
        if (TextDisplayed != null)
        {
            TextDisplayed.Invoke(message, true);
            yield return new WaitForSeconds(1.5f);
            ShowMessageText(message, false);
        }

    }

    // Use this for initialization
    void Start () {
        staticMsgs[0] = gameOver;
        staticMsgs[1] = win;
        staticMsgs[2] = levelClear;
        staticAm = amounts;
        pro = gameObject.GetComponent<Progress>();
        px = pro.SetPaddingX(Progress.level);
        py = pro.SetPaddingY(Progress.level);
        pro.SetSize(c.GetComponent<RectTransform>(), Progress.level);
        if(!music.isPlaying) music.Play();
        Cm.sc = score;
        Cm.sc.text = Cm.p.ToString();
        initial = new int[6];
        Initial(initial);
        Cm.lentsCharges = chargeLentsText;
        Cm.callColorCharges = chargeCallColorText;
        Cm.callShapeCharges = chargeCallShapeText;
        Cm.bombCharges = chargeBombText;
        Cm.lentsCharges.text = chargeLents.ToString();
        Cm.callColorCharges.text = chargeCallColor.ToString();
        Cm.callShapeCharges.text = chargeCallShape.ToString();
        Cm.bombCharges.text = chargeBomb.ToString();
        l = Instantiate(lens, transform);
        l.gameObject.SetActive(false);
        Cm.lens = l;
        goal.text = Progress.Goal.ToString();
        Shu();
        cms = new List<Cm>();
        for (int i = 0; i < Progress.RowCards; i++)
        {
            for (int h = 0; h < Progress.ColuCards; h++)
            {
                int isp = positions[Progress.RowCards * h + i];
                if (isp >= Progress.RowCards * Progress.ColuCards/2) isp -= Progress.RowCards * Progress.ColuCards/2;
                GameObject g = Instantiate(c, new Vector2(vini.x + px * i, vini.y + py * h), Quaternion.identity);
                g.GetComponent<Cm>().face.sprite = sprites[isp].spriteCard;
                g.GetComponent<Cm>().reward = Cardreward;
                if (sRewa != null)
                {
                    g.GetComponent<Cm>().rewardSprite.sprite = sRewa;
                }
                else
                {
                    g.GetComponent<Cm>().rewardSprite.gameObject.SetActive(false);
                }
                g.GetComponent<Cm>().color = sprites[isp].color;
                g.GetComponent<Cm>().shape = sprites[isp].shape;
                g.GetComponent<Cm>().position = Progress.RowCards*h+i;
                cms.Add(g.GetComponent<Cm>());
            }
        }
	}
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cm.AllRetry();
        Cm.p = initial[0];
        chargeLents = initial[1];
        chargeCallColor = initial[2];
        chargeCallShape = initial[3];
        chargeBomb = initial[4];
        Progress.level = initial[5];
    }

    public void Retire()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Cm.AllRetry();
        Cm.p = 5;
        chargeLents = 0;
        chargeCallColor = 0;
        chargeCallShape = 0;
        chargeBomb = 0;
        Progress.level = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static IEnumerator NextScene()
    {
        Cm.pairsThisLevel = 0;
        if (Cm.p >=Progress.Goal && Progress.level < 5)
        {
            staticMsgs[2].gameObject.SetActive(true);
            yield return new WaitForSeconds(2.2f);
            Progress.level++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(Cm.p > Progress.Goal && Progress.level == 5)
        {
            staticMsgs[1].gameObject.SetActive(true);
        }
        else
        {
            staticMsgs[0].gameObject.SetActive(true);
        }

    }

    public static bool lentsOn = false;
    public static bool callColorOn = false;
    public static bool callShapeOn = false;
    public static bool bombOn = false;
    public static int chargeLents = 0;
    public static int chargeCallColor = 0;
    public static int chargeCallShape = 0;
    public static int chargeBomb = 0;

    public Text chargeLentsText;
    public Text chargeCallColorText;
    public Text chargeCallShapeText;
    public Text chargeBombText;

    public Button lensButton;
    public Sprite lensOn;
    public Sprite lensOff;

    public Button callColorButton;
    public Sprite callColorButtonOn;
    public Sprite callColorButtonOff;

    public Button callShapeButton;
    public Sprite callShapeButtonOn;
    public Sprite callShapeButtonOff;

    public Button bombButton;
    public Sprite bombButtonOn;
    public Sprite bombButtonOff;

    public delegate void onClickTool();
    public static onClickTool OnToolSelected;


    public void LentsOn()
    {
        if (chargeLents > 0 && !l.activeInHierarchy && !Cm.animating)
        {
            if (OnToolSelected != null && lentsOn)
            {
                OnToolSelected.Invoke();
            }
            else
            {
                if(OnToolSelected != null) OnToolSelected.Invoke();
                lentsOn = true;
                LentsButtonOn();
                OnToolSelected += LentsOff;
                OnToolSelected += LentsButtonOff;
                Cm.OnToolUsed += UseLens;
            }
        }
        else if(chargeLents ==0)
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(notCharges));
            }
            
            if (OnToolSelected != null) OnToolSelected.Invoke();
        }

    }
    void LentsOff()
    {
        lentsOn = false;
        OnToolSelected -= LentsOff;
        Cm.OnToolUsed -= UseLens;
        Cm.animating = false;
    }
    void LentsButtonOn()
    {
        lensButton.GetComponent<Image>().sprite = lensOn;
    }
    void LentsButtonOff()
    {
        lensButton.GetComponent<Image>().sprite = lensOff;
        OnToolSelected -= LentsButtonOff;
    }
    void UseLens(Cm cm)
    {
        chargeLents--;
        Cm.lentsCharges.text = chargeLents.ToString();
        l.transform.position = cm.transform.position;
        l.gameObject.SetActive(true);
        if (OnToolSelected != null) OnToolSelected.Invoke();
        Invoke("HideLens", 2.2f);
    }
    void HideLens()
    {
        l.gameObject.SetActive(false);
    }

    public void CallColorOn()
    {
        if (chargeCallColor > 0 && !Cm.animating)
        {
            if (OnToolSelected != null && callColorOn)
            {
                OnToolSelected.Invoke();
            }
            else
            {
                if (OnToolSelected != null) OnToolSelected.Invoke();
                callColorOn = true;
                FaceActivated();
                CallColorButtonOn();
                OnToolSelected += CallColorOff;
                OnToolSelected += FaceNoActivated;
                OnToolSelected += CallColorButtonOff;
                Cm.OnToolUsed += UseCallingColor;
            }
        }
        else if(chargeCallColor ==0)
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(notCharges));
            }
            if (OnToolSelected != null) OnToolSelected.Invoke();
        }

    }
    void CallColorOff()
    {
        callColorOn = false;
        OnToolSelected -= CallColorOff;
        OnToolSelected -= FaceNoActivated;
        Cm.OnToolUsed -= UseCallingColor;
    }
    void CallColorButtonOn()
    {
        callColorButton.GetComponent<Image>().sprite = callColorButtonOn ;
    }
    void CallColorButtonOff()
    {
        callColorButton.GetComponent<Image>().sprite = callColorButtonOff;
        OnToolSelected -= CallColorButtonOff;
    }
    void UseCallingColor(Cm cm)
    {
        if (cm.faced)
        {
            chargeCallColor--;
            Cm.callColorCharges.text = chargeCallColor.ToString();
            foreach (Cm tempcm in FindObjectsOfType<Cm>())
            {
                if (cm.color == tempcm.color)
                {
                    StartCoroutine(Calling(tempcm));
                }
            }            
        }
        else
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(needCardFace));
            }
            Cm.animating = false;
        }
        if (OnToolSelected != null) OnToolSelected.Invoke();

    }

    public void CallShapeOn()
    {
        if (chargeCallShape > 0 &&!Cm.animating)
        {
            if (OnToolSelected != null && callShapeOn)
            {
                OnToolSelected.Invoke();
            }
            else
            {
                if (OnToolSelected != null) OnToolSelected.Invoke();
                callShapeOn = true;
                FaceActivated();
                CallShapeButtonOn();
                OnToolSelected += CallShapeOff;
                OnToolSelected += FaceNoActivated;
                OnToolSelected += CallShapeButtonOff;
                Cm.OnToolUsed += UseCallingShape;
            }
        }
        else if(chargeCallShape ==0)
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(notCharges));
            }
            if (OnToolSelected != null) OnToolSelected.Invoke();
        }

    }
    void CallShapeOff()
    {
        callShapeOn = false;
        OnToolSelected -= CallShapeOff;
        OnToolSelected -= FaceNoActivated;
        Cm.OnToolUsed -= UseCallingShape;
    }
    void CallShapeButtonOn()
    {
        callShapeButton.GetComponent<Image>().sprite = callShapeButtonOn;
    }
    void CallShapeButtonOff()
    {
        callShapeButton.GetComponent<Image>().sprite = callShapeButtonOff;
        OnToolSelected -= CallShapeButtonOff;
    }
    void UseCallingShape(Cm cm)
    {
        if (cm.faced)
        {
            chargeCallShape--;
            Cm.callShapeCharges.text = chargeCallShape.ToString();
            foreach (Cm tempcm in FindObjectsOfType<Cm>())
            {
                if (cm.shape == tempcm.shape)
                {
                    StartCoroutine(Calling(tempcm));
                }
            }
        }
        else
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(needCardFace));
            }
            Cm.animating = false;
        }
        if (OnToolSelected != null) OnToolSelected.Invoke();

    }

    public void BombOn()
    {
        if (chargeBomb > 0  && !Cm.animating)
        {
            if (OnToolSelected != null && bombOn)
            {
                OnToolSelected.Invoke();
            }
            else
            {
                if (OnToolSelected != null) OnToolSelected.Invoke();
                bombOn = true;
                BombButtonOn();
                OnToolSelected += BombOff;
                OnToolSelected += BombButtonOff;
                Cm.OnToolUsed += UseBomb;
            }
        }

        else if(chargeBomb==0)
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(notCharges));
            }
            if (OnToolSelected != null) OnToolSelected.Invoke();
        }
    }
    void BombOff()
    {
        bombOn = false;
        OnToolSelected -= BombOff;
        Cm.OnToolUsed -= UseBomb;
    }
    void BombButtonOn()
    {
        bombButton.GetComponent<Image>().sprite = bombButtonOn;
    }
    void BombButtonOff()
    {
        bombButton.GetComponent<Image>().sprite = bombButtonOff;
        OnToolSelected -= BombButtonOff;
    }
    void UseBomb(Cm cm)
    {
        if (Cm.c1==null)
        {
            chargeBomb--;
            bombSound.Play();
            Cm.bombCharges.text = chargeBomb.ToString();
            int row = cm.position / Progress.RowCards;
            int colu = cm.position - row * Progress.RowCards;
            int minRow = row - 1 < 0 ? row : row - 1;
            int maxRow = row + 1 >= Progress.ColuCards ? row : row + 1;
            int minCol = colu - 1 < 0 ? colu : colu - 1;
            int maxCol = colu + 1 >= Progress.RowCards ? colu : colu + 1;
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int h = minCol; h <= maxCol; h++)
                {
                    Cm temp = i * Progress.RowCards + h == cm.position ? cm : cms.Find(c => c.position == i * Progress.RowCards + h);
                    if (!temp.faced) StartCoroutine(Explosion(temp));
                }
            }
        }
        else
        {
            if (TextDisplayed == null)
            {
                StartCoroutine(ShowAndHide(firstCardHide));
            }
            Cm.animating = false;
        }
        if (OnToolSelected != null) OnToolSelected.Invoke();
    }

    IEnumerator Explosion(Cm cm)
    {
        cm.OpenCard();
        yield return new WaitForSeconds(1.4f);
        cm.Resuma();
    }
    IEnumerator Calling(Cm cm)
    {
        cm.anim.SetBool("called", true);
        yield return new WaitForSeconds(2f + 0.25f * Progress.level);
        cm.anim.SetBool("called", false);
        Cm.animating = false;
    }


    void FaceActivated()
    {
        foreach (Cm cm in FindObjectsOfType<Cm>())
        {
            cm.GetComponent<GraphicRaycaster>().ignoreReversedGraphics = false;
        }
    }
    void FaceNoActivated()
    {
        foreach (Cm cm in FindObjectsOfType<Cm>())
        {
            cm.GetComponent<GraphicRaycaster>().ignoreReversedGraphics = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
