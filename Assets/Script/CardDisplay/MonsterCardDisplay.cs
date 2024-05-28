using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class MonsterCardDisplay : CardDisplay
{
    public Image CardPicture;
    public TextMeshProUGUI costText;
    public Transform rankParent;
    public GameObject rankPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI health;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI rangeText;

    //卡名 稀有度 种族 星级 召唤条件
    //攻击力 生命值 攻击距离 数值 技能描述
    //卡片位置 模型位置

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateIndividualCardView(Card _card)
    {
        if (_card is not MonsterCard)
        {
            Debug.Log("Card type wrong, should be monster card: " + _card.cardName);
            return;
        }

        MonsterCard cardModel = (MonsterCard)_card;
        if (cardModel.imageLocation != "")
        {
            CardPicture = Resources.Load<Image>(cardModel.imageLocation);
        }
        costText.text = Convert.ToString(cardModel.cost);
        nameText.text = cardModel.cardName;
        effectText.text = cardModel.effectText;
        attack.text = Convert.ToString(cardModel.attackPower);
        health.text = Convert.ToString(cardModel.healthPoint);
        typeText.text = cardModel.type.ToString();
        rangeText.text = "range: " + (int)cardModel.attackRange;


        foreach (Transform child in rankParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < cardModel.rank; i++)
        {
            Instantiate(rankPrefab, rankParent);
        }
    }

    public override void UpdateColor(Card _card, Card originalCard)
    {
        MonsterCard cardModel = (MonsterCard)_card;
        MonsterCard originalCardModel = (MonsterCard)originalCard;

        // 费用
        if (cardModel.cost == originalCardModel.cost)
        {
            costText.color = Color.white;
        }
        else if (cardModel.cost < originalCardModel.cost)
        {
            costText.color = Color.green;
        }
        else
        {
            costText.color = Color.red;
        }

        // 攻击力
        if (cardModel.attackPower == originalCardModel.attackPower)
        {
            attack.color = Color.white;
        }
        else if (cardModel.attackPower > originalCardModel.attackPower)
        {
            attack.color = Color.green;
        }
        else
        {
            attack.color = Color.red;
        }

        // 生命值
        if (cardModel.healthPoint == originalCardModel.healthPoint)
        {
            health.color = Color.white;
        }
        else if (cardModel.healthPoint > originalCardModel.healthPoint)
        {
            health.color = Color.green;
        }
        else
        {
            health.color = Color.red;
        }
    }
}
