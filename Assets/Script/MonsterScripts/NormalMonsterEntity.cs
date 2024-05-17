using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonsterEntity : BaseEntity
{

    public void Update()
    {
        if(CanBattle)
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
                }
            }
            else
            {
                GetInRange();
            }
        }
    }
}

