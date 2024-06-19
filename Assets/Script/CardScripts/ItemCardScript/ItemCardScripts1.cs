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
        targetMonster.cardModel.attackPower += card.effectData;

        targetMonster.UpdateMonster();
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
            targetMonster.gameObject.AddComponent<IncreaseHealthOnDeathMonsterBehavior>().CardModel = newCard;
        }
        // 如果对象已经有了
        else
        {
            targetMonster.GetComponent<IncreaseHealthOnDeathMonsterBehavior>().CardModel.effectData += card.effectData;
        }

        // 如果需要加入到卡牌附加说明
        targetMonster.cardModel.equippedCard.Add(card);
    }
}
public class IncreaseHealthOnDeathMonsterBehavior : MonoBehaviour
{
    private BaseEntity baseEntity;
    public Card CardModel;

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
        baseEntity.cardModel.healthPoint += CardModel.effectData;
        baseEntity.UpdateMonster();
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

        // 如果需要加入到卡牌附加说明
        targetMonster.cardModel.equippedCard.Add(card);
    }
}
public class WarriorSoulMonsterBehaiovr : MonoBehaviour
{
    private BaseEntity baseEntity;
    public Card cardModel;
    private bool active = false;

    private void Start()
    {
        baseEntity = this.gameObject.GetComponent<BaseEntity>();

        if (baseEntity == null)
        {
            Debug.Log("Equped baseEntity is null");
        }
    }

    public void SetUp(Card _card)
    {
        cardModel = _card;
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;
        BattleManager.Instance.OnUnitDied += OnUnitDied;
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
            this.baseEntity.cardModel.attackPower -= cardModel.effectData;
            this.baseEntity.cardModel.healthPoint -= (cardModel.effectData * 10);
            baseEntity.UpdateMonster();
            active = false;
        }
    }

    private void CheckIsOnlyUnitLeft()
    {
        if (!active)
        {
            if (BattleManager.Instance.GetMyTeamEntities(baseEntity.myTeam).Count == 1)
            {
                this.baseEntity.cardModel.attackPower += cardModel.effectData;
                this.baseEntity.cardModel.healthPoint += (cardModel.effectData * 10);
                //回复全部血量
                baseEntity.UpdateMonster();
                active = true;
            }
        }
    }
}