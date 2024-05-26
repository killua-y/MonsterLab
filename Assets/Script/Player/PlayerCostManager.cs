using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCostManager : Manager<PlayerCostManager>
{
    public TextMeshProUGUI costText;

    private int currentCost;
    private int maxCost;
    // Start is called before the first frame update
    void Start()
    {
        maxCost = PlayerStatesManager.maxCost;

        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
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
}
