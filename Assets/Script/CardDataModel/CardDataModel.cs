using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Card;

public class CardDataModel : MonoBehaviour
{
    public TextAsset textCardData; // 卡牌数据txt文件
    public TextAsset textPlayerData; // 玩家卡牌数据txt文件

    private List<Card> cardList = new List<Card>(); // 存储卡牌数据的链表
    private int[] playerCardData; // 储存玩家卡牌数据的array
    private int[] playerDNAData; // 储存玩家DNA数据的array
    private int totalCoins;

    // Start is called before the first frame update
    void Awake()
    {
        LordCardList();
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 加载所有卡牌数据
    public void LordCardList()
    {
        int currentIndex = 0;

        //Load 怪兽卡
        string[] MonsterDataArray = textCardData.text.Split('\n');
        foreach (var row in MonsterDataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "m")
            {
                //卡名 稀有度 种族 星级 召唤条件
                //攻击力 生命值 攻击距离 数值 技能描述
                //卡片位置 模型位置
                int id = currentIndex;
                currentIndex += 1;

                string name = rowArray[1];
                CardRarity rarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[2]);
                MonsterType type = EnumConverter.ConvertToEnum<MonsterType>(rowArray[3]);
                int uniqueID = 0;
                int rank = int.Parse(rowArray[4]);
                int sacrifice = int.Parse(rowArray[5]);
                int attack = int.Parse(rowArray[6]);
                int healthPoint = int.Parse(rowArray[7]);
                int attackRange = int.Parse(rowArray[8]);
                int effectData = int.Parse(rowArray[9]);
                string effectText = rowArray[10];
                string cardLoaction = rowArray[11];
                string imageLoaction = rowArray[12];
                string modelLocation = rowArray[13];
                cardList.Add(new MonsterCard(id, name, rarity, uniqueID, type, rank, sacrifice, attack,
                    healthPoint, attackRange, effectData, effectText, cardLoaction, imageLoaction, modelLocation));

                //Debug.Log("Load monster card: " + name);
            }
            else if (rowArray[0] == "s")
            {
                //卡名 稀有度 费用 数值 技能描述 卡片位置
                int id = currentIndex;
                currentIndex += 1;
                string name = rowArray[1];
                CardRarity rarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[2]);
                int uniqueID = 0;

                int cost = int.Parse(rowArray[3]);
                int effectData = int.Parse(rowArray[4]);
                string effectText = rowArray[5];
                string cardLoaction = rowArray[6];
                string imageLoaction = rowArray[7];
                cardList.Add(new SpellCard(id, name, rarity, uniqueID, cost, effectData, effectText, cardLoaction, imageLoaction));

                //Debug.Log("Load magic card: " + name);
            }
            else if (rowArray[0] == "i")
            {
                //卡名 稀有度 费用 数值 技能描述 卡片位置
                int id = currentIndex;
                currentIndex += 1;
                string name = rowArray[1];
                CardRarity rarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[2]);
                int uniqueID = 0;

                int cost = int.Parse(rowArray[3]);
                int effectData = int.Parse(rowArray[4]);
                string effectText = rowArray[5];
                string cardLoaction = rowArray[6];
                string imageLoaction = rowArray[7];
                cardList.Add(new ItemCard(id, name, rarity, uniqueID, cost, effectData, effectText, cardLoaction, imageLoaction));

                //Debug.Log("Load item card: " + name);
            }
        }
    }

    // 获得卡牌
    public void ObtainCard(int _id)
    {
        // 玩家数据中增加该卡牌
        playerCardData[_id] += 1;
    }

    // 删除卡牌
    public void DeleteCard(int _id)
    {
        // 查看剩余卡牌是否大于0
        if(playerCardData[_id] >= 1)
        {
            playerCardData[_id] -= 1;
        }
    }

    // 加载局内卡组，赋予uniqueID
    public List<Card> InitializeDeck(int currentAssignedID)
    {
        List<Card> deckCardList = new List<Card>();

        for (int cardIndex = 0; cardIndex < playerCardData.Length; cardIndex++)
        {
            int quantity = playerCardData[cardIndex];

            if (quantity >= 1)
            {
                for (int i = 0; i < quantity; i++)
                {
                    if (cardIndex < cardList.Count)  // Ensure the index is within the range of available cards
                    {
                        Card newCard = cardList[cardIndex];  // Assuming constructor cloning or similar method
                        newCard.uniqueID = currentAssignedID;
                        currentAssignedID += 1;
                        deckCardList.Add(newCard);

                    }
                }
            }
        }

        return deckCardList;
    }

    // 加载玩家卡组数据
    public void LoadPlayerData()
    {
        playerCardData = new int[cardList.Count];
        playerDNAData = new int[cardList.Count];
        string[] dataArray = textPlayerData.text.Split('\n');
        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "coins")
            {
                totalCoins = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerCardData[id] = num;
                //Debug.Log("CardID: " + id + "Add");
            }
            else if (rowArray[0] == "DNA")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerDNAData[id] = num;
            }
        }
        //updateText();
    }

    // 保存玩家钱/卡牌/DNA数据
    public void SavePlayerData()
    {
        List<string> datas = new List<string>();
        string path = Application.dataPath + "/Datas/playerdata.csv";
        datas.Add("coins," + totalCoins.ToString());
        for (int i = 0; i < playerCardData.Length; i++)
        {
            if (playerCardData[i] != 0)
            {
                datas.Add("card," + i.ToString() + "," + playerCardData[i].ToString());
            }
        }
        for (int i = 0; i < playerDNAData.Length; i++)
        {
            if (playerDNAData[i] != 0)
            {
                datas.Add("NDA," + i.ToString() + "," + playerCardData[i].ToString());
            }
        }

        File.WriteAllLines(path, datas);
    }
}
