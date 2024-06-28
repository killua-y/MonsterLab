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

    void OnUnitSummon(BaseEntity baseEntity)
    {
        if (baseEntity.myTeam == Team.Player)
        {
            if (isFirst)
            {
                baseEntity.cardModel.attackPower += DNAModel.effectData;
                baseEntity.cardModel.healthPoint += (DNAModel.effectData * 10);
                baseEntity.RestoreHealth((DNAModel.effectData * 10));
                baseEntity.UpdateMonster();
                isFirst = false;
            }
        }
    }

    void OnPreparePhaseStart()
    {
        isFirst = true;
    }
}
