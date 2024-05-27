using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSizeDNABehavior : DNABehavior
{
    public override void OnAcquire()
    {
        PlayerStatesManager.maxUnit += DNAModel.effectData;
    }
}
