using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskyEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.RepeatAllSummonEffects(this);
    }
}
