using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        List<BaseEntity> enemyList = BattleManager.Instance.GetEntitiesAgainst(myTeam);
        foreach (BaseEntity enemy in enemyList)
        {
            enemy.TakeDamage(cardModel.effectData, null);
        }
    }
}
