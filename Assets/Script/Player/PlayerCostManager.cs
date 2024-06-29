using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCostManager : MonoBehaviour
{
    public TextMeshProUGUI costText;
    public int currentCost;

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
    }

    private void UpdateCostText()
    {
        costText.text = currentCost + "/" + PlayerStatesManager.maxCost;
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
