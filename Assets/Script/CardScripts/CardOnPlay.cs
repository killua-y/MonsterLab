using System.Collections;
using System.Collections.Generic;
using events;
using UnityEngine;

public class CardOnPlay : MonoBehaviour
{
    public CardContainer container;
    public void OnCardPlayed(CardPlayed evt)
    {
        string cardName = evt.card.GetComponent<CardBehavior>().card.cardName;
        if (cardName != null)
        {
            Debug.Log("Cast Card: " + cardName);
        }
    }
}
