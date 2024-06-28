using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class FireBallCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.TakeDamage(card.effectData, DamageType.Spell);
    }
}

public class IncreaseHealthCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseHealth(targetMonster, card.effectData);
    }
}

public class DrawCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        InGameStateManager.Instance.DrawCards(card.effectData);
    }
}

public class LevelUpCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseAttack(targetMonster, card.effectData);
        targetMonster.cardModel.rank += 1;

        targetMonster.UpdateMonster();
    }
}

public class MagneticGrabberCardBehavior : CardBehavior
{
    public override void CheckLegality(Node node)
    {
        if (node.currentEntity.GetComponent<DragMonster>() != null)
        {
            Debug.Log("This monster already able to drag");

            return;
        }

        base.CheckLegality(node);
    }

    public override void CastCard(Node node)
    {
        targetMonster.gameObject.AddComponent<DragMonster>();

        // 如果需要加入到卡牌说明
        RecordCast(targetMonster);
    }
}

public class MirrorCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        Card newCard = Card.CloneCard(targetMonster.cardModel);

        InGameStateManager.Instance.AddToHand(newCard);
    }
}

public class SimpleCloneCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        MonsterCard newCard = (MonsterCard)Card.CloneCard(targetMonster.cardModel);
        newCard.attackPower = 1;
        newCard.healthPoint = 1;

        InGameStateManager.Instance.AddToHand(newCard);
    }
}

public class SimpleSlimeSummonCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        MonsterCard Slime = (MonsterCard)Card.CloneCard(CardDataModel.Instance.GetCard(card.effectData));

        BattleManager.Instance.InstaniateMontser(node, Team.Player, Slime);
    }
}

public class SummonBlackSlimeCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        for (int i = 0; i < 3; i++)
        {
            MonsterCard blackSlime = (MonsterCard)Card.CloneCard(CardDataModel.Instance.GetCard(card.effectData));

            BattleManager.Instance.InstaniateMontser(GridManager.Instance.GetFreeNode(2, 3, true), Team.Player, blackSlime);
        }
    }
}

public class DemonContractCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.UnitDie(null, true);
        FindAnyObjectByType<PlayerCostManager>().IncreaseCost(card.effectData);
        InGameStateManager.Instance.DrawCards(card.effectData * 2);
    }
}