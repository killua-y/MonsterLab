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

// 打100
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

// 全队加攻狼
public class IncreaseAttackWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.IncreaseAllAttackUponSummon(this);
    }
}

// 获得0费装备卡狼
public class ZeroCostItemWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.Gain0CostItemCardUponSummon(this);
    }
}

// 根据所有base unit增加属性
public class KingWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        UponSummonFunction.KingWolfUponSummon(this);
    }
}

// Jax
public class JaxEntity : BaseEntity
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
        }
    }

    protected override void OnDestroy()
    {
        InGameStateManager.Instance.OnItemCardPlayed -= ReceiveWeapon;
        base.OnDestroy();
    }
}


// 反伤
public class ThornWolfEntity : BaseEntity
{
    public override void TakeDamage(int amount, DamageType damageType, BaseEntity from = null)
    {
        if ((dead) || (from == null) || (from.dead))
        {
            return;
        }

        // 返还收到的伤害
        from.TakeDamage(amount, DamageType.MonsterSkill, this);

        base.TakeDamage(amount, damageType, from);
    }
}

// 使用装备卡抽牌
public class BlackSmithEntity : BaseEntity
{
    protected override void Start()
    {
        InGameStateManager.Instance.OnItemCardPlayed += ReceiveWeapon;
        base.Start();
    }

    // 每次释放装备卡都抽一张牌
    private void ReceiveWeapon(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        InGameStateManager.Instance.DrawOneCard();
    }

    protected override void OnDestroy()
    {
        InGameStateManager.Instance.OnItemCardPlayed -= ReceiveWeapon;
        base.OnDestroy();
    }
}

// 根据祭品星级翻倍攻击力
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
        }

        base.Consume(sacrfices);
    }
}

// 根据祭品属性增加自身属性
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
