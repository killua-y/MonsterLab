using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Card;

public class SaveAndLoadManager : MonoBehaviour
{
    private string playerDataLocation = "/Datas/InGameData/playerData.json";

    // 引用的script
    private GameSetting gameSetting;
    private MapLayout mapLayout;

    private void Awake()
    {
        gameSetting = FindAnyObjectByType<GameSetting>();
        mapLayout = FindAnyObjectByType<MapLayout>();
    }

    private void Start()
    {
        string path = Application.dataPath + playerDataLocation;

        // Check if the file exists
        if (File.Exists(path))
        {
            Debug.Log("Continue Game");
            LoadData();
        }
        else
        {
            Debug.Log("New Game");
            LoadNewGame();
        }
    }

    public void LoadNewGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.Seed = 2;

        CardDataModel.Instance.LoadDefaultCard();
        playerData.PlayerDNA = CardDataModel.Instance.GetPlayerDNA();

        LoadData(playerData);
    }

    public void LoadData(PlayerData playerData = null)
    {
        if (playerData == null)
        {
            string path = Application.dataPath + playerDataLocation;
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }

        // 该顺序无法变化
        // 不需要其他script的loadData
        gameSetting.LoadData(playerData);
        PlayerStatesManager.Instance.LoadData(playerData);
        CardDataModel.Instance.LoadData(playerData);

        // 从这里开始是需要其他script的loadData
        mapLayout.LoadData(playerData);
        RewardManager.Instance.LoadData();

        // acts永远最后call
        ActsManager.Instance.LoadData(playerData);
    }

    public void SaveData()
    {
        PlayerData playerData = new PlayerData();

        // 从各处script调用playerData里的数据
        playerData.Seed = 2;
        playerData.randomState = gameSetting.SaveData();
        playerData.playerStates = PlayerStatesManager.Instance.SaveData();
        playerData.currentLayerBox = mapLayout.SaveData();
        playerData.nextAct = ActsManager.Instance.SaveData();

        foreach (Card card in CardDataModel.Instance.GetMainDeck())
        {
            if (card is MonsterCard)
            {
                playerData.MainDeckMonster.Add((MonsterCard)card);
            }
            else if (card is SpellCard)
            {
                playerData.MainDeckSpell.Add((SpellCard)card);
            }
            else if (card is ItemCard)
            {
                playerData.MainDeckItem.Add((ItemCard)card);
            }
            else
            {
                Debug.Log("Undefined card type deteced");
            }
        }

        foreach (Card card in CardDataModel.Instance.GetExtraDeck())
        {
            if (card is MonsterCard)
            {
                playerData.ExtraDeckMonster.Add((MonsterCard)card);
            }
            else if (card is SpellCard)
            {
                playerData.ExtraDeckSpell.Add((SpellCard)card);
            }
            else if (card is ItemCard)
            {
                playerData.ExtraDeckItem.Add((ItemCard)card);
            }
            else
            {
                Debug.Log("Undefined card type deteced");
            }
        }

        playerData.PlayerDNA = CardDataModel.Instance.GetPlayerDNA();

        string json = JsonUtility.ToJson(playerData);
        string path = Application.dataPath + playerDataLocation;
        File.WriteAllText(path, json);
    }
}

[System.Serializable]
public class PlayerData
{
    public int Seed;
    public RandomState randomState;

    public PlayerStates playerStates;

    public List<TowerBox> currentLayerBox;

    public List<MonsterCard> MainDeckMonster = new List<MonsterCard>();
    public List<SpellCard> MainDeckSpell = new List<SpellCard>();
    public List<ItemCard> MainDeckItem = new List<ItemCard>();

    public List<MonsterCard> ExtraDeckMonster = new List<MonsterCard>();
    public List<SpellCard> ExtraDeckSpell = new List<SpellCard>();
    public List<ItemCard> ExtraDeckItem = new List<ItemCard>();

    public List<DNA> PlayerDNA;

    public NextAct nextAct;
}

[System.Serializable]
public class PlayerStates
{
    public int row;
    public int column;
    public int PlayerHealth;
    public int Gold;
    public int MaxCost;
    public int MaxUnit;
    public int ExtraDeckCapacity;
}

[System.Serializable]
public class NextAct
{
    public int layer;
    public int step;
    public bool startCurrentAct;
    public int generateCombatReward;
    public List<Enemy> EnemiesEncountered;
    public List<QuestionMarkEvent> EventEncountered;
}