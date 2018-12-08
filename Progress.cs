using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress : MonoBehaviour {

    public static int Goal
    {
        get
        {
            return 10 + (level - 1) * 5 + (level - 1) * (level - 2) * 4 + (level - 1) * (level - 2) * (level - 3) * 2 + (level - 1) * (level - 2) * (level - 3) * (level - 4)  ;
        }
    }
    public static int level = 1;
    public static int RowCards
    {
        get
        {
            return 4 + level / 2;
        }
    }

    public static int ColuCards
    {
        get
        {
            return 3 + level / 2;
        }
    }

    public static int PairsToEndLevel
    {
        get
        {
            return RowCards*ColuCards/ 2;
        }
    }

    public List<Vector2> paddings = new List<Vector2>();
    public List<Vector2> cardSize = new List<Vector2>();

    public float SetPaddingX(int level)
    {
        return paddings[-1 + (level +2)/2].x;
    }

    public float SetPaddingY(int level)
    {
        return paddings[-1 + (level + 2) / 2].y;
    }

    public void SetSize(RectTransform rect, int level)
    {
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardSize[-1 + (level + 2) / 2].x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cardSize[-1 + (level + 2) / 2].y);
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
