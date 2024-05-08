using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterEntity : BaseEntity
{

    public void Update()
    {
        if (!HasEnemy)
        {
            FindTarget();
        }

        if (IsInRange && !moving)
        {
            //In range for attack!
            if (canAttack)
            {
                Attack();
                //currentTarget.TakeDamage(baseDamage);
            }
        }
        else
        {
            GetInRange();
        }
    }

    public new void Attack()
    {
        //Debug.Log("Normal Monster Attack!!");
    }
}
