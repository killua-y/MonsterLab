using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectBehavior : MonoBehaviour
{
    public static CardSelectBehavior Instance;
    public Transform CardHolder;

    private void Awake()
    {
        Instance = this;
        this.gameObject.SetActive(false);
    }

    public void AddCard(List<Card> cards, GameObject reawrdParent)
    {
        this.gameObject.SetActive(true);

        foreach (Transform child in CardHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, CardHolder);
            newCard.AddComponent<Scaling>();
            newCard.AddComponent<RewardCardOnClick>();
            newCard.GetComponent<RewardCardOnClick>().SetUp(card.id, reawrdParent);
        }
    }

    public void FinishCardSelect()
    {
        this.gameObject.SetActive(false);
    }
}
