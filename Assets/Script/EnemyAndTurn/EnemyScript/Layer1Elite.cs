using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class ArcherEnemy : EnemyBehavior
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

public class BombCarrierEnemy : EnemyBehavior
{
    protected new int MaxTurn = 4;

    // 该敌人拥有的怪兽
    private MonsterCard BombCarrier;
    private MonsterCard Slave;

    public override void LoadEnemy()
    {
        BombCarrier = TurnManager.Instance.monsterList[5];
        Slave = TurnManager.Instance.monsterList[7];

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(1);
        MonsterSummonTurn.Add(2);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 4, BombCarrier);
            SummonEnenmy(2, 5, BombCarrier);
            SummonEnenmy(3, 4, BombCarrier);
            SummonEnenmy(2, 4, Slave);
            SummonEnenmy(3, 7, Slave);
        }
        else if (index == 1)
        {
            SummonEnenmy(2, 5, BombCarrier);
            SummonEnenmy(2, 5, BombCarrier);
            SummonEnenmy(2, 5, BombCarrier);
        }
        else if (index == 2)
        {
            SummonEnenmy(2, 4, Slave);
            SummonEnenmy(2, 5, BombCarrier);
            SummonEnenmy(2, 5, BombCarrier);
        }

        base.SummonEnemy();
    }
}