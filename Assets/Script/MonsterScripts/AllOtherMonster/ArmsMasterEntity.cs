using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsMasterEntity : BaseEntity
{
    public override void ReceiveWeapon(CardBehavior cardBehavior)
    {
        cardBehavior.CastCard(this.currentNode);
    }
}
