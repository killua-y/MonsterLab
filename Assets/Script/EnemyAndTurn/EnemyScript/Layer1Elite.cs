using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class ArcherEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard Archer;
    private MonsterCard BlackSlime;

    public override void LoadEnemy()
    {
        Archer = CardDataModel.Instance.GetEnemyCard(4);
        BlackSlime = CardDataModel.Instance.GetEnemyCard(2);

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

public class BombCarrierFactoryEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard BombMonsterGenerator;
    private MonsterCard Slime;

    public override void LoadEnemy()
    {
        BombMonsterGenerator = CardDataModel.Instance.GetEnemyCard(9);
        Slime = CardDataModel.Instance.GetEnemyCard(0);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(1);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(0, 7, BombMonsterGenerator);
            SummonEnenmy(2, 7, BombMonsterGenerator);
            SummonEnenmy(4, 7, BombMonsterGenerator);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
        }
        else if (index == 1)
        {
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
        }

        base.SummonEnemy();
    }
}