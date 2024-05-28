using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerWolfBaseEntity : BaseEntity
{
    // 自己的数据会增加召唤物的数值
    protected override void Consume(List<BaseEntity> sacrfices)
    {
        foreach (BaseEntity sacrfice in sacrfices)
        {
            cardModel.attackPower += sacrfice.cardModel.attackPower;
            cardModel.healthPoint += sacrfice.cardModel.healthPoint;
        }

        base.Consume(sacrfices);
    }
}
