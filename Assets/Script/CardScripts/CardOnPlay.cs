using events;
using UnityEngine;

public class CardOnPlay : MonoBehaviour
{
    public CardContainer container;

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
