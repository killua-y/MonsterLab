using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Card;

public class CardDataModel : MonoBehaviour
{
    public static CardDataModel Instance;

    public TextAsset warriorCard; // 战士的默认卡组

    private string textCardDataPath = "/Datas/cardsdata - AllCards.csv"; // 卡牌数据txt文件
    private string textDNADataPath = "/Datas/cardsdata - DNA.csv"; // DNA数据text文件
    private string textPlayerDataPath = "/Datas/playerdata.csv"; // 玩家的卡牌数据存储文件

    private List<Card> cardList = new List<Card>(); // 存储卡牌数据的链表
    private List<DNA> DNAList = new List<DNA>(); // 存储DNA数据的链表

    //储存玩家卡牌数据的list
    private List<Card> playerExtraDeckData = new List<Card>();
    private List<Card> playerCardData = new List<Card>();
    private int[] playerDNAData; // 储存玩家DNA数据的array
    public int totalCoins;

    // 怪兽链表
    public TextAsset enemyTextCardData; // 地方怪兽卡牌数据txt文件
    public List<Enemy> enemyList = new List<Enemy>(); // 所有敌人的数据
    private List<MonsterCard> enemyCardList = new List<MonsterCard>(); // 存储地方怪兽卡牌数据的链表

    // keyword
    private string keyWordsDataPath = "/Datas/cardsdata - Keyword.csv";
    public List<string> keyWords = new List<string>();
    public List<string> keyWordsDefinition = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        LoadKeyWordList();
        LoadCardList();
        LoadEnemyCardList();
        LoadDNAList();
    }

    public void LoadNewGame()
    {
        LoadPlayerData();
        File.WriteAllLines(Application.dataPath + textPlayerDataPath, warriorCard.text.Split('\n'));
        Debug.Log("reset player deck");
    }

    private void LoadKeyWordList()
    {
        string path = Application.dataPath + keyWordsDataPath;
        string[] keyWordArray = File.ReadAllLines(path);

        foreach (var row in keyWordArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "")
            {
                continue;
            }
            else
            {
                keyWords.Add(rowArray[1]);
                keyWordsDefinition.Add(rowArray[2]);
            }
        }
    }

    // 加载所有卡牌数据
    private void LoadCardList()
    {
        //Load卡片
        string path = Application.dataPath + textCardDataPath;
        string[] dataArray = File.ReadAllLines(path);

        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "")
            {
                continue;
            }
            else
            {
                Card newCard = HelperFunction.LoadCard(rowArray, keyWords);
                cardList.Add(newCard);
            }
        }
    }

    public void LoadDNAList()
    {
        // 加载DNA数据
        string path = Application.dataPath + textDNADataPath;
        string[] dataArray = File.ReadAllLines(path);

        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "")
            {
                continue;
            }
            else if (rowArray[0] == "DNA")
            {
                DNA newdna = HelperFunction.LoadDNA(rowArray, keyWords);
                if (newdna != null)
                {
                    DNAList.Add(newdna);
                }
            }
            else
            {
                Debug.Log("DNA cvs data error, the first string is : " + rowArray[0]);
            }
        }
    }


    private void LoadEnemyCardList()
    {
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
            else if (rowArray[0] == "")
            {
                continue;
            }
            else if (rowArray[0] == "m")
            {
                MonsterCard newCard = (MonsterCard)HelperFunction.LoadCard(rowArray, keyWords);
                if (newCard != null)
                {
                    enemyCardList.Add(newCard);
                }

                //Debug.Log("Load enemy monster card: " + name);
            }
            else if (rowArray[0] == "e")
            {
                string name = rowArray[2];
                int layer = int.Parse(rowArray[3]);
                EnemyType enemyType = HelperFunction.ConvertToEnum<EnemyType>(rowArray[4]);
                string scriptLocation = rowArray[5];
                enemyList.Add(new Enemy(name, layer, enemyType, scriptLocation));
            }
            else
            {
                Debug.Log("enemy cvs data error, the first string is : " + rowArray[0]);
            }
        }
    }

    // 获得卡牌
    public void ObtainCard(Card _card)
    {
        // 玩家数据中增加该卡牌
        playerCardData.Add(Card.CloneCard(_card));
    }

    // 删除卡牌
    public void DeleteCard(Card _card)
    {
        // 查看剩余卡牌是否大于0
        if (playerCardData.Contains(_card))
        {
            playerCardData.Remove(_card);
        }
        else if (playerExtraDeckData.Contains(_card))
        {
            playerExtraDeckData.Remove(_card);
        }
        else
        {
            Debug.Log("Trying to delete card that do not have");
        }
    }

    public void ObtainDNA(int _id)
    {
        if (playerDNAData[_id] >= 1)
        {
            Debug.Log("Error: acquire DNA that already have");
        }

        // 玩家数据中增加该DNA
        playerDNAData[_id] += 1;
    }

    // 加载玩家卡组数据
    public void LoadPlayerData()
    {
        string path = Application.dataPath + textPlayerDataPath;
        string[] dataArray = File.ReadAllLines(path);

        playerDNAData = new int[DNAList.Count];

        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "")
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
                for (int i = 0; i < num; i++)
                {
                    playerCardData.Add(Card.CloneCard(cardList[id]));
                }
            }
            else if (rowArray[0] == "DNA")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerDNAData[id] = num;
            }
            else if (rowArray[0] == "extraDeck")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                for (int i = 0; i < num; i++)
                {
                    playerExtraDeckData.Add(Card.CloneCard(cardList[id]));
                }
                //Debug.Log("Load extra deck card with id : " + id + " num: " + num);
            }
            else
            {
                Debug.Log("Player cvs data error, the first string is : " + rowArray[0]);
            }
        }
    }

    // 保存玩家钱/卡牌/DNA数据
    public void SavePlayerData()
    {
        List<string> datas = new List<string>();
        string path = Application.dataPath + textPlayerDataPath;
        datas.Add("coins," + totalCoins.ToString());

        for (int i = 0; i < playerDNAData.Length; i++)
        {
            if (playerDNAData[i] != 0)
            {
                datas.Add("NDA," + i.ToString() + "," + playerDNAData[i].ToString());
            }
        }

        File.WriteAllLines(path, datas);
    }

    // 加载局内卡组
    public List<Card> GetMainDeck()
    {
        return playerCardData;
    }

    // 加载局内额外卡组
    public List<Card> GetExtraDeck()
    {
        return playerExtraDeckData;
    }

    // 加载局内额外卡组
    public List<Card> GetPlayerDeck()
    {
        List<Card> cardList = new List<Card>();
        cardList.AddRange(playerCardData);
        cardList.AddRange(playerExtraDeckData);
        return cardList;
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

    // 输出所有卡牌信息
    public List<DNA> GetAllDNA()
    {
        return DNAList;
    }

    // 输出需要的卡牌信息
    public Card GetCard(int index)
    {
        return cardList[index];
    }

    // 输出需要的卡牌信息
    public Card GetEnemyCard(int index)
    {
        return enemyCardList[index];
    }

    public void ChangeDeckFromMainToExtra(Card card, bool fromMainToExtra)
    {
        if (fromMainToExtra)
        {
            if (playerCardData.Contains(card))
            {
                playerCardData.Remove(card);
                playerExtraDeckData.Add(card);
            }
            else
            {
                Debug.Log("Bad bug");
            }
        }
        else
        {
            if (playerExtraDeckData.Contains(card))
            {
                playerExtraDeckData.Remove(card);
                playerCardData.Add(card);
            }
            else
            {
                Debug.Log("Bad bug");
            }
        }
    }

    public void LoadData()
    {
        string jsonString = JsonUtility.ToJson(playerExtraDeckData);
        playerExtraDeckData = JsonUtility.FromJson<List<Card>>(jsonString);
    }

    public void SaveData()
    {
        // Serialize the list to JSON
        string jsonString = JsonUtility.ToJson(playerExtraDeckData);

        // Write the JSON string to a file
        File.WriteAllText("playerCards.json", jsonString);
    }
}
