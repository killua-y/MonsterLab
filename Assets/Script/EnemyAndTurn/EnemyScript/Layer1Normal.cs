using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class AcidSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 2;

    // 该敌人拥有的怪兽
    private MonsterCard Slime;
    private MonsterCard AcidSlime;

    public override void LoadEnemy()
    {
        Slime = CardDataModel.Instance.GetEnemyCard(0);
        AcidSlime = CardDataModel.Instance.GetEnemyCard(1);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 4, Slime);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(3, 4, Slime);
            SummonEnenmy(1, 7, AcidSlime);
            SummonEnenmy(3, 7, AcidSlime);
        }

        base.SummonEnemy();
    }
}

public class BlackSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 3;

    // 该敌人拥有的怪兽
    private MonsterCard BlackSlime;
    private MonsterCard FireSlime;

    public override void LoadEnemy()
    {
        BlackSlime = CardDataModel.Instance.GetEnemyCard(2);
        FireSlime = CardDataModel.Instance.GetEnemyCard(3);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(3, 5, BlackSlime);
            SummonEnenmy(1, 4, FireSlime);
            SummonEnenmy(2, 5, FireSlime);
            SummonEnenmy(3, 5, FireSlime);
        }

        base.SummonEnemy();
    }
}


public class SpliteSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 3;

    // 该敌人拥有的怪兽
    private MonsterCard BlackSlime;
    private MonsterCard FireSlime;

    public override void LoadEnemy()
    {
        BlackSlime = CardDataModel.Instance.GetEnemyCard(2);
        FireSlime = CardDataModel.Instance.GetEnemyCard(3);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(3, 5, BlackSlime);
            SummonEnenmy(1, 4, FireSlime);
            SummonEnenmy(2, 5, FireSlime);
            SummonEnenmy(3, 5, FireSlime);
        }

        base.SummonEnemy();
    }
}