using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatesManager : Manager<PlayerStatesManager>
{
    public TextMeshProUGUI costText;

    private int maxCost = 3;
    private int currentCost;

    //new void Awake()
    //{
    //    base.Awake();
    //}

    // Start is called before the first frame update
    void Start()
    {
        currentCost = maxCost;
        InGameStateManager.Instance.OnTurnStart += OnTurnStart;
        InGameStateManager.Instance.OnTurnEnd += OnTurnEnd;
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
