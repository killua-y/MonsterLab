using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BombCarrierEntity : BaseEntity
{
    public override void UponDeath()
    {
        // 战斗中自爆产生1.5格的范围伤害
        if (canBattle)
        {
            List<BaseEntity> allEnemies = BattleManager.Instance.GetEntitiesAgainst(myTeam);
            if (allEnemies.Count == 0)
            {
                return;
            }

            float minDistance = range;
            List<BaseEntity> entitys = new List<BaseEntity>();
            foreach (BaseEntity e in allEnemies)
            {
                if (Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
                {
                    entitys.Add(e);
                }
            }

            float damage = ((float)cardModel.effectData / 100) * cardModel.attackPower;
            int intDamage = (int)damage;
            // 对每个敌人造成爆炸伤害
            foreach (BaseEntity e in entitys)
            {
                e.TakeDamage(intDamage, this);
            }
        }

        base.UponDeath();
    }
}

public class BombMonsterGeneratorEntity : BaseEntity
{
    private MonsterCard boomer;

    protected override void Start()
    {
        Invoke("SummonChild", 0);

        base.Start();
    }

    public override void Update()
    {
        // 防御塔型单位, 什么都不做
    }

    // 战斗阶段开始
    protected override void OnPreparePhaseStart()
    {
        base.OnPreparePhaseStart();

        Invoke("SummonChild", 0);
    }

    private void SummonChild()
    {
        // 召唤一只自爆怪兽
        if (boomer == null)
        {
            boomer = CardDataModel.Instance.GetEnemyCard(cardModel.effectData);
        }

        EnemyBehavior.SummonEnenmy(2, 4, boomer);
    }
}

public class MotherSlimeEntity: BaseEntity
{
    private MonsterCard acidSlime;

    // 战斗阶段开始
    protected override void OnPreparePhaseStart()
    {
        base.OnPreparePhaseStart();

        Invoke("SummonChild", 0);
    }

    private void SummonChild()
    {
        // 召唤一只史莱姆
        if (acidSlime == null)
        {
            acidSlime = CardDataModel.Instance.GetEnemyCard(cardModel.effectData);
        }

        EnemyBehavior.SummonEnenmy(2, 7, acidSlime);
    }
}
