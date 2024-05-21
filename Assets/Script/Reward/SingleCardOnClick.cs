using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleCardOnClick : MonoBehaviour, IPointerClickHandler
{
    private int cardIndex;
    private GameObject rewardParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CardDataModel cardDataModel = FindObjectOfType<CardDataModel>();
        cardDataModel.ObtainCard(cardIndex);
        Destroy(rewardParent);
    }

    public void SetUp(int _cardIdex, GameObject _parent)
    {
        cardIndex = _cardIdex;
        rewardParent = _parent;
    }
}
