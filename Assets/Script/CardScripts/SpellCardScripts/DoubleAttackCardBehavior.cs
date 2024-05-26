using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower *= card.effectData;

        targetMonster.UpdateMonster();
    }
}
