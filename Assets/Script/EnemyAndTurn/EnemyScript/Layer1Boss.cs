using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MaxxSlime : EnemyBehavior
{
    protected new int MaxTurn = 5;

    // 该敌人拥有的怪兽
    private MonsterCard MaxSlime;
    private MonsterCard AcidSlime;

    public override void LoadEnemy()
    {
        MaxSlime = TurnManager.Instance.monsterList[0];
        AcidSlime = TurnManager.Instance.monsterList[1];

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 5, MaxSlime);
            SummonEnenmy(2, 5, MaxSlime);
            SummonEnenmy(3, 5, MaxSlime);
            SummonEnenmy(1, 7, AcidSlime);
            SummonEnenmy(3, 7, AcidSlime);

            // 最后一波
            TurnManager.Instance.isFinalWaive = true;
        }

        base.SummonEnemy();
    }
}