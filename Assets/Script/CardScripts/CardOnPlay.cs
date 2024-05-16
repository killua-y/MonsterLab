using System.Collections;
using System.Collections.Generic;
using events;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOnPlay : MonoBehaviour
{
    public CardContainer container;
    public Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void OnCardPlayed(CardPlayed evt)
    {
        // 释放卡牌
        CardBehavior cardBehavior = evt.card.GetComponent<CardBehavior>();
        Tile tile = GetTileUnder();
        if(tile != null)
        {
            cardBehavior.CheckLegality(tile);
        }

        string cardName = cardBehavior.card.cardName;
        if (cardName != null)
        {
            Debug.Log("Cast Card: " + cardName);
        }
    }

    public Tile GetTileUnder()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100);

        if (hit.collider != null)
        {
            // return找到的tile
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }

        return null;
    }
}
