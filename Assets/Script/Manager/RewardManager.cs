using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class RewardManager : Manager<RewardManager>
{
    public GameObject rewardCanvas;
    public GameObject rewardPanel;
    public GameObject Reward;

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
        InitializeDNARewardList();
    }

    private void InitializeCardRewardList()
    {
        // 只添加自己职业颜色和白色的卡
        foreach (Card card in CardDataModel.Instance.GetAllCard())
        {
            if ((card.color == CardColor.None) || (card.color == MainMenuBehavior.character))
            {
                allCard.Add(card);
            }
        }

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

    public void GenerateReward(int remainningTurn)
    {
        EnemyType enemyType = ActsManager.currentEnemyType;

        switch (enemyType)
        {
            case EnemyType.Normal:
                GenerateGoldReward(remainningTurn);
                GenerateReward(1, 0);
                break;

            case EnemyType.Elite:
                GenerateGoldReward(remainningTurn);
                GenerateReward(1, 1);
                break;

            case EnemyType.Boss:
                GenerateGoldReward(remainningTurn);
                GenerateReward(1, 0);
                GenerateCardReward(CardRarity.Legend);
                GenerateLegendDNAReward();
                break;

            default:
                break;
        }
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
        HelperFunction.Shuffle(legendCard);
        HelperFunction.Shuffle(rareCard);
        HelperFunction.Shuffle(normalCard);

        if (randomRange(randNumber, 1, 5))
        {
            // All Legend
            for (int i = 0; i < 3; i++)
            {
                cards.Add(legendCard[i]);
            }
        }
        else if (randomRange(randNumber, 6, 30))
        {
            // 2 normal
            for (int i = 0; i < 2; i++)
            {
                cards.Add(normalCard[i]);
            }
            // 1 rare
            cards.Add(rareCard[0]);
        }
        else if (randomRange(randNumber, 31, 60))
        {
            // All Rare
            for (int i = 0; i < 3; i++)
            {
                cards.Add(rareCard[i]);
            }
        }
        else if (randomRange(randNumber, 61, 90))
        {
            // All Normal
            for (int i = 0; i < 3; i++)
            {
                cards.Add(normalCard[i]);
            }
        }
        else if (randomRange(randNumber, 91, 100))
        {
            // 2 rare
            for (int i = 0; i < 2; i++)
            {
                cards.Add(rareCard[i]);
            }
            // 1 legend
            cards.Add(legendCard[0]);
        }
        else
        {
            Debug.Log("Should not get here");
            return;
        }

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<CardRewardBehavior>().SetUp(cards);
    }

    public void GenerateCardReward(CardRarity cardRarity)
    {
        int randNumber = Random.Range(1, 101);

        List<Card> cards = new List<Card>();

        switch (cardRarity)
        {
            case CardRarity.Normal:
                // Normal
                HelperFunction.Shuffle(normalCard);
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(normalCard[i]);
                }
                break;
            case CardRarity.Rare:
                HelperFunction.Shuffle(rareCard);
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(rareCard[i]);
                }
                break;
            case CardRarity.Legend:
                HelperFunction.Shuffle(legendCard);
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(legendCard[i]);
                }
                break;
            default:
                break;
        }

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<CardRewardBehavior>().SetUp(cards);
    }

    public void GenerateDNAReward()
    {
        int randNumber = Random.Range(1, 101);

        DNA dna = null;

        if (randomRange(randNumber, 1, 50))
        {
            if (rareDNA.Count == 0)
            {
                return;
            }

            // Rare
            dna = rareDNA[0];
            rareDNA.Remove(rareDNA[0]);
        }
        else if (randomRange(randNumber, 51, 100))
        {
            if (normalDNA.Count == 0)
            {
                return;
            }

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
            GameObject newReward = Instantiate(Reward, rewardPanel.transform);
            newReward.AddComponent<DNARewardBehavior>().SetUp(dna);
        }
    }

    public void GenerateLegendDNAReward()
    {
        DNA dna = null;

        // legend
        if (legendDNA.Count == 0)
        {
            return;
        }

        dna = legendDNA[0];
        rareDNA.Remove(legendDNA[0]);

        if (dna != null)
        {
            GameObject newReward = Instantiate(Reward, rewardPanel.transform);
            newReward.AddComponent<DNARewardBehavior>().SetUp(dna);
        }
    }

    public void GenerateGoldReward(int remainningTurn)
    {
        if (remainningTurn == 0)
        {
            return;
        }

        int gold = remainningTurn * 50;

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<GoldRewardBehavior>().SetUp(gold);
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
