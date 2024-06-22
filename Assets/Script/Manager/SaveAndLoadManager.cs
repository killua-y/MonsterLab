using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Card;

public class SaveAndLoadManager : MonoBehaviour
{
    private string playerDataLocation = "/Datas/InGameData/playerData.json";

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

        CardDataModel.Instance.LoadData(playerData);
        PlayerStatesManager.Instance.LoadData(playerData);
    }

    public void SaveData()
    {
        PlayerData playerData = new PlayerData();

        // 从各处script调用playerData里的数据
        playerData.Seed = 2;
        playerData.layer = 1;
        playerData.PlayerHealth = PlayerStatesManager.playerHealthPoint;
        playerData.Gold = PlayerStatesManager.Gold;
        playerData.MaxCost = PlayerStatesManager.maxCost;
        playerData.MaxUnit = PlayerStatesManager.maxCost;
        playerData.startCurrentAct = false;

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
    public int layer;
    public int PlayerHealth;
    public int Gold;
    public int MaxCost;
    public int MaxUnit;
    public bool startCurrentAct;

    public List<MonsterCard> MainDeckMonster = new List<MonsterCard>();
    public List<SpellCard> MainDeckSpell = new List<SpellCard>();
    public List<ItemCard> MainDeckItem = new List<ItemCard>();

    public List<MonsterCard> ExtraDeckMonster = new List<MonsterCard>();
    public List<SpellCard> ExtraDeckSpell = new List<SpellCard>();
    public List<ItemCard> ExtraDeckItem = new List<ItemCard>();

    public List<DNA> PlayerDNA;
    public List<Enemy> EnemiesEncountered;
    public List<string> EventEncountered;
}