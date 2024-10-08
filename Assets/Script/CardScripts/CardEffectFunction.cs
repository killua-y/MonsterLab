using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectFunction : MonoBehaviour
{
    public static void IncreaseHealth(BaseEntity target, int amount)
    {
        target.cardModel.healthPoint += amount;
        target.UpdateMonster();
        target.RestoreHealth(amount);
        EffectManager.Instance.PlayEffect("HealthBuffEffect", target.transform.position);
    }

    public static void IncreaseAttack(BaseEntity target, int amount)
    {
        target.cardModel.attackPower += amount;
        target.UpdateMonster();
        EffectManager.Instance.PlayEffect("AttackBuffEffect", target.transform.position);
    }
}
