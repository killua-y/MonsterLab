using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class ArcherEnermy : EnemyBehavior
{
    protected new int MaxTurn = 4;

    // 该敌人拥有的怪兽
    private MonsterCard Archer;
    private MonsterCard BlackSlime;

    public override void LoadEnemy()
    {
        Archer = TurnManager.Instance.monsterList[4];
        BlackSlime = TurnManager.Instance.monsterList[2];

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(3);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(2, 7, Archer);
            SummonEnenmy(1, 5, BlackSlime);
            SummonEnenmy(3, 5, BlackSlime);
        }
        else if (index == 1)
        {
            SummonEnenmy(3, 5, BlackSlime);
        }

        base.SummonEnemy();
    }
}

public class BombEnermy : EnemyBehavior
{
    protected new int MaxTurn = 2;

    // 该敌人拥有的怪兽
    private MonsterCard Slime;
    private MonsterCard AcidSlime;

    public override void LoadEnemy()
    {
        Slime = TurnManager.Instance.monsterList[0];
        AcidSlime = TurnManager.Instance.monsterList[1];

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 5, Slime);
            SummonEnenmy(2, 5, Slime);
            SummonEnenmy(3, 5, Slime);
            SummonEnenmy(1, 7, AcidSlime);
            SummonEnenmy(3, 7, AcidSlime);
        }

        base.SummonEnemy();
    }
}