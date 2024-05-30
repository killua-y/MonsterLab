using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BombCarrierEntity : BaseEntity
{


}

public class BombMonsterGeneratorEntity : BaseEntity
{

    // 战斗阶段开始
    protected override void OnPreparePhaseStart()
    {
        base.OnPreparePhaseStart();

        // 召唤一只自爆怪兽
        MonsterCard boomer = (MonsterCard)TurnManager.Instance.monsterList[0];
        EnemyBehavior.SummonEnenmy(this.currentNode.rowIndex, this.currentNode.columnIndex, boomer);
    }
}
