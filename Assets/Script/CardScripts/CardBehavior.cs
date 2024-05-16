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

    public virtual void InitializeCard(Card _card)
    {
        card = _card;

        GetComponent<CardDisplay>().UpdateCardView(card);

        // 设置卡牌释放类型
        if (card.castType == CastType.None)
        {
            targetCard = false;
        }
        else
        {
            targetCard = true;
        }
    }

    public virtual void CheckLegality(Tile _tile, Card _card = null)
    {
        if (PlayerStatesManager.Instance.GetRemainingCost() >= card.cost)
        {
            // 合法，释放卡牌效果
            CastCard(_tile, _card);
        }
    }

    public virtual void OnPointDown()
    {
        
    }

    public virtual void OnPointUp()
    {

    }

    public virtual void CastCard(Tile _tile, Card _card = null)
    {
        Debug.Log("Please attach correspond card behavior srcipt to this card: " + card.cardName);
    }
}
