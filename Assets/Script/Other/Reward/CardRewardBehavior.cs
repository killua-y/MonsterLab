using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardRewardBehavior : MonoBehaviour, IPointerClickHandler
{
    private List<Card> cards;

    public void SetUp(List<Card> _cards)
    {
        cards = _cards;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardSelectBehavior.Instance.AddCard(cards, this.gameObject);
    }
}
