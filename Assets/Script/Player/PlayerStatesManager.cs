using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public TextMeshProUGUI playerHealthText;
    public Transform DNAParent;
    public GameObject DNAPrefab;

    public static int maxCost = 3;
    public static int playerHealthPoint = 3;
    public static int maxUnit = 5;

    private List<DNA> playerDNAList = new List<DNA>();

    // Start is called before the first frame update
    void Start()
    {
        playerDNAList = CardDataModel.Instance.GetPlayerDNA();

        // 根据玩家已经有的dna来重新生成
        foreach (DNA dna in playerDNAList)
        {
            GameObject newDNA = Instantiate(DNAPrefab, DNAParent);
            newDNA.AddComponent(Type.GetType(dna.scriptLocation));
            newDNA.GetComponent<DNABehavior>().SetUp(dna);
        }
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
}
