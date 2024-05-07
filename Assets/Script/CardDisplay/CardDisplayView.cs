using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class CardDisplayView : MonoBehaviour
{
    public GameObject MonsterCardModel;

    public GameObject SpellCardModel;

    public GameObject ItemCardModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 可视化单个卡牌，将卡牌的可视化后加入到parent下面
    public void DisPlaySingleCard(Card _card, Transform _parent)
    {
        if (_card is MonsterCard)
        {
            if (_card.cardLocation == "")
            {
                GameObject newCard = GameObject.Instantiate(MonsterCardModel, _parent); ;
                newCard.GetComponent<MonsterCardDisplay>().InitializeCard((MonsterCard)_card);
                //Debug.Log("Creating new monster card object for " + _card.cardName);
                newCard.GetComponent<CardBehavior>().InitializeCard(_card);
            }
            else
            {
                Debug.Log("Card location is: " + _card.cardLocation);
            }
        }
        else if(_card is SpellCard)
        {
            if (_card.cardLocation == "")
            {
                GameObject newCard = GameObject.Instantiate(SpellCardModel, _parent); ;
                newCard.GetComponent<SpellCardDisplay>().InitializeCard((SpellCard)_card);
                //Debug.Log("Creating new spell card object for " + _card.cardName);
                newCard.GetComponent<CardBehavior>().InitializeCard(_card);
            }
            else
            {
                Debug.Log("Card location is: " + _card.cardLocation);
            }
        }
        else if (_card is ItemCard)
        {
            if (_card.cardLocation == "")
            {
                GameObject newCard = GameObject.Instantiate(ItemCardModel, _parent); ;
                newCard.GetComponent<ItemCardDisplay>().InitializeCard((ItemCard)_card);
                //Debug.Log("Creating new item card object for " + _card.cardName);
                newCard.GetComponent<CardBehavior>().InitializeCard(_card);
            }
            else
            {
                Debug.Log("Card location is: " + _card.cardLocation);
            }
        }
        else
        {
            Debug.Log("Card : " + _card.cardName + " is not any exist type");
        }

    }
}
