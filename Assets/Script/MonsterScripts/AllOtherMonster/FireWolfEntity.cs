using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.FireWolfUponSummon(this);
    }
}