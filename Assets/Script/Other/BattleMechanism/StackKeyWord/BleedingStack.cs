using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingStack : MonoBehaviour
{
    private BaseEntity equipedMonster;
    private StatusUnitBehavior StatesUnit;
    public int stackAmount;
    public static bool triggerTwice;

    // Start is called before the first frame update
    void Awake()
    {
        equipedMonster = this.GetComponent<BaseEntity>();

        if (equipedMonster == null)
        {
            Debug.Log("Did not find equpied monster for bleeding stack");
        }

        BattleManager.Instance.BeforeBattlePhase += BeforeBattlePhase;
        StatesUnit = this.GetComponent<MonsterUI>().AddNewStatus();
        stackAmount = 0;
    }

    public void IncreaseStack(int amount)
    {
        stackAmount += amount;
        StatesUnit.UpdateStatusNumber(stackAmount);
    }

    private void BeforeBattlePhase()
    {
        if (stackAmount * 2 >= 15)
        {
            EffectManager.Instance.PlayEffect("BloodEffect2", equipedMonster.transform.position);
        }
        else if (stackAmount * 2 >= 100)
        {
            EffectManager.Instance.PlayEffect("BloodEffect3", equipedMonster.transform.position);
        }
        else
        {
            EffectManager.Instance.PlayEffect("BloodEffect1", equipedMonster.transform.position);
        }

        // 施加流血伤害
        equipedMonster.TakeDamage(stackAmount * 2, DamageType.Bleeding);

        // 再次触发
        if (BleedingTriggerTwiceDNABehavior.canActiveTwice)
        {
            equipedMonster.TakeDamage(stackAmount * 2, DamageType.Bleeding);
        }
    }

    private void OnDestroy()
    {
        BattleManager.Instance.BeforeBattlePhase -= BeforeBattlePhase;
    }
}
