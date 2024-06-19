using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class FireBallCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.TakeDamage(card.effectData);
    }
}

public class IncreaseHealthCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.healthPoint += card.effectData;

        targetMonster.UpdateMonster();
    }
}

public class DrawCardBehavior : CardBehavior
{

    public override void CastCard(Node node)
    {
        //n张抽牌
        for (int i = 0; i < card.effectData; i++)
        {
            InGameStateManager.Instance.DrawOneCard();
        }
    }
}

public class LevelUpCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower += card.effectData;
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

        // 如果需要加入到卡牌附加说明
        targetMonster.cardModel.equippedCard.Add(card);
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