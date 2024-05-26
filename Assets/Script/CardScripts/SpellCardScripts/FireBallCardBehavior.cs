using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.TakeDamage(card.effectData);
    }
}
