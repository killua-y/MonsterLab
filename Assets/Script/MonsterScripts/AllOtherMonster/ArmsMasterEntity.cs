using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsMasterEntity : BaseEntity
{
    protected override void Start()
    {
        InGameStateManager.Instance.OnItemCardPlayed += ReceiveWeapon;
        base.Start();
    }

    // 对自己释放的装备卡会被再次释放
    private void ReceiveWeapon(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        if (targetMonster == this)
        {
            cardBehavior.CastCard(this.currentNode);
            cardBehavior.IndividualCastComplete(this.currentNode);
        }
    }

    private void OnDestroy()
    {
        InGameStateManager.Instance.OnItemCardPlayed -= ReceiveWeapon;
    }
}
