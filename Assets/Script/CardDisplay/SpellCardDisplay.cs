using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class SpellCardDisplay : CardDisplay
{
    public Image CardPicture;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI cost;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 将怪兽卡数据导入模型
    public override void UpdateCardView(Card _card)
    {
        if (_card is not SpellCard)
        {
            Debug.Log("Card type wrong, should be item card: " + _card.cardName);
            return;
        }

        SpellCard cardModel = (SpellCard)_card;

        if (cardModel.imageLocation != "")
        {
            CardPicture = Resources.Load<Image>(cardModel.imageLocation);
        }
        cost.text = Convert.ToString(cardModel.cost);
        nameText.text = cardModel.cardName;
        effectText.text = cardModel.effectText;
    }
}
