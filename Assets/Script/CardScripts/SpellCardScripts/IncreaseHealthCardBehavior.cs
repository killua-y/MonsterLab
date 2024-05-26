using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealthCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.healthPoint += card.effectData;

        targetMonster.UpdateMonster();
    }
}
