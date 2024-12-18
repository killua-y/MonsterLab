using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MotherSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 5;

    // 该敌人拥有的怪兽
    private MonsterCard AcidSlime;
    private MonsterCard MotherSlime;

    public override void LoadEnemy()
    {
        AcidSlime = CardDataModel.Instance.GetEnemyCard(1);
        MotherSlime = CardDataModel.Instance.GetEnemyCard(6);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(3);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 4, MotherSlime);
            SummonEnenmy(3, 4, MotherSlime);
            SummonEnenmy(1, 7, AcidSlime);
            SummonEnenmy(1, 7, AcidSlime);
            SummonEnenmy(1, 7, AcidSlime);
        }
        else if (index == 1)
        {
            SummonEnenmy(2, 4, MotherSlime);
        }

        base.SummonEnemy();
    }
}