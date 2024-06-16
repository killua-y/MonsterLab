using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardSelectBehavior : MonoBehaviour
{
    public static RewardCardSelectBehavior Instance;
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
            newCard.GetComponent<RewardCardOnClick>().SetUp(card, reawrdParent);
        }
    }

    public void FinishCardSelect()
    {
        this.gameObject.SetActive(false);
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
        RewardCardSelectBehavior.Instance.FinishCardSelect();
        Destroy(rewardParent);
    }

    public void SetUp(Card _card, GameObject _parent)
    {
        card = _card;
        rewardParent = _parent;
    }
}