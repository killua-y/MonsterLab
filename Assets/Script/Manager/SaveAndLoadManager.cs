using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance;
    private string playerDataLocation = "/Datas/InGameData/playerData.json";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        string path = Application.dataPath + playerDataLocation;

        // Check if the file exists
        if (File.Exists(path))
        {
            LoadData();
        }
        else
        {
            LoadNewGame();
        }
    }

    public void LoadNewGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.Seed = 2;
        playerData.layer = 1;
        playerData.PlayerHealth = 3;
        playerData.Gold = 100;
        playerData.MaxCost = 10;
        playerData.MaxUnit = 5;
        playerData.startCurrentAct = false;

        LoadData();
    }

    public void LoadData(PlayerData playerData = null)
    {
        if (playerData == null)
        {
            string path = Application.dataPath + playerDataLocation;
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }

        CardDataModel.Instance.LoadPlayerData();
    }

    public void SaveData()
    {
        PlayerData playerData = new PlayerData();

        // 从各处script调用playerData里的数据
        playerData.Seed = GameSetting.instance.seed;
        playerData.PlayerHealth = PlayerStatesManager.playerHealthPoint;
        playerData.PlayerMainDeck = CardDataModel.Instance.GetMainDeck();
        playerData.PlayerExtraDeck = CardDataModel.Instance.GetExtraDeck();
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
    public int layer;
    public int PlayerHealth;
    public int Gold;
    public int MaxCost;
    public int MaxUnit;
    public bool startCurrentAct;
    public List<Card> PlayerMainDeck;
    public List<Card> PlayerExtraDeck;
    public List<DNA> PlayerDNA;
    public List<Enemy> EnemiesEncountered;
    public List<string> EventEncountered;
}