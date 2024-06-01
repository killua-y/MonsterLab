using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornWolfEntity : BaseEntity
{
    public override void TakeDamage(int amount, BaseEntity from = null)
    {
        if ((dead) || (from == null))
        {
            return;
        }

        // 返还收到的伤害
        from.TakeDamage(amount, this);

        base.TakeDamage(amount, from);
    }
}