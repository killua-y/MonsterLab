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
    public static int extraDeckCapacity;

    public Transform heartParent;
    public GameObject heartIcon;
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
            player.row = 3;
            player.column = 0;
            Gold = 100;
            maxCost = 3;
            playerHealthPoint = 3;
            maxUnit = 5;
            extraDeckCapacity = 3;
        }
        else
        {
            player.row = playerData.playerStates.row;
            player.column = playerData.playerStates.column;
            Gold = playerData.playerStates.Gold;
            maxCost = playerData.playerStates.MaxCost;
            playerHealthPoint = playerData.playerStates.PlayerHealth;
            maxUnit = playerData.playerStates.MaxUnit;
            extraDeckCapacity = playerData.playerStates.ExtraDeckCapacity;
        }
        
        List<DNA> playerDNAData = playerData.PlayerDNA;

        // 根据玩家已经有的dna来重新生成
        foreach (DNA dna in playerDNAData)
        {
            GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
            newDNA.AddComponent(Type.GetType(dna.scriptLocation));
            newDNA.GetComponent<DNABehavior>().SetUp(dna);
        }

        UpdateHealthUI(playerHealthPoint);
        playerGoldText.text = Gold.ToString() ;
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
            ExtraDeckCapacity = extraDeckCapacity,
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
        UpdateHealthUI(playerHealthPoint);
    }

    public void IncreaseHealth(int number)
    {
        playerHealthPoint += number;
        UpdateHealthUI(playerHealthPoint);
    }

    void UpdateHealthUI(int currentHealth)
    {
        foreach(Transform child in heartParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < currentHealth; i++)
        {
            Instantiate(heartIcon, heartParent);
        }
    }


    public void IncreaseGold(int number)
    {
        Gold += number;
        playerGoldText.text = Gold.ToString();
    }

    public void DecreaseGold(int number)
    {
        Gold -= number;
        playerGoldText.text = Gold.ToString();
    }
}
