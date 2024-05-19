using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class RewardManager : MonoBehaviour
{

    private CardDataModel cardDataModel;

    // 不区分卡牌种类
    public List<Card> allCard;
    public List<Card> normalCard;
    public List<Card> rareCard;
    public List<Card> legendCard;
    // Start is called before the first frame update
    void Start()
    {
        cardDataModel = FindObjectOfType<CardDataModel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeRewardList()
    {
        allCard = cardDataModel.GetAllCard();
        foreach (Card card in allCard)
        {
            switch (card.cardRarity)
            {
                case CardRarity.Normal:
                    // 普通卡池
                    normalCard.Add(card);
                    break;

                case CardRarity.Rare:
                    // 稀有卡池
                    rareCard.Add(card);
                    break;

                case CardRarity.Legend:
                    // 传说卡池
                    legendCard.Add(card);
                    break;

                default:
                    // Code for any other cases (if any)
                    break;
            }
        }
    }

    public void GenerateReward()
    {

    }
}
