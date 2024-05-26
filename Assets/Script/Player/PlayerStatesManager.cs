using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI costText;

    public static int playerHealthPoint = 3;
    private int maxCost = 3;
    private int currentCost;
    public static int maxUnit = 5;

    private List<DNA> playerDNAList = new List<DNA>();

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;

        playerDNAList = CardDataModel.Instance.GetPlayerDNA();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateCostText()
    {
        costText.text = currentCost + "/" + maxCost;
    }

    public int GetRemainingCost()
    {
        return currentCost;
    }

    public void IncreaseCost(int _cost)
    {
        currentCost += _cost;
        UpdateCostText();
    }

    public void DecreaseCost(int _cost)
    {
        currentCost -= _cost;
        UpdateCostText();
    }

    public void OnPreparePhaseStart()
    {
        currentCost = maxCost;
        UpdateCostText();
    }

    public void OnPreparePhaseEnd()
    {

    }
}
