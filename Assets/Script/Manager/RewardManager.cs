using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class RewardManager : MonoBehaviour
{
    public GameObject rewardCanvas;
    public GameObject rewardPanel;
    public GameObject cardReward;
    private CardDataModel cardDataModel;

    // 不区分卡牌种类
    public List<Card> allCard = new List<Card>();
    public List<Card> normalCard = new List<Card>();
    public List<Card> rareCard = new List<Card>();
    public List<Card> legendCard = new List<Card>();
    // Start is called before the first frame update
    void Start()
    {
        cardDataModel = FindObjectOfType<CardDataModel>();
        InitializeRewardList();
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
        rewardCanvas.SetActive(true);

        GenerateCardReward();

        GenerateCardReward();
    }

    public void GenerateCardReward()
    {
        int randNumber = Random.Range(1, 101);

        List<Card> cards = new List<Card>();

        if (randomRange(randNumber, 1, 30))
        {
            // Legend
            InGameCardModel.Shuffle(legendCard);
            for (int i = 0; i < 3; i++)
            {
                cards.Add(legendCard[i]);
            }
        }
        else if (randomRange(randNumber, 31, 60))
        {
            // Rare
            InGameCardModel.Shuffle(rareCard);
            for (int i = 0; i < 3; i++)
            {
                cards.Add(rareCard[i]);
            }
        }
        else if (randomRange(randNumber, 61, 100))
        {
            // Normal
            InGameCardModel.Shuffle(normalCard);
            for (int i = 0; i < 3; i++)
            {
                cards.Add(normalCard[i]);
            }
        }
        else
        {
            Debug.Log("Should not get here");
            return;
        }

        GameObject newReward = Instantiate(cardReward, rewardPanel.transform);
        newReward.GetComponent<CardRewardBehavior>().AddCard(cards);
    }

    bool randomRange(int test, int min, int max)
    {
        if ((test >= min) && (test <= max))
        {
            return true;
        }

        return false;
    }
}
