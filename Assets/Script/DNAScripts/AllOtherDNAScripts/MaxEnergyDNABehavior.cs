using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxEnergyDNABehavior : DNABehavior
{
    public override void OnAcquire()
    {
        PlayerStatesManager.maxCost += DNAModel.effectData;
    }
}
