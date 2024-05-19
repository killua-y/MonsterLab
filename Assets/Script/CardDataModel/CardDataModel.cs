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

    // 怪兽链表
    public TextAsset enemyTextCardData; // 地方怪兽卡牌数据txt文件
    private List<MonsterCard> enemyCardList = new List<MonsterCard>(); // 存储地方怪兽卡牌数据的链表

    // Start is called before the first frame update
    void Awake()
    {
        LordCardList();
        LordEnemyCardList();
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
                int id = currentIndex;
                currentIndex += 1;
                int uniqueID = 0;
                string cardName = rowArray[1];
                CardColor color = EnumConverter.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = EnumConverter.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string scriptLocation = rowArray[8];
                string imageLocation = rowArray[9];

                MonsterType type = EnumConverter.ConvertToEnum<MonsterType>(rowArray[10]);
                int attackPower = int.Parse(rowArray[11]);
                int healthPoint = int.Parse(rowArray[12]);
                float attackRange = float.Parse(rowArray[13]);
                int mana = int.Parse(rowArray[14]);
                string modelLocation = rowArray[15];

                cardList.Add(new MonsterCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, scriptLocation, imageLocation,
                    type, attackPower, healthPoint, attackRange, mana, modelLocation));

                //Debug.Log("Load monster card: " + name);
            }
            else if (rowArray[0] == "s")
            {
                int id = currentIndex;
                currentIndex += 1;
                int uniqueID = 0;
                string cardName = rowArray[1];
                CardColor color = EnumConverter.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = EnumConverter.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string scriptLocation = rowArray[8];
                string imageLocation = rowArray[9];

                cardList.Add(new SpellCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, scriptLocation, imageLocation));

                //Debug.Log("Load magic card: " + name);
            }
            else if (rowArray[0] == "i")
            {
                int id = currentIndex;
                currentIndex += 1;
                int uniqueID = 0;
                string cardName = rowArray[1];
                CardColor color = EnumConverter.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = EnumConverter.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string scriptLocation = rowArray[8];
                string imageLocation = rowArray[9];

                cardList.Add(new ItemCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, scriptLocation, imageLocation));

                //Debug.Log("Load item card: " + name);
            }
        }
    }

    public void LordEnemyCardList()
    {
        int currentIndex = 0;

        // 加载敌方怪兽数据
        // Load 怪兽卡
        string[] enemyMonsterDataArray = enemyTextCardData.text.Split('\n');
        foreach (var row in enemyMonsterDataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "m")
            {
                int id = currentIndex;
                currentIndex += 1;
                int uniqueID = 0;
                string cardName = rowArray[1];
                CardColor color = EnumConverter.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = EnumConverter.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = EnumConverter.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string cardLocation = rowArray[8];
                string imageLocation = rowArray[9];

                MonsterType type = EnumConverter.ConvertToEnum<MonsterType>(rowArray[10]);
                int attackPower = int.Parse(rowArray[11]);
                int healthPoint = int.Parse(rowArray[12]);
                float attackRange = float.Parse(rowArray[13]);
                int mana = int.Parse(rowArray[14]);
                string modelLocation = rowArray[15];

                enemyCardList.Add(new MonsterCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, cardLocation, imageLocation,
                    type, attackPower, healthPoint, attackRange, mana, modelLocation));

                //Debug.Log("Load enemy monster card: " + name);
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
                        Card newCard = Card.CloneCard(cardList[cardIndex]);  // Assuming constructor cloning or similar method
                        newCard.uniqueID = currentAssignedID;
                        currentAssignedID += 1;
                        deckCardList.Add(newCard);
                    }
                }
            }
        }

        return deckCardList;
    }

    // Helper，其他function会call来获取卡组数据
    public List<MonsterCard> GetEnemyMonster()
    {
        // 因为monster在生成时会自动创建新的clone，所以这里不需要输出clone
        return enemyCardList;
    }

    // 输出所有卡牌信息
    public List<Card> GetAllCard()
    {
        //List<Card> result = new List<Card>();
        //foreach (Card card in cardList)
        //{
        //    result.Add(Card.CloneCard(card));
        //}
        //return result;
        return cardList;
    }
}
