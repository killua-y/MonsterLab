using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManage : Manager<DeckManage>
{
    [Header("Main deck Extra Deck")]
    public Transform Deck;
    public Transform mainDeckScrollContent;
    public Transform extraDeckScollContent;

    private List<Card> mainDeck;
    private List<Card> extraDeck;

    public void OpenDeck()
    {
        if (Deck.gameObject.activeSelf)
        {
            Deck.gameObject.SetActive(false);
        }
        else
        {
            ShowMainDeck();
            ShowExtraDeck();
            Deck.gameObject.SetActive(true);
        }
    }

    private void ShowMainDeck()
    {
        foreach (Transform child in mainDeckScrollContent.transform)
        {
            Destroy(child.gameObject);
        }

        mainDeck = CardDataModel.Instance.InitializeDeck();
        foreach (Card card in mainDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, mainDeckScrollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card.id, true);
        }
    }

    private void ShowExtraDeck()
    {
        foreach (Transform child in extraDeckScollContent.transform)
        {
            Destroy(child.gameObject);
        }

        extraDeck = CardDataModel.Instance.InitializeExtraDeck();
        foreach (Card card in extraDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, extraDeckScollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card.id, false);
            //Debug.Log("Get card with index : " + card.id);
        }
    }

    public void ChangeDeckFromMainToExtra(int cardIndex, bool fromMainToExtra, GameObject cardObject)
    {
        if (InGameStateManager.inGame)
        {
            return;
        }

        if (fromMainToExtra)
        {
            cardObject.transform.SetParent(extraDeckScollContent);
            cardObject.GetComponent<DeckManageCardOnClick>().isMainDeck = false;
        }
        else
        {
            cardObject.transform.SetParent(mainDeckScrollContent);
            cardObject.GetComponent<DeckManageCardOnClick>().isMainDeck = true;
        }

        CardDataModel.Instance.ChangeDeckFromMainToExtra(cardIndex, fromMainToExtra);
    }
}

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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManage.Instance.ChangeDeckFromMainToExtra(cardIndex, isMainDeck, this.gameObject);
        }
    }
}
