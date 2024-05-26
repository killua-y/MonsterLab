using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Card;

public class CardDataModel : MonoBehaviour
{
    public static CardDataModel Instance;
    public TextAsset textCardData; // 卡牌数据txt文件
    public TextAsset textDNAData; // DNA数据text文件
    // 玩家的卡牌数据存储文件
    private string textPlayerDataPath = "/Datas/playerdata.csv";
    public TextAsset warriorCard; // 战士的默认卡组

    public static bool NewGame = true;

    private List<Card> cardList = new List<Card>(); // 存储卡牌数据的链表
    private List<DNA> DNAList = new List<DNA>(); // 存储DNA数据的链表
    private int[] playerExtraDeckData; // 储存玩家额外卡组数据的array
    private int[] playerCardData; // 储存玩家卡牌数据的array
    private int[] playerDNAData; // 储存玩家DNA数据的array
    private int totalCoins;

    // 怪兽链表
    public TextAsset enemyTextCardData; // 地方怪兽卡牌数据txt文件
    private List<MonsterCard> enemyCardList = new List<MonsterCard>(); // 存储地方怪兽卡牌数据的链表

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        LoadCardList();
        LoadEnemyCardList();

        if (NewGame)
        {
            File.WriteAllLines(Application.dataPath + textPlayerDataPath, warriorCard.text.Split('\n'));
            Debug.Log("reset player deck");
            NewGame = false;
        }
        else
        {
            //Debug.Log("avoid reset deck");
        }

        // 需要在LoadCardList()之后call
        LoadPlayerData();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 加载所有卡牌数据
    public void LoadCardList()
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
                CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string scriptLocation = rowArray[8];
                string imageLocation = rowArray[9];

                MonsterType type = HelperFunction.ConvertToEnum<MonsterType>(rowArray[10]);
                int attackPower = int.Parse(rowArray[11]);
                int healthPoint = int.Parse(rowArray[12]);
                float attackRange = float.Parse(rowArray[13]);
                int mana = int.Parse(rowArray[14]);
                string modelLocation = rowArray[15];
                string skillScriptLocation = rowArray[16];

                cardList.Add(new MonsterCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, scriptLocation, imageLocation,
                    type, attackPower, healthPoint, attackRange, mana, modelLocation, skillScriptLocation));

                //Debug.Log("Load monster card: " + name);
            }
            else if (rowArray[0] == "s")
            {
                int id = currentIndex;
                currentIndex += 1;
                int uniqueID = 0;
                string cardName = rowArray[1];
                CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[5]);
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
                CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[5]);
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

    public void LoadEnemyCardList()
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
                CardColor color = HelperFunction.ConvertToEnum<CardColor>(rowArray[2]);
                CardRarity cardRarity = HelperFunction.ConvertToEnum<CardRarity>(rowArray[3]);
                int cost = int.Parse(rowArray[4]);
                CastType castType = HelperFunction.ConvertToEnum<CastType>(rowArray[5]);
                int effectData = int.Parse(rowArray[6]);
                string effectText = rowArray[7];
                string cardLocation = rowArray[8];
                string imageLocation = rowArray[9];

                MonsterType type = HelperFunction.ConvertToEnum<MonsterType>(rowArray[10]);
                int attackPower = int.Parse(rowArray[11]);
                int healthPoint = int.Parse(rowArray[12]);
                float attackRange = float.Parse(rowArray[13]);
                int mana = int.Parse(rowArray[14]);
                string modelLocation = rowArray[15];
                string skillScriptLocation = rowArray[16];

                enemyCardList.Add(new MonsterCard(id, uniqueID, cardName, color, cardRarity,
                    cost, castType, effectData, effectText, cardLocation, imageLocation,
                    type, attackPower, healthPoint, attackRange, mana, modelLocation, skillScriptLocation));

                //Debug.Log("Load enemy monster card: " + name);
            }
        }
    }

    // 获得卡牌
    public void ObtainCard(int _id)
    {
        // 玩家数据中增加该卡牌
        playerCardData[_id] += 1;

        SavePlayerData();
    }

    // 删除卡牌
    public void DeleteCard(int _id)
    {
        // 查看剩余卡牌是否大于0
        if(playerCardData[_id] >= 1)
        {
            playerCardData[_id] -= 1;
        }

        SavePlayerData();
    }

    // 加载玩家卡组数据
    public void LoadPlayerData()
    {
        string path = Application.dataPath + textPlayerDataPath;

        playerCardData = new int[cardList.Count];
        playerDNAData = new int[cardList.Count];
        playerExtraDeckData = new int[cardList.Count];

        string[] dataArray = File.ReadAllLines(path);

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
                //Debug.Log("Load card with id : " + id);
            }
            else if (rowArray[0] == "DNA")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerDNAData[id] = num;

                // 将玩家已经拥有的DNA从卡池中移除
                if (num != 0)
                {
                    DNAList.RemoveAll(dna => dna.id == id);
                }
            }
            else if (rowArray[0] == "extraDeck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerExtraDeckData[id] = num;
                //Debug.Log("Load extra deck card with id : " + id + " num: " + num);
            }
            else
            {
                Debug.Log("player cvs data error, the first string is : " + rowArray[0]);
            }
        }
        //Debug.Log("Player data loaded. Path: " + path);
        //Debug.Log("load data: " + string.Join("\n", dataArray));
    }

    // 保存玩家钱/卡牌/DNA数据
    public void SavePlayerData()
    {
        List<string> datas = new List<string>();
        string path = Application.dataPath + textPlayerDataPath;
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
                datas.Add("NDA," + i.ToString() + "," + playerDNAData[i].ToString());
            }
        }

        for (int i = 0; i < playerExtraDeckData.Length; i++)
        {
            if (playerExtraDeckData[i] != 0)
            {
                datas.Add("extraDeck," + i.ToString() + "," + playerExtraDeckData[i].ToString());
            }
        }

        File.WriteAllLines(path, datas);
        //Debug.Log("Player data saved. Path: " + path);
        //Debug.Log("Saved data: " + string.Join("\n", datas));
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

    // 加载局内额外卡组，赋予uniqueID
    public List<Card> InitializeExtraDeck(int currentAssignedID)
    {
        List<Card> extraDeckCardList = new List<Card>();

        for (int cardIndex = 0; cardIndex < playerExtraDeckData.Length; cardIndex++)
        {
            int quantity = playerExtraDeckData[cardIndex];

            if (quantity >= 1)
            {
                for (int i = 0; i < quantity; i++)
                {
                    if (cardIndex < cardList.Count)  // Ensure the index is within the range of available cards
                    {
                        Card newCard = Card.CloneCard(cardList[cardIndex]);  // Assuming constructor cloning or similar method
                        newCard.uniqueID = currentAssignedID;
                        currentAssignedID += 1;
                        extraDeckCardList.Add(newCard);
                        //Debug.Log("generate extra deck card : " + newCard.id);
                    }
                }
            }
        }

        return extraDeckCardList;
    }

    // 加载玩家DNA
    public List<DNA> GetPlayerDNA()
    {
        List<DNA> playerDNAList = new List<DNA>();

        for (int index = 0; index < playerDNAData.Length; index++)
        {
            int quantity = playerDNAData[index];

            if (quantity >= 1)
            {
                playerDNAList.Add(DNAList[index]);
            }
        }

        return playerDNAList;
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
        return cardList;
    }

    public void ChangeExtraDeck(Card card, bool toExtraDeck)
    {
        int cardIndex = card.id;

        // 如果bool为true说明是从卡组向extra deck添加卡片
        if (toExtraDeck)
        {
            playerCardData[cardIndex] -= 1;
            playerExtraDeckData[cardIndex] += 1;
        }
        // 反之为从extra deck向卡组添加卡片
        else
        {
            playerCardData[cardIndex] += 1;
            playerExtraDeckData[cardIndex] -= 1;
        }
    }

    public void ChangeDeckFromMainToExtra(int cardIndex, bool fromMainToExtra)
    {
        if (fromMainToExtra)
        {
            playerCardData[cardIndex] -= 1;
            playerExtraDeckData[cardIndex] += 1;
            if (playerCardData[cardIndex] < 0)
            {
                Debug.Log("Bad bug");
            }
        }
        else
        {
            playerExtraDeckData[cardIndex] -= 1;
            playerCardData[cardIndex] += 1;
            if (playerExtraDeckData[cardIndex] < 0)
            {
                Debug.Log("Bad bug");
            }
        }

        SavePlayerData();
    }
}
