using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckManage : Singleton<DeckManage>
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

        mainDeck = CardDataModel.Instance.GetMainDeck();

        foreach (Card card in mainDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, mainDeckScrollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card, true);
        }
    }

    private void ShowExtraDeck()
    {
        foreach (Transform child in extraDeckScollContent.transform)
        {
            Destroy(child.gameObject);
        }

        extraDeck = CardDataModel.Instance.GetExtraDeck();
        foreach (Card card in extraDeck)
        {
            GameObject cardObject = CardDisplayView.Instance.DisPlaySingleCard(card, extraDeckScollContent);
            cardObject.AddComponent<Scaling>();
            cardObject.AddComponent<DeckManageCardOnClick>().SetUp(card, false);
            //Debug.Log("Get card with index : " + card.id);
        }
    }

    public void ChangeDeckFromMainToExtra(Card card, bool fromMainToExtra, GameObject cardObject)
    {
        if (InGameStateManager.inCombat)
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

        CardDataModel.Instance.ChangeDeckFromMainToExtra(card, fromMainToExtra);
    }
}

public class DeckManageCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    public bool isMainDeck = true;

    public void SetUp(Card _card, bool _isMainDeck)
    {
        card = _card;
        isMainDeck = _isMainDeck;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DeckManage.Instance.ChangeDeckFromMainToExtra(card, isMainDeck, this.gameObject);
        }
    }
}
