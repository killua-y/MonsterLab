using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class MonsterCardDisplay : CardDisplay
{
    public Transform rankParent;
    public GameObject rankPrefab;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI health;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI rangeText;

    //卡名 稀有度 种族 星级 召唤条件
    //攻击力 生命值 攻击距离 数值 技能描述
    //卡片位置 模型位置

    public GameObject equipedItemParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void UpdateCardView(Card _card)
    {
        if (_card is not MonsterCard)
        {
            Debug.Log("Card type wrong, should be monster card: " + _card.cardName);
            return;
        }

        MonsterCard cardModel = (MonsterCard)_card;

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

        generateEquipedItem(_card);

        base.UpdateCardView(_card);
    }

    public override void UpdateColor(Card _card, Card originalCard)
    {
        MonsterCard cardModel = (MonsterCard)_card;
        MonsterCard originalCardModel = (MonsterCard)originalCard;

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

        base.UpdateColor(_card, originalCard);
    }

    private void generateEquipedItem(Card _card)
    {
        MonsterCard card;

        if (_card is MonsterCard)
        {
            card = (MonsterCard)_card;
            if (card.equippedCard.Count == 0)
            {
                return;
            }
        }
        else
        {
            return;
        }

        if (_card.keyWords.Count == 0)
        {
            equipedItemParent.transform.position = keyWordParent.transform.position;
        }

        foreach (Card itemCard in card.equippedCard)
        {
            GameObject newItemCardExplain = Instantiate(keyWordPrefab, equipedItemParent.transform);

            newItemCardExplain.GetComponent<AdjustImageSize>().Setup(itemCard.cardName, itemCard.effectText);
        }
    }

    public override void ShowKeyWord()
    {
        if (!equipedItemParent.activeSelf)
        {
            equipedItemParent.SetActive(true);
        }

        base.ShowKeyWord();
    }

    public override void HideKeyWord()
    {
        if (equipedItemParent.activeSelf)
        {
            equipedItemParent.SetActive(false);
        }
        base.HideKeyWord();
    }

    public override void FlipKeyWord(bool reverse)
    {
        RectTransform keyWordParentRectTransform = equipedItemParent.GetComponent<RectTransform>();
        if (reverse)
        {
            if (keyWordParentRectTransform.localPosition.x > 0)
            {
                keyWordParentRectTransform.localPosition = new Vector2(-keyWordParentRectTransform.localPosition.x, keyWordParentRectTransform.localPosition.y);
            }
        }
        else
        {
            if (keyWordParentRectTransform.localPosition.x < 0)
            {
                keyWordParentRectTransform.localPosition = new Vector2(-keyWordParentRectTransform.localPosition.x, keyWordParentRectTransform.localPosition.y);
            }
        }

        base.FlipKeyWord(reverse);
    }
}
