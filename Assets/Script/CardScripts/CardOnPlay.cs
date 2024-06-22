using System.Collections;
using System.Collections.Generic;
using events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnPlay : Singleton<CardOnPlay>
{
    public CardContainer container;

    private void Start()
    {
    }

    public void OnCardPlayed(CardPlayed evt)
    {
        // 释放卡牌
        CardBehavior cardBehavior = evt.card.GetComponent<CardBehavior>();
        if (cardBehavior.isValid)
        {
            Tile tile = HelperFunction.GetTileUnder();
            if (tile != null)
            {
                cardBehavior.CheckLegality(GridManager.Instance.GetNodeForTile(tile));
            }
        }
    }
}
