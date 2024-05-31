using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private int cardIndex;
    private GameObject rewardParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        CardDataModel cardDataModel = FindObjectOfType<CardDataModel>();
        cardDataModel.ObtainCard(cardIndex);
        CardSelectBehavior.Instance.FinishCardSelect();
        Destroy(rewardParent);
    }

    public void SetUp(int _cardIdex, GameObject _parent)
    {
        cardIndex = _cardIdex;
        rewardParent = _parent;
    }
}
