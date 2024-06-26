using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Card;

public class CardDisplay : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Image cardPicture;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public Image rarityGem;
    public GameObject keyWordParent;
    public GameObject keyWordPrefab;

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
            originalCard = CardDataModel.Instance.GetEnemyCard(_card.id);
        }
        else
        {
            originalCard = CardDataModel.Instance.GetCard(_card.id);
        }

        UpdateColor(_card, originalCard);
        generateKeyWord(_card);
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

    private void generateKeyWord(Card _card)
    {
        if (_card.keyWords.Count == 0)
        {
            return;
        }

        List<string> keyWordsDefinition = CardDataModel.Instance.keyWordsDefinition;
        List<string> keyWords = CardDataModel.Instance.keyWords;

        foreach (string keyword in _card.keyWords)
        {
            GameObject newKeyWordExplain = Instantiate(keyWordPrefab, keyWordParent.transform);

            string description = keyWordsDefinition[keyWords.IndexOf(keyword)];

            newKeyWordExplain.GetComponent<AdjustImageSize>().Setup(keyword, description);
        }
    }

    public virtual void ShowKeyWord()
    {
        if (!keyWordParent.activeSelf)
        {
            keyWordParent.SetActive(true);
        }

        // 查看鼠标是否在屏幕右1/4
        if (this.transform.position.x > Screen.width * 3 / 4)
        {
            FlipKeyWord(true);
        }
    }

    public virtual void HideKeyWord()
    {
        if (keyWordParent.activeSelf)
        {
            keyWordParent.SetActive(false);
        }
    }

    public virtual void FlipKeyWord(bool reverse)
    {
        RectTransform keyWordParentRectTransform = keyWordParent.GetComponent<RectTransform>();
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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowKeyWord();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideKeyWord();
    }
}
