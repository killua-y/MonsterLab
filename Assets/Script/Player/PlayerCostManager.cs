using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCostManager : Singleton<PlayerCostManager>
{
    public TextMeshProUGUI costText;

    private int currentCost;
    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
    }

    private void UpdateCostText()
    {
        costText.text = currentCost + "/" + PlayerStatesManager.maxCost;
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
        currentCost = PlayerStatesManager.maxCost;
        UpdateCostText();
    }
}
