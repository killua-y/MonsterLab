using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Singleton<PlayerStatesManager>
{
    public static int Gold;
    public static int maxCost;
    public static int playerHealthPoint;
    public static int maxUnit;

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerGoldText;
    public Transform DNAParent;
    public GameObject DNAPrefab;

    // 引用的script
    private PlayerBehavior player;

    protected override void Awake()
    {
        base.Awake();
        player = FindAnyObjectByType<PlayerBehavior>();
    }

    public void LoadData(PlayerData playerData)
    {
        if (playerData.playerStates == null)
        {
            Gold = 100;
            maxCost = 10;
            playerHealthPoint = 3;
            maxUnit = 5;
            player.row = 3;
            player.column = 0;
        }
        else
        {
            Gold = playerData.playerStates.Gold;
            maxCost = playerData.playerStates.MaxCost;
            playerHealthPoint = playerData.playerStates.PlayerHealth;
            maxUnit = playerData.playerStates.MaxUnit;
            player.row = playerData.playerStates.row;
            player.column = playerData.playerStates.column;
        }
        
        List<DNA> playerDNAData = playerData.PlayerDNA;

        // 根据玩家已经有的dna来重新生成
        foreach (DNA dna in playerDNAData)
        {
            GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
            newDNA.AddComponent(Type.GetType(dna.scriptLocation));
            newDNA.GetComponent<DNABehavior>().SetUp(dna);
        }

        playerHealthText.text = "Player Health: " + playerHealthPoint;
        playerGoldText.text = "Gold: " + Gold;
    }

    public PlayerStates SaveData()
    {
        return new PlayerStates
        {
            row = player.row,
            column = player.column,
            PlayerHealth = playerHealthPoint,
            Gold = Gold,
            MaxCost = maxCost,
            MaxUnit = maxUnit,
        };
}


    // 新获取dna
    public void AcquireDNA(DNA DNAModel)
    {
        CardDataModel.Instance.ObtainDNA(DNAModel.id);
        GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
        newDNA.AddComponent(Type.GetType(DNAModel.scriptLocation));
        newDNA.GetComponent<DNABehavior>().SetUp(DNAModel);
        newDNA.GetComponent<DNABehavior>().OnAcquire();

    }

    public void DecreaseHealth(int number)
    {
        playerHealthPoint -= number;
        playerHealthText.text = "Player Health: " + playerHealthPoint;
    }

    public void IncreaseHealth(int number)
    {
        playerHealthPoint += number;
        playerHealthText.text = "Player Health: " + playerHealthPoint;
    }

    public void IncreaseGold(int number)
    {
        Gold += number;
        playerGoldText.text = "Gold: " + Gold;
    }

    public void DecreaseGold(int number)
    {
        Gold -= number;
        playerGoldText.text = "Gold: " + Gold;
    }
}
