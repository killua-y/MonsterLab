using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 重复全部
public class HuskyEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.RepeatAllSummonEffects(this);
    }
}

// 抽牌
public class BabyWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.BabyWolfUponSummon(this);
    }
}

// 能量
public class GainEnergyWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.GainEnergyUponSummon(this);
    }
}

// aoe打10
public class FireWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.FireWolfUponSummon(this);
    }
}

// 复制狼
public class ShadowWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.ShadowWolfUponSummon(this);
    }
}