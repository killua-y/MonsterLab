using System;
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

    // 可视化单个卡牌，将卡牌的可视化后加入到parent下面，只被InGameStateManeger call
    public void DisPlaySingleCard(Card _card, Transform _parent)
    {
        if (_card is MonsterCard)
        {
            if (_card.scriptLocation == "")
            {
                GameObject newCard = GameObject.Instantiate(MonsterCardModel, _parent);
                CardContainer cardContainer = _parent.GetComponent<CardContainer>();
                if(cardContainer != null)
                {
                    cardContainer.InitCards();
                }

                // attach script
                newCard.AddComponent(Type.GetType("MonsterCardBehavior"));
                newCard.GetComponent<CardBehavior>().InitializeCard(_card);
            }
            else
            {
                Debug.Log("Card location is: " + _card.scriptLocation);
            }
        }
        else if(_card is SpellCard)
        {

            GameObject newCard = GameObject.Instantiate(SpellCardModel, _parent);
            CardContainer cardContainer = _parent.GetComponent<CardContainer>();
            if (cardContainer != null)
            {
                cardContainer.InitCards();
            }

            // attach script
            string cardScriptName = "CardBehavior";

            if (_card.scriptLocation != "")
            {
                cardScriptName = _card.scriptLocation;
            }

            newCard.AddComponent(Type.GetType(cardScriptName));
            newCard.GetComponent<CardBehavior>().InitializeCard(_card);
        }
        else if (_card is ItemCard)
        {
            GameObject newCard = GameObject.Instantiate(ItemCardModel, _parent);
            CardContainer cardContainer = _parent.GetComponent<CardContainer>();
            if (cardContainer != null)
            {
                cardContainer.InitCards();
            }

            // attach script
            string cardScriptName = "CardBehavior";

            if(_card.scriptLocation != "")
            {
                cardScriptName = _card.scriptLocation;
            }

            newCard.AddComponent(Type.GetType(cardScriptName));
            newCard.GetComponent<CardBehavior>().InitializeCard(_card);
        }
        else
        {
            Debug.Log("Card : " + _card.cardName + " is not any exist type");
        }

    }
}
