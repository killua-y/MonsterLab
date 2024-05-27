using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class RewardManager : Manager<RewardManager>
{
    public GameObject rewardCanvas;
    public GameObject rewardPanel;
    public GameObject cardReward;
    public GameObject dnaReward;

    // 不区分卡牌种类
    public List<Card> allCard = new List<Card>();
    public List<Card> normalCard = new List<Card>();
    public List<Card> rareCard = new List<Card>();
    public List<Card> legendCard = new List<Card>();

    // DNAList
    public List<DNA> allDNA = new List<DNA>();
    public List<DNA> normalDNA = new List<DNA>();
    public List<DNA> rareDNA = new List<DNA>();
    public List<DNA> legendDNA = new List<DNA>();
    // Start is called before the first frame update
    void Start()
    {
        InitializeCardRewardList();
    }

    private void InitializeCardRewardList()
    {
        allCard = CardDataModel.Instance.GetAllCard();
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

    private void InitializeDNARewardList()
    {
        // 清空DNA卡池
        allDNA = new List<DNA>();
        normalDNA = new List<DNA>();
        rareDNA = new List<DNA>();
        legendDNA = new List<DNA>();

        allDNA = CardDataModel.Instance.GetAllDNA();
        List<DNA> playerDNAList = CardDataModel.Instance.GetPlayerDNA();

        // 将玩家已经拥有的DNA从奖池中移除
        foreach (DNA playerDNA in playerDNAList)
        {
            allDNA.RemoveAll(dna => dna.id == playerDNA.id);
        }

        foreach (DNA dna in allDNA)
        {
            switch (dna.DNARarity)
            {
                case CardRarity.Normal:
                    // 普通卡池
                    normalDNA.Add(dna);
                    break;

                case CardRarity.Rare:
                    // 稀有卡池
                    rareDNA.Add(dna);
                    break;

                case CardRarity.Legend:
                    // 传说卡池
                    legendDNA.Add(dna);
                    break;

                default:
                    // Code for any other cases (if any)
                    break;
            }
        }

        HelperFunction.Shuffle(normalDNA);
        HelperFunction.Shuffle(rareDNA);
        HelperFunction.Shuffle(legendDNA);
    }

    public void GenerateReward(int CardReward, int DNAReward)
    {
        rewardCanvas.SetActive(true);

        for (int i = 0; i < CardReward; i ++)
        {
            GenerateCardReward();
        }

        for (int i = 0; i < DNAReward; i++)
        {
            GenerateDNAReward();
        }
    }

    public void GenerateCardReward()
    {
        int randNumber = Random.Range(1, 101);

        List<Card> cards = new List<Card>();

        if (randomRange(randNumber, 1, 30))
        {
            // Legend
            HelperFunction.Shuffle(legendCard);
            for (int i = 0; i < 3; i++)
            {
                cards.Add(legendCard[i]);
            }
        }
        else if (randomRange(randNumber, 31, 60))
        {
            // Rare
            HelperFunction.Shuffle(rareCard);
            for (int i = 0; i < 3; i++)
            {
                cards.Add(rareCard[i]);
            }
        }
        else if (randomRange(randNumber, 61, 100))
        {
            // Normal
            HelperFunction.Shuffle(normalCard);
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

    public void GenerateDNAReward()
    {
        // 更新DNA奖池
        InitializeDNARewardList();

        int randNumber = Random.Range(1, 101);

        DNA dna = null;

        if (randomRange(randNumber, 1, 30))
        {
            // Legend
            dna = legendDNA[0];
            legendDNA.Remove(legendDNA[0]);
        }
        else if (randomRange(randNumber, 31, 60))
        {
            // Rare
            dna = rareDNA[0];
            rareDNA.Remove(rareDNA[0]);
        }
        else if (randomRange(randNumber, 61, 100))
        {
            // Normal
            dna = normalDNA[0];
            normalDNA.Remove(normalDNA[0]);
        }
        else
        {
            Debug.Log("Should not get here");
            return;
        }

        if (dna != null)
        {
            GameObject newReward = Instantiate(dnaReward, rewardPanel.transform);
            newReward.GetComponent<DNARewardBehavior>().SetUp(dna);
        }
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
