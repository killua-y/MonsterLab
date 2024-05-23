using System.Collections;
using System.Collections.Generic;
using events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnPlay : Manager<CardOnPlay>
{
    public CardContainer container;

    private void Start()
    {
    }

    public void OnCardPlayed(CardPlayed evt)
    {
        // 释放卡牌
        CardBehavior cardBehavior = evt.card.GetComponent<CardBehavior>();
        Tile tile = HelperFunction.GetTileUnder();
        if(tile != null)
        {
            cardBehavior.CheckLegality(GridManager.Instance.GetNodeForTile(tile));
        }
    }

    // The function to get multiple tiles based on user clicks
    public IEnumerator GetTiles(int n, System.Action<List<Tile>> callback)
    {
        List<Tile> clickedTiles = new List<Tile>();
        InGameStateManager.gamePased = true;

        while (clickedTiles.Count < n)
        {
            if (Input.GetMouseButtonDown(0)) // Check for mouse click
            {
                Tile clickedTile = HelperFunction.GetTileUnder();
                if (clickedTile != null && !clickedTiles.Contains(clickedTile))
                {
                    clickedTiles.Add(clickedTile);
                }
            }
            yield return null; // Wait for the next frame
        }

        InGameStateManager.gamePased = false;
        callback(clickedTiles); // Return the list of clicked tiles
    }
}
