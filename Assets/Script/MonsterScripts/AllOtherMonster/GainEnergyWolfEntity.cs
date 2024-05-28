

using UnityEngine;

public class GainEnergyWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.GainEnergyUponSummon(this);
    }
}
