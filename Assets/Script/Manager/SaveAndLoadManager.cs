using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Card;

public class SaveAndLoadManager : MonoBehaviour
{
    private string playerDataLocation = "playerData.json";

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
        //string path = Application.persistentDataPath + playerDataLocation;

        //// Check if the file exists
        //if (File.Exists(path))
        //{
        //    Debug.Log("Continue Game");
        //    LoadData();
        //}
        //else
        //{
        //    Debug.Log("New Game");
        //    LoadNewGame();
        //}

        string directoryPath = Path.Combine(Application.persistentDataPath, "InGameData");
        string filePath = Path.Combine(directoryPath, playerDataLocation);

        // Check if the file exists before trying to read it
        if (File.Exists(filePath))
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
        playerData.Seed = MainMenuBehavior.seed;

        CardDataModel.Instance.LoadDefaultCard();
        playerData.PlayerDNA = CardDataModel.Instance.GetPlayerDNA();

        LoadData(playerData);
    }

    public void LoadData(PlayerData playerData = null)
    {
        if (playerData == null)
        {
            //string path = Application.persistentDataPath + playerDataLocation;
            //string json = File.ReadAllText(path);
            //playerData = JsonUtility.FromJson<PlayerData>(json);
            playerData = LoadData<PlayerData>(playerDataLocation);
        }
        // 给主菜单static设置seed，从而让设置页面可以访问
        MainMenuBehavior.seed = playerData.Seed;

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
        playerData.Seed = MainMenuBehavior.seed;
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

        //string json = JsonUtility.ToJson(playerData);
        //string path = Application.persistentDataPath + playerDataLocation;
        //File.WriteAllText(path, json);

        SaveData<PlayerData>(playerData, playerDataLocation);
    }

    public T LoadData<T>(string fileName)
    {
        // Combine the path for the file
        string directoryPath = Path.Combine(Application.persistentDataPath, "InGameData");
        string filePath = Path.Combine(directoryPath, fileName);

        // Check if the file exists before trying to read it
        if (File.Exists(filePath))
        {
            // Read the JSON data from the file
            string jsonData = File.ReadAllText(filePath);

            // Deserialize the JSON back to the object
            T dataObject = JsonUtility.FromJson<T>(jsonData);

            Debug.Log("Data loaded from: " + filePath);
            return dataObject;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            return default(T);  // Return default if file doesn't exist
        }
    }

    public void SaveData<T>(T dataObject, string fileName)
    {
        // Convert the object to JSON
        string jsonData = JsonUtility.ToJson(dataObject, true);

        // Combine the path for the file
        string directoryPath = Path.Combine(Application.persistentDataPath, "InGameData");
        string filePath = Path.Combine(directoryPath, fileName);

        // Write the JSON data to the file
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Data saved to: " + filePath);
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