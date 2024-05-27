using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPointDNABehavior : DNABehavior
{
    private void Start()
    {
        BattleManager.Instance.OnUnitSummon += OnSummon;
    }

    private void OnSummon(BaseEntity baseEntity)
    {
        if (baseEntity.myTeam == Team.Player)
        {
            baseEntity.cardModel.healthPoint += DNAModel.effectData;
            baseEntity.UpdateMonster();
        }
    }
}
