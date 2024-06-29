using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BleedWolfEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 5;

    // 该敌人拥有的怪兽
    private MonsterCard BleedWolf;

    public override void LoadEnemy()
    {
        BleedWolf = CardDataModel.Instance.GetEnemyCard(8);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(3);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(2, 4, BleedWolf);
        }
        else if (index == 1)
        {
            SummonEnenmy(2, 4, BleedWolf);
        }

        base.SummonEnemy();
    }
}