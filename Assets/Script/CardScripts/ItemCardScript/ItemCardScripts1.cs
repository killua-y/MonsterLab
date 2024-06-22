using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//翻倍攻击力
public class DoubleAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower *= card.effectData;
        targetMonster.UpdateMonster();
    }
}

// 增加攻击力
public class IncreaseAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseAttack(targetMonster, card.effectData);
    }
}

// 增加血量
public class VineArmorCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseHealth(targetMonster, card.effectData);
    }
}

// 死亡增加血量
public class IncreaseHealthOnDeathCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster = node.currentEntity;

        // 如果对象没有被装备
        if (targetMonster.GetComponent<IncreaseHealthOnDeathMonsterBehavior>() == null)
        {
            Card newCard = Card.CloneCard(this.card);
            targetMonster.gameObject.AddComponent<IncreaseHealthOnDeathMonsterBehavior>().cardModel = newCard;
        }
        // 如果对象已经有了
        else
        {
            targetMonster.GetComponent<IncreaseHealthOnDeathMonsterBehavior>().cardModel.effectData += card.effectData;
        }

        // 如果需要加入到卡牌说明
        RecordCast(targetMonster);
    }
}
public class IncreaseHealthOnDeathMonsterBehavior : MonoBehaviour
{
    private BaseEntity baseEntity;
    public Card cardModel;

    private void Start()
    {
        baseEntity = this.gameObject.GetComponent<BaseEntity>();
        if (baseEntity != null)
        {
            baseEntity.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        CardEffectFunction.IncreaseHealth(baseEntity, cardModel.effectData);
    }

    private void OnDestroy()
    {
        baseEntity.OnDeath -= OnDeath;
    }
}

// 减少攻击力
public class ReduceAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower -= card.effectData;

        // 攻击力不会小于1
        if (targetMonster.cardModel.attackPower <= 0)
        {
            targetMonster.cardModel.attackPower = 1;
        }

        targetMonster.UpdateMonster();
    }
}


public class WarriorSoulCardBehaiovr : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster = node.currentEntity;

        // 如果对象没有被装备
        if (targetMonster.GetComponent<WarriorSoulCardBehaiovr>() == null)
        {
            Card newCard = Card.CloneCard(this.card);
            targetMonster.gameObject.AddComponent<WarriorSoulMonsterBehaiovr>().SetUp(newCard);
        }
        // 如果对象已经被装备
        else
        {
            targetMonster.GetComponent<WarriorSoulMonsterBehaiovr>().cardModel.effectData += card.effectData;
        }

        // 如果需要加入到卡牌说明
        RecordCast(targetMonster);
    }
}
public class WarriorSoulMonsterBehaiovr : MonoBehaviour
{
    private BaseEntity baseEntity;
    public Card cardModel;
    private bool active = false;

    public void SetUp(Card _card)
    {
        cardModel = _card;
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;
        BattleManager.Instance.OnUnitDied += OnUnitDied;

        baseEntity = this.gameObject.GetComponent<BaseEntity>();

        if (baseEntity == null)
        {
            Debug.Log("Equped baseEntity is null");
        }

        // 将攻击距离变为1格
        baseEntity.range = 1.5f;
        baseEntity.cardModel.attackRange = 1.5f;
    }

    void OnUnitDied(BaseEntity baseEntity)
    {
        CheckIsOnlyUnitLeft();
    }

    void OnBattlePhaseStart()
    {
        CheckIsOnlyUnitLeft();
    }

    void OnBattlePhaseEnd()
    {
        if (active)
        {
            CardEffectFunction.IncreaseAttack(baseEntity, -cardModel.effectData);
            CardEffectFunction.IncreaseHealth(baseEntity, -cardModel.effectData * 10);
            active = false;
        }
    }

    private void CheckIsOnlyUnitLeft()
    {
        if (!active)
        {
            if (BattleManager.Instance.GetMyTeamEntities(baseEntity.myTeam).Count == 1)
            {
                CardEffectFunction.IncreaseAttack(baseEntity, cardModel.effectData);
                CardEffectFunction.IncreaseHealth(baseEntity, cardModel.effectData * 10);
                active = true;
            }
        }
    }

    private void OnDestroy()
    {
        InGameStateManager.Instance.OnBattlePhaseStart -= OnBattlePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd -= OnBattlePhaseEnd;
        BattleManager.Instance.OnUnitDied -= OnUnitDied;
    }
}

