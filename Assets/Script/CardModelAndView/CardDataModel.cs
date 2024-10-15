using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static Card;

public class CardDataModel : Singleton<CardDataModel>
{
    private string textCardDataPath = "/Datas/cardsdata - AllCards.csv"; // 卡牌数据txt文件
    private string textDNADataPath = "/Datas/cardsdata - DNA.csv"; // DNA数据text文件
    private string textPlayerDataPath = "/Datas/playerdata.csv"; // 玩家的卡牌数据存储文件

    private List<Card> cardList = new List<Card>(); // 存储卡牌数据的链表
    private List<DNA> DNAList = new List<DNA>(); // 存储DNA数据的链表

    //储存玩家卡牌数据的list
    private List<Card> playerExtraDeckData = new List<Card>();
    private List<Card> playerCardData = new List<Card>();
    private List<DNA> playerDNAData = new List<DNA>(); // 储存玩家DNA数据的array

    // 怪兽链表
    public TextAsset enemyTextCardData; // 敌方怪兽卡牌数据txt文件
    private List<MonsterCard> enemyCardList = new List<MonsterCard>(); // 存储敌方怪兽卡牌数据的链表

    // keyword
    private string keyWordsDataPath = "/Datas/cardsdata - Keyword.csv";
    public List<string> keyWords = new List<string>();
    public List<string> keyWordsDefinition = new List<string>();

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        LoadKeyWordList();
        LoadCardList(textCardDataPath, cardList);
        LoadEnemyCardList();
        LoadDNAList();
    }

    public void LoadData(PlayerData playerData)
    {
        if (playerData.MainDeckMonster != null)
        {
            playerCardData.AddRange(playerData.MainDeckMonster);
            playerCardData.AddRange(playerData.MainDeckSpell);
            playerCardData.AddRange(playerData.MainDeckItem);
            playerExtraDeckData.AddRange(playerData.ExtraDeckMonster);
            playerExtraDeckData.AddRange(playerData.ExtraDeckSpell);
            playerExtraDeckData.AddRange(playerData.ExtraDeckItem);
            playerDNAData = playerData.PlayerDNA;
        }
        else
        {
            Debug.Log("New game, reset player deck");
        }
    }

    private void LoadKeyWordList()
    {
        string path = Application.persistentDataPath + keyWordsDataPath;
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
    public void LoadCardList(string dataPath, List<Card> cardList)
    {
        //Load卡片
        string path = Application.persistentDataPath + dataPath;
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
        string path = Application.persistentDataPath + textDNADataPath;
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
        if (playerDNAData.Contains(DNAList[_id]))
        {
            Debug.Log("Error: acquire DNA that already have");
        }

        // 玩家数据中增加该DNA
        playerDNAData.Add(DNAList[_id]);
    }

    // 加载玩家卡组数据
    public void LoadDefaultCard()
    {
        string path = Application.persistentDataPath + textPlayerDataPath;
        string[] dataArray = File.ReadAllLines(path);

        foreach (var row in dataArray)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "")
            {
                continue;
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
            else if (rowArray[0] == "DNA")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);

                if (num >= 1)
                {
                    playerDNAData.Add(DNAList[id]);
                }
            }
            else
            {
                Debug.Log("Player cvs data error, the first string is : " + rowArray[0]);
            }
        }
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
        return playerDNAData;
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
    public MonsterCard GetEnemyCard(int index)
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
}
