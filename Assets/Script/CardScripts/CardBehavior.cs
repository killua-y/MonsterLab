using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class CardBehavior : MonoBehaviour
{
    public bool targetCard;

    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeCard(Card _card)
    {
        card = _card;

        // 设置卡牌释放类型
        if (card is MonsterCard)
        {
            targetCard = true;
        }
        else if (card is ItemCard)
        {
            targetCard = true;
        }
        else if (card is SpellCard)
        {
            targetCard = false;
        }
    }
}
