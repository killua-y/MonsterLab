using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardSelectBehavior : MonoBehaviour
{
    public GameObject RewardCardSelectObject;
    public Transform CardHolder;

    public void AddCard(List<Card> cards, GameObject reawrdParent)
    {
        RewardCardSelectObject.SetActive(true);

        foreach (Transform child in CardHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in cards)
        {
            GameObject newCard = CardDisplayView.Instance.DisPlaySingleCard(card, CardHolder);
            newCard.AddComponent<Scaling>();
            newCard.AddComponent<RewardCardOnClick>();
            newCard.GetComponent<RewardCardOnClick>().SetUp(card, reawrdParent);
        }
    }

    public void FinishCardSelect()
    {
        RewardCardSelectObject.SetActive(false);
    }
}

public class RewardCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private Card card;
    private GameObject rewardParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        CardDataModel cardDataModel = FindObjectOfType<CardDataModel>();
        cardDataModel.ObtainCard(card);
        FindAnyObjectByType<RewardCardSelectBehavior>().FinishCardSelect();
        Destroy(rewardParent);
    }

    public void SetUp(Card _card, GameObject _parent)
    {
        card = _card;
        rewardParent = _parent;
    }
}