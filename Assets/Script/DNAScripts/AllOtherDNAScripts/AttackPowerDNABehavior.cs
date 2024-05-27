using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerDNABehavior : DNABehavior
{
    private void Start()
    {
        BattleManager.Instance.OnUnitSummon += OnSummon;
    }

    private void OnSummon(BaseEntity baseEntity)
    {
        baseEntity.cardModel.attackPower += DNAModel.effectData;
        baseEntity.UpdateMonster();
    }
}
