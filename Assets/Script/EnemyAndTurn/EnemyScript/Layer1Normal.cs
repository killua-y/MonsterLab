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


public class ALotOfSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard AcidSlime;
    private MonsterCard FireSlime;

    public override void LoadEnemy()
    {
        AcidSlime = CardDataModel.Instance.GetEnemyCard(1);
        FireSlime = CardDataModel.Instance.GetEnemyCard(3);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(1);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(2, 4, FireSlime);
            SummonEnenmy(2, 4, FireSlime);
            SummonEnenmy(2, 4, FireSlime);
            SummonEnenmy(2, 7, AcidSlime);
            SummonEnenmy(2, 7, AcidSlime);
            SummonEnenmy(2, 7, AcidSlime);
        }
        else if (index == 1)
        {
            SummonEnenmy(2, 4, FireSlime);
            SummonEnenmy(2, 7, AcidSlime);
        }

        base.SummonEnemy();
    }
}

public class SalveAndSlimeEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard Slime;
    private MonsterCard Slave;

    public override void LoadEnemy()
    {
        Slime = CardDataModel.Instance.GetEnemyCard(0);
        Slave = CardDataModel.Instance.GetEnemyCard(7);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(0, 4, Slave);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(2, 4, Slime);
            SummonEnenmy(4, 4, Slave);
        }

        base.SummonEnemy();
    }
}

public class BombCarrierEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard BombCarrier;
    private MonsterCard Slave;

    public override void LoadEnemy()
    {
        BombCarrier = CardDataModel.Instance.GetEnemyCard(5);
        Slave = CardDataModel.Instance.GetEnemyCard(7);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
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
        }

        base.SummonEnemy();
    }
}


public class TestEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 2;

    // 该敌人拥有的怪兽
    private MonsterCard Slime;
    private MonsterCard AcidSlime;
    private MonsterCard FireSlime;
    private MonsterCard wolf;
    private MonsterCard archer;

    public override void LoadEnemy()
    {
        Slime = CardDataModel.Instance.GetEnemyCard(0);
        AcidSlime = CardDataModel.Instance.GetEnemyCard(1);
        FireSlime = CardDataModel.Instance.GetEnemyCard(3);
        archer = CardDataModel.Instance.GetEnemyCard(4);
        wolf = CardDataModel.Instance.GetEnemyCard(8);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(1, 4, FireSlime);
            SummonEnenmy(2, 4, wolf);
            SummonEnenmy(3, 4, Slime);
            SummonEnenmy(1, 6, AcidSlime);
            SummonEnenmy(2, 7, archer);
            SummonEnenmy(3, 6, AcidSlime);
        }

        base.SummonEnemy();
    }
}