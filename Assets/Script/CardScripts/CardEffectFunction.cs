using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectFunction : MonoBehaviour
{
    public static void IncreaseHealth(BaseEntity target, int amount)
    {
        target.cardModel.healthPoint += amount;
        target.RestoreHealth(amount);
        target.UpdateMonster();
    }

    public static void IncreaseAttack(BaseEntity target, int amount)
    {
        target.cardModel.attackPower += amount;
        target.UpdateMonster();
    }
}
