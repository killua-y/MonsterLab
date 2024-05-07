using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class MonsterCardDisplay : MonoBehaviour
{
    public Image CardPicture;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI health;
    public TextMeshProUGUI typeText;

    //卡名 稀有度 种族 星级 召唤条件
    //攻击力 生命值 攻击距离 数值 技能描述
    //卡片位置 模型位置

    private MonsterCard cardModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 将怪兽卡数据导入模型
    public void InitializeCard(MonsterCard _card)
    {
        cardModel = _card;
        if (_card.imageLocation != "")
        {
            CardPicture = Resources.Load<Image>(_card.imageLocation);
        }
        nameText.text = _card.cardName;
        effectText.text = _card.effectText;
        attack.text = Convert.ToString(_card.attack);
        health.text = Convert.ToString(_card.healthPoint);
        typeText.text = _card.type.ToString();
    }
}
