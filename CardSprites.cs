using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Card", menuName = "cards")]
public class CardSprites : ScriptableObject{

    public Sprite spriteCard;
    public CardColor color;
    public CardShape shape;
}
