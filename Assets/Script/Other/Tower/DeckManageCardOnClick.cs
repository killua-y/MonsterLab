using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManageCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private int cardIndex;
    public bool isMainDeck = true;

    public void SetUp(int index, bool _isMainDeck)
    {
        cardIndex = index;
        isMainDeck = _isMainDeck;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ActsManager.Instance.ChangeDeckFromMainToExtra(cardIndex, isMainDeck, this.gameObject);
    }
}
