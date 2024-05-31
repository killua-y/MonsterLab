using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public static int Gold = 0;
    public static int maxCost = 10;
    public static int playerHealthPoint = 3;
    public static int maxUnit = 5;

    private List<DNA> playerDNAList = new List<DNA>();

    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerGoldText;
    public Transform DNAParent;
    public GameObject DNAPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerDNAList = CardDataModel.Instance.GetPlayerDNA();
        Gold = CardDataModel.Instance.totalCoins;

        // 根据玩家已经有的dna来重新生成
        foreach (DNA dna in playerDNAList)
        {
            GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
            newDNA.AddComponent(Type.GetType(dna.scriptLocation));
            newDNA.GetComponent<DNABehavior>().SetUp(dna);
        }

        playerHealthText.text = "Player Health: " + playerHealthPoint;
        playerGoldText.text = "Gold: " + Gold;
    }

    // 新获取dna
    public void AcquireDNA(DNA DNAModel)
    {
        CardDataModel.Instance.ObtainDNA(DNAModel.id);
        GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
        newDNA.AddComponent(Type.GetType(DNAModel.scriptLocation));
        newDNA.GetComponent<DNABehavior>().SetUp(DNAModel);
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
