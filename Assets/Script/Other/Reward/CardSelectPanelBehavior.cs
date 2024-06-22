using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectPanelBehavior : MonoBehaviour
{
    public GameObject panel;
    public Transform DeckContent;
    public List<Card> resultCardList;

    public void SelectCardFromDeck(int amount, Action<List<Card>> callback)
    {
        StartCoroutine(SelectCard(amount, callback));
    }

    public void SelectCardFromDeck(Action<Card> callback)
    {
        StartCoroutine(SelectCard(1, cards => callback(cards[0])));
    }

    private IEnumerator SelectCard(int amount, Action<List<Card>> callback)
    {
        resultCardList = new List<Card>();

        OpenPlayerDeck();
        while (resultCardList.Count < amount)
        {
            if (Input.GetMouseButtonDown(1)) // Check for mouse click
            {
                resultCardList = new List<Card>();
            }
            yield return null; // Wait for the next frame
        }

        CloseDeck();
        callback(resultCardList);
    }

    public void OpenPlayerDeck()
    {
        List<Card> cardList = new List<Card>();
        cardList = CardDataModel.Instance.GetPlayerDeck();

        foreach (Card card in cardList)
        {
            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, DeckContent);
            newCard.AddComponent<Scaling>();
            newCard.AddComponent<CardSelectOnClick>().SetUp(card);
        }

        panel.SetActive(true);
    }

    public void CloseDeck()
    {
        foreach (Transform child in DeckContent)
        {
            Destroy(child.gameObject);
        }

        panel.SetActive(false);
    }
}


public class CardSelectOnClick : MonoBehaviour, IPointerClickHandler
{
    private Card card;

    // 引用的script
    private CardSelectPanelBehavior cardSelectPanelBehavior;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (cardSelectPanelBehavior == null)
            {
                cardSelectPanelBehavior = FindAnyObjectByType<CardSelectPanelBehavior>();
            }

            if (cardSelectPanelBehavior.resultCardList.Contains(card))
            {
                cardSelectPanelBehavior.resultCardList.Remove(card);
            }
            else
            {
                cardSelectPanelBehavior.resultCardList.Add(card);
            }
        }
    }

    public void SetUp(Card _card)
    {
        card = _card;
    }
}