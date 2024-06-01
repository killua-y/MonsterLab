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

        HelperFunction.Shuffle(normalDNA, RandomManager.DNARewardRand);
        HelperFunction.Shuffle(rareDNA, RandomManager.DNARewardRand);
        HelperFunction.Shuffle(legendDNA, RandomManager.DNARewardRand);
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

        for (int i = 0; i < CardReward; i++)
        {
            GenerateCardReward(3);
        }

        for (int i = 0; i < DNAReward; i++)
        {
            GenerateDNAReward();
        }
    }

    public void GenerateCardReward(int number)
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < number; i++)
        {
            int newCardIndex = GetNextCardID();
            cards.Add(CardDataModel.Instance.GetCard(newCardIndex));
        }

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<CardRewardBehavior>().SetUp(cards);
    }

    public void GenerateCardReward(CardRarity cardRarity)
    {
        List<Card> cards = new List<Card>();

        switch (cardRarity)
        {
            case CardRarity.Normal:
                // Normal
                HelperFunction.Shuffle(normalCard, RandomManager.cardRewardRand);
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(normalCard[i]);
                }
                break;
            case CardRarity.Rare:
                HelperFunction.Shuffle(rareCard, RandomManager.cardRewardRand);
                for (int i = 0; i < 3; i++)
                {
                    cards.Add(rareCard[i]);
                }
                break;
            case CardRarity.Legend:
                HelperFunction.Shuffle(legendCard, RandomManager.cardRewardRand);
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
        int randNumber = RandomManager.DNARewardRand.Next(1, 101);

        DNA dna = GetNextDNA();

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

    // 种子随机数生成
    // Method to get a single card ID based on probabilities
    public int GetNextCardID()
    {
        float randomValue = (float)RandomManager.cardRewardRand.NextDouble();

        if (randomValue < 0.5f)
        {
            // 50% probability for normal cards
            return GetRandomCardID(normalCard);
        }
        else if (randomValue < 0.5f + 0.4f)
        {
            // 40% probability for rare cards
            return GetRandomCardID(rareCard);
        }
        else
        {
            // 10% probability for legend cards
            return GetRandomCardID(legendCard);
        }
    }

    private int GetRandomCardID(List<Card> cardList)
    {
        if (cardList.Count == 0)
        {
            throw new System.Exception("Card list is empty");
        }

        int index = RandomManager.cardRewardRand.Next(cardList.Count);
        return cardList[index].id;
    }

    public DNA GetNextDNA()
    {
        int randNumber = RandomManager.DNARewardRand.Next(1, 101);

        DNA dna = null;

        if (randomRange(randNumber, 1, 50))
        {
            if (rareDNA.Count == 0)
            {
                return null;
            }

            // Rare
            dna = rareDNA[0];
            rareDNA.Remove(rareDNA[0]);
        }
        else if (randomRange(randNumber, 51, 100))
        {
            if (normalDNA.Count == 0)
            {
                return null;
            }

            // Normal
            dna = normalDNA[0];
            normalDNA.Remove(normalDNA[0]);
        }
        else
        {
            Debug.Log("Should not get here");
            return null;
        }

        return dna;
    }
}