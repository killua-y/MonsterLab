using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class RewardManager : Singleton<RewardManager>
{
    public GameObject rewardCanvas;
    public GameObject rewardPanel;
    public GameObject Reward;

    // 不区分卡牌种类
    public List<Card> allCard = new List<Card>();
    public List<Card> normalCard = new List<Card>();
    public List<Card> rareCard = new List<Card>();
    public List<Card> legendCard = new List<Card>();

    //
    private int cardRewardOptionNumber = 3;

    // DNAList
    public List<DNA> allDNA = new List<DNA>();
    public List<DNA> normalDNA = new List<DNA>();
    public List<DNA> rareDNA = new List<DNA>();
    public List<DNA> legendDNA = new List<DNA>();

    public void LoadData()
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

        HelperFunction.Shuffle(normalCard, GameSetting.randForInitialize);
        HelperFunction.Shuffle(rareCard, GameSetting.randForInitialize);
        HelperFunction.Shuffle(legendCard, GameSetting.randForInitialize);
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


        HelperFunction.Shuffle(normalDNA, GameSetting.randForInitialize);
        HelperFunction.Shuffle(rareDNA, GameSetting.randForInitialize);
        HelperFunction.Shuffle(legendDNA, GameSetting.randForInitialize);
    }

    public void Finish()
    {
        foreach (Transform child in rewardPanel.transform)
        {
            Destroy(child.gameObject);
        }
        rewardCanvas.gameObject.SetActive(false);
        ActsManager.Instance.LeaveScene();
    }

    public void GenerateReward(int remainningTurn)
    {
        BoxType enemyType = ActsManager.currentBoxType;

        switch (enemyType)
        {
            case BoxType.NormalFight:
                GenerateGoldReward(remainningTurn);
                GenerateCardReward(1);
                break;

            case BoxType.EliteFight:
                GenerateGoldReward(remainningTurn);
                GenerateCardReward(1);
                GenerateDNAReward(CardRarity.Rare);
                break;

            case BoxType.BossFight:
                GenerateGoldReward(remainningTurn);
                GenerateCardReward(1);
                GenerateCardReward(CardRarity.Legend);
                GenerateDNAReward(CardRarity.Legend);
                break;

            default:
                break;
        }
    }

    public void GenerateCardReward(int number)
    {
        rewardCanvas.SetActive(true);

        for (int j = 0; j < number; j++)
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < cardRewardOptionNumber; i++)
            {
                int newCardIndex = GetNextCardID();
                cards.Add(CardDataModel.Instance.GetCard(newCardIndex));
            }

            GameObject newReward = Instantiate(Reward, rewardPanel.transform);
            newReward.AddComponent<CardRewardBehavior>().SetUp(cards);
        }
    }

    public void GenerateCardReward(CardRarity cardRarity)
    {
        rewardCanvas.SetActive(true);
        List<Card> cards = new List<Card>();

        switch (cardRarity)
        {
            case CardRarity.Normal:
                // Normal
                for (int i = 0; i < 3; i++)
                {
                    Card card = normalCard[i];
                    cards.Add(card);
                    normalCard.RemoveAt(i);
                    normalCard.Add(card);
                }
                break;
            case CardRarity.Rare:
                for (int i = 0; i < 3; i++)
                {
                    Card card = rareCard[i];
                    cards.Add(card);
                    rareCard.RemoveAt(i);
                    rareCard.Add(card);
                }
                break;
            case CardRarity.Legend:
                for (int i = 0; i < 3; i++)
                {
                    Card card = legendCard[i];
                    cards.Add(card);
                    legendCard.RemoveAt(i);
                    legendCard.Add(card);
                }
                break;
            default:
                break;
        }

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<CardRewardBehavior>().SetUp(cards);
    }

    public void GenerateDNAReward(CardRarity cardRarity)
    {
        rewardCanvas.SetActive(true);
        DNA dna;
        switch (cardRarity)
        {
            case CardRarity.Normal:
                // Normal
                if (normalDNA.Count == 0)
                {
                    dna = null;
                }

                dna = normalDNA[0];
                normalDNA.Remove(normalDNA[0]);
                break;

            case CardRarity.Rare:
                // Rare
                if (rareDNA.Count == 0)
                {
                    dna = null;
                }

                dna = rareDNA[0];
                rareDNA.Remove(rareDNA[0]);
                break;

            case CardRarity.Legend:
                // Legend
                if (legendDNA.Count == 0)
                {
                    dna = null;
                }

                dna = legendDNA[0];
                legendDNA.Remove(legendDNA[0]);
                break;
            default:
                dna = null;
                break;
        }

        if (dna != null)
        {
            GameObject newReward = Instantiate(Reward, rewardPanel.transform);
            newReward.AddComponent<DNARewardBehavior>().SetUp(dna);
        }
    }

    public void GenerateGoldReward(int remainningTurn)
    {
        rewardCanvas.SetActive(true);
        if (remainningTurn == 0)
        {
            return;
        }

        int gold = remainningTurn * 50;

        GameObject newReward = Instantiate(Reward, rewardPanel.transform);
        newReward.AddComponent<GoldRewardBehavior>().SetUp(gold);
    }

    // 种子随机数生成
    // Method to get a single card ID based on probabilities
    public int GetNextCardID()
    {
        float randomValue = (float)GameSetting.cardRewardRand.NextDouble();

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

        int index = GameSetting.cardRewardRand.Next(cardList.Count);
        return cardList[index].id;
    }
}