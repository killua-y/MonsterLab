using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BombCarrierEntity : BaseEntity
{

    public override void UponDeath()
    {
        // 战斗中自爆产生1.5格的范围伤害
        if (CanBattle)
        {
            var allEnemies = BattleManager.Instance.GetEntitiesAgainst(myTeam);
            float minDistance = range;
            List<BaseEntity> entitys = null;
            foreach (BaseEntity e in allEnemies)
            {
                if (Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
                {
                    entitys.Add(e);
                }
            }

            // 对每个敌人造成爆炸伤害
            foreach (BaseEntity e in entitys)
            {
                int damage = (cardModel.effectData / 100) * cardModel.attackPower;
                e.TakeDamage(damage, this);
            }
        }

        base.UponDeath();
    }
}

public class BombMonsterGeneratorEntity : BaseEntity
{
    protected override void Start()
    {
        Invoke("SummonChild", 0);

        base.Start();
    }

    public override void Update()
    {
        // 什么都不做
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
        MonsterCard boomer = (MonsterCard)TurnManager.Instance.monsterList[5];
        EnemyBehavior.SummonEnenmy(this.currentNode.rowIndex, this.currentNode.columnIndex, boomer);
    }
}

public class MotherSlimeEntity: BaseEntity
{
    // 战斗阶段开始
    protected override void OnPreparePhaseStart()
    {
        base.OnPreparePhaseStart();

        Invoke("SummonChild", 0);
    }

    private void SummonChild()
    {
        // 召唤一只史莱姆
        MonsterCard AcidSlime = (MonsterCard)TurnManager.Instance.monsterList[1];
        EnemyBehavior.SummonEnenmy(2, 7, AcidSlime);
    }
}
