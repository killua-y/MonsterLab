using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Image cardPicture;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public Image rarityGem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void UpdateCardView(Card _card)
    {
        if (_card.imageLocation != "")
        {
            cardPicture.sprite = Resources.Load<Sprite>(_card.imageLocation);
        }
        costText.text = Convert.ToString(_card.cost);
        nameText.text = _card.cardName;
        effectText.text = _card.effectText;

        // 更新卡牌稀有度
        if (rarityGem.sprite == null)
        {
            switch (_card.cardRarity)
            {
                case CardRarity.Normal:
                    rarityGem.sprite = Resources.Load<Sprite>("UI/Card/rarityGem-gray");
                    break;
                case CardRarity.Rare:
                    rarityGem.sprite = Resources.Load<Sprite>("UI/Card/rarityGem-blue");
                    break;
                case CardRarity.Legend:
                    rarityGem.sprite = Resources.Load<Sprite>("UI/Card/rarityGem-orange");
                    break;
                default:
                    Debug.Log("This card do not have a card rarity");
                    break;
            }
        }

        Card originalCard;
        if (_card.color == CardColor.Black)
        {
            originalCard = TurnManager.Instance.monsterList[_card.id];
        }
        else
        {
            originalCard = CardDataModel.Instance.GetCard(_card.id);
        }

        UpdateColor(_card, originalCard);
    }

    public virtual void UpdateColor(Card _card, Card originalCard)
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
