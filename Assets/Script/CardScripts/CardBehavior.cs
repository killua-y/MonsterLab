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

    public Card cardModel;

    protected Node targetNode;
    protected BaseEntity targetMonster;

    protected bool IsDragging = false;
    protected Tile previousTile = null;

    // 引用的script
    protected PlayerCostManager playerCostManager;

    private void Start()
    {
        playerCostManager = FindAnyObjectByType<PlayerCostManager>();
    }

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

                    case CastType.PlayerArea:
                        isValid = (GridManager.Instance.GetNodeForTile(tileUnder).IsPlayerArea);
                        break;

                    case CastType.EnemyArea:
                        isValid = (!GridManager.Instance.GetNodeForTile(tileUnder).IsPlayerArea);
                        break;

                    default:
                        isValid = false;
                        Debug.LogWarning("Unknown CastType");
                        break;
                }

                if (tileUnder != previousTile)
                {
                    //We are over a different tile.
                    SetHighlight(tileUnder);
                }
            }
        }
    }

    protected virtual void SetHighlight(Tile tileUnder)
    {
        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
        }
        tileUnder.SetHighlight(true, isValid);
        previousTile = tileUnder;
    }

    public virtual void InitializeCard(Card _card)
    {
        cardModel = _card;
        castType = cardModel.castType;

        // 设置卡牌释放类型
        if (cardModel.castType == CastType.None)
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
        if (playerCostManager.currentCost < cardModel.cost)
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
        Debug.Log("No thing happened for card : " + cardModel.cardName);
    }

    public virtual void CastComplete(Node node)
    {
        // 消耗费用
        playerCostManager.DecreaseCost(cardModel.cost);

        // 广播释放魔法/装备这个动作
        if (cardModel is SpellCard)
        {
            InGameStateManager.Instance.SpellCardPlayed(this, targetMonster);
        }
        else if (cardModel is ItemCard)
        {
            InGameStateManager.Instance.ItemCardPlayed(this, targetMonster);
        }

        // 判断卡牌是丢弃还是消耗
        if (cardModel is SpellCard)
        {
            if (cardModel.keyWords.Contains("Exhaust"))
            {
                // 消耗
                InGameStateManager.Instance.ExhaustOneCard(cardModel);
                Destroy(this.gameObject);
            }
            else
            {
                // 丢弃
                InGameStateManager.Instance.DiscardOneCard(cardModel);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (cardModel.keyWords.Contains("Reuse"))
            {
                // 丢弃
                InGameStateManager.Instance.DiscardOneCard(cardModel);
                Destroy(this.gameObject);
            }
            else
            {
                // 消耗
                InGameStateManager.Instance.ExhaustOneCard(cardModel);
                Destroy(this.gameObject);
            }
        }
    }

    public virtual void RecordCast(BaseEntity baseEntity)
    {
        baseEntity.cardModel.equippedCard.Add(cardModel);

        // 如果装备卡中有关键词则添加到怪兽卡上
        if (cardModel.keyWords.Count != 0)
        {
            foreach (string keyword in cardModel.keyWords)
            {
                if (!baseEntity.cardModel.keyWords.Contains(keyword))
                {
                    baseEntity.cardModel.keyWords.Add(keyword);
                }
            }
        }

    }
}
