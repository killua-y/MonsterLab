using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class ItemCardDisplay : CardDisplay
{
    public Image CardPicture;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 将怪兽卡数据导入模型
    public override void UpdateIndividualCardView(Card _card)
    {
        if (_card is not ItemCard)
        {
            Debug.Log("Card type wrong, should be item card: " + _card.cardName);
            return;
        }

        ItemCard cardModel = (ItemCard)_card;
        if (cardModel.imageLocation != "")
        {
            CardPicture = Resources.Load<Image>(cardModel.imageLocation);
        }
        costText.text = Convert.ToString(cardModel.cost);
        nameText.text = cardModel.cardName;
        effectText.text = cardModel.effectText;
    }

    public override void UpdateColor(Card _card, Card originalCard)
    {
        // 费用
        if (_card.cost == originalCard.cost)
        {
            costText.color = Color.white;
        }
        else if (_card.cost < originalCard.cost)
        {
            costText.color = Color.green;
        }
        else
        {
            costText.color = Color.red;
        }
    }
}
