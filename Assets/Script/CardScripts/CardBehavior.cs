using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class CardBehavior : MonoBehaviour
{
    public bool targetCard;
    public CastType castType;
    public bool isValid = true;

    public Card card;

    protected Node targetNode;
    protected BaseEntity targetMonster;

    protected bool IsDragging = false;
    protected Tile previousTile = null;

    // Update is called once per frame
    void Update()
    {
        if (IsDragging)
        {
            Tile tileUnder = HelperFunction.GetTileUnder();
            if (tileUnder != null)
            {
                // 根据自己卡片的释放类型决定下方格子是否合法
                switch (castType)
                {
                    case CastType.None:
                        isValid = true;
                        return;

                    case CastType.AllMonster:
                        isValid = GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied;
                        break;

                    case CastType.EnemyMonster:
                        isValid = ((GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied)
                            && (!GridManager.Instance.GetNodeForTile(tileUnder).IsPlayerArea));
                        break;

                    case CastType.PlayerMonster:
                        isValid = ((GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied)
                            && (GridManager.Instance.GetNodeForTile(tileUnder).IsPlayerArea));
                        break;

                    case CastType.PlayerEmptyTile:
                        isValid = ((!GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied)
                            && (GridManager.Instance.GetNodeForTile(tileUnder).IsPlayerArea));
                        break;

                    case CastType.AllEmptyTile:
                        isValid = (!GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied);
                        break;

                    default:
                        isValid = false;
                        Debug.LogWarning("Unknown CastType");
                        break;
                }

                tileUnder.SetHighlight(true, isValid);

                if (previousTile != null && tileUnder != previousTile)
                {
                    //We are over a different tile.
                    previousTile.SetHighlight(false, false);
                }

                previousTile = tileUnder;
            }
        }
    }

    public virtual void InitializeCard(Card _card)
    {
        card = _card;
        castType = card.castType;

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

    public virtual void CheckLegality(Node node)
    {
        // 查看费用是否合理
        if (PlayerCostManager.Instance.GetRemainingCost() < card.cost)
        {
            return;
        }

        targetMonster = node.currentEntity;

        // 合法，释放卡牌效果
        //Debug.Log("Cast Card: " + card.cardName);
        CastCard(node);

        // 释放结束
        CastComplete(node);
    }

    public virtual void OnPointDown()
    {
        IsDragging = true;
        this.GetComponent<CardDisplay>().HideKeyWord();
    }

    public virtual void OnPointUp()
    {
        IsDragging = false;
        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
        }
    }

    public virtual void CastCard(Node node)
    {
        Debug.Log("Please attach correspond card behavior srcipt to this card: " + card.cardName);
    }

    public virtual void CastComplete(Node node)
    {
        // 消耗费用
        PlayerCostManager.Instance.DecreaseCost(card.cost);

        // 广播释放魔法/装备这个动作
        if (card is SpellCard)
        {
            InGameStateManager.Instance.SpellCardPlayed(this, targetMonster);
        }
        else if (card is ItemCard)
        {
            InGameStateManager.Instance.ItemCardPlayed(this, targetMonster);
        }

        // 判断卡牌是丢弃还是消耗
        if (card is SpellCard)
        {
            if (card.keyWords.Contains("Exhaust"))
            {
                // 消耗
                InGameStateManager.Instance.ExhaustOneCard(card);
                Destroy(this.gameObject);
            }
            else
            {
                // 丢弃
                InGameStateManager.Instance.DiscardOneCard(card);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (card.keyWords.Contains("Reuse"))
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
}
