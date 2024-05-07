using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class SpellCardDisplay : MonoBehaviour
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
    public void InitializeCard(SpellCard _card)
    {
        if (_card.imageLocation != "")
        {
            CardPicture = Resources.Load<Image>(_card.imageLocation);
        }
        cost.text = Convert.ToString(_card.cost);
        nameText.text = _card.cardName;
        effectText.text = _card.effectText;
    }
}
