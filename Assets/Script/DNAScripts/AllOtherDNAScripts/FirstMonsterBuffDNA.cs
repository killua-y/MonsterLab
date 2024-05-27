using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMonsterBuffDNA : DNABehavior
{
    private bool isFirst = true;

    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        BattleManager.Instance.OnUnitSummon += OnUnitSummon;
    }

    void OnUnitSummon(BaseEntity targetMonster)
    {
        if (isFirst)
        {
            targetMonster.cardModel.attackPower += DNAModel.effectData;
            targetMonster.cardModel.healthPoint += (DNAModel.effectData * 10);
            targetMonster.UpdateMonster();
            isFirst = false;
        }
    }

    void OnPreparePhaseStart()
    {
        isFirst = true;
    }
}
