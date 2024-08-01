using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 攻击增加攻击力
public class Strike2AttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster = node.currentEntity;

        Strike2AttackMonsterBehavior strike2AttackMonsterBehavior = targetMonster.GetComponent<Strike2AttackMonsterBehavior>();

        // 如果对象没有被装备
        if (strike2AttackMonsterBehavior == null)
        {
            strike2AttackMonsterBehavior = targetMonster.gameObject.AddComponent<Strike2AttackMonsterBehavior>();
        }

        strike2AttackMonsterBehavior.effectData += cardModel.effectData;

        // 如果需要加入到卡牌说明
        RecordCast(targetMonster);
    }
}
public class Strike2AttackMonsterBehavior : MonoBehaviour
{
    private BaseEntity baseEntity;
    public int effectData;

    private void Start()
    {
        baseEntity = this.gameObject.GetComponent<BaseEntity>();

        if (baseEntity == null)
        {
            Debug.Log("equped to null monster");
        }
        else
        {
            baseEntity.OnStrike += OnStrike;
        }
    }

    private void OnStrike(int amount, BaseEntity targetMonster)
    {
        // 增加攻击力
        CardEffectFunction.IncreaseAttack(baseEntity, effectData);
    }

    private void OnDestroy()
    {
        baseEntity.OnStrike -= OnStrike;
    }
}

// 防御
public class ApplyBlockCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster = node.currentEntity;

        Strike2AttackMonsterBehavior strike2AttackMonsterBehavior = targetMonster.GetComponent<Strike2AttackMonsterBehavior>();

        // 如果对象没有被装备
        if (strike2AttackMonsterBehavior == null)
        {
            strike2AttackMonsterBehavior = targetMonster.gameObject.AddComponent<Strike2AttackMonsterBehavior>();
        }

        strike2AttackMonsterBehavior.effectData += cardModel.effectData;

        // 如果需要加入到卡牌说明
        RecordCast(targetMonster);
    }
}
public class ApplyBlockMonsterBehavior : MonoBehaviour
{
    private BaseEntity baseEntity;
    public int effectData;

    private void Start()
    {
        baseEntity = this.gameObject.GetComponent<BaseEntity>();

        if (baseEntity == null)
        {
            Debug.Log("equped to null monster");
        }
        else
        {
            baseEntity.OnStrike += OnStrike;
        }
    }

    private void OnStrike(int amount, BaseEntity targetMonster)
    {
        // 增加攻击力
        CardEffectFunction.IncreaseAttack(baseEntity, effectData);
    }

    private void OnDestroy()
    {
        baseEntity.OnStrike -= OnStrike;
    }
}