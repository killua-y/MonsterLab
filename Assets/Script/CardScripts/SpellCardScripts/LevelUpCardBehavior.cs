using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower += card.effectData;
        targetMonster.cardModel.rank += 1;

        targetMonster.UpdateMonster();
    }
}
