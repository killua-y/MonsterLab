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
        // 查看费用是否合理
        if (PlayerStatesManager.Instance.GetRemainingCost() < card.cost)
        {
            return;
        }

        Node node = GridManager.Instance.GetNodeForTile(_tile);

        // 查看释放单位是否合理
        if ((card.castType == CastType.AllMonster) || (card.castType == CastType.EnemyMonster) || (card.castType == CastType.PlayerMonster))
        {
            // 如果当前格没有怪兽
            if (node.currentEntity == null)
            {
                return;
            }
        }

        // 合法，释放卡牌效果
        CastCard(_tile, _card);
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

    public virtual void CastComplete()
    {
        // 消耗费用
        if (card is not MonsterCard)
        {
            PlayerStatesManager.Instance.DecreaseCost(card.cost);
        }

        if (card is SpellCard)
        {
            // 丢弃
            InGameStateManager.Instance.DiscardOneCard(card);
            Destroy(this.gameObject);
        }
        else
        {
            // 消耗
            InGameStateManager.Instance.ExhaustOneCard(card);
            Destroy(this.gameObject);
        }
    }
}
