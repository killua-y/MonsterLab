using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankDevourerEntity : BaseEntity
{
    // 每有一个2星以上的怪兽，生命值翻倍
    protected override void Consume(List<BaseEntity> sacrfices)
    {
        foreach (BaseEntity sacrfice in sacrfices)
        {
            if (sacrfice.cardModel.cost >= 2)
            {
                cardModel.attackPower *= 2;
            }

            sacrfice.UnitDie(null, true);
        }
    }
}
