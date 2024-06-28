using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingStack : MonoBehaviour
{
    private BaseEntity equipedMonster;
    public int stackAmount;

    // Start is called before the first frame update
    void Awake()
    {
        equipedMonster = this.GetComponent<BaseEntity>();

        if (equipedMonster == null)
        {
            Debug.Log("Did not find equpied monster for bleeding stack");
        }

        BattleManager.Instance.BeforeBattlePhase += BeforeBattlePhase;
        stackAmount = 0;
    }

    public void IncreaseStack(int amount)
    {
        stackAmount += amount;
    }

    private void BeforeBattlePhase()
    {
        // 施加流血伤害
        equipedMonster.TakeDamage(stackAmount * 2, DamageType.Bleeding);
    }

    private void OnDestroy()
    {
        BattleManager.Instance.BeforeBattlePhase -= BeforeBattlePhase;
    }
}
