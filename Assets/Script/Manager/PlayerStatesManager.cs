using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public TextMeshProUGUI costText;

    public static int playerHealthPoint = 5;
    private int maxCost = 3;
    private int currentCost;

    // Start is called before the first frame update
    void Start()
    {
        currentCost = maxCost;
        InGameStateManager.Instance.OnPreparePhaseStart += OnTurnStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnTurnEnd;
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

    public void OnTurnStart()
    {
        currentCost = maxCost;
        UpdateCostText();
    }

    public void OnTurnEnd()
    {

    }
}
