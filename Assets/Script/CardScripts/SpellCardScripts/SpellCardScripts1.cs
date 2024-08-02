using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

// 火球术，造成伤害
public class FireBallCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        EffectManager.Instance.PlayEffect("FireBallEffect", targetMonster.transform.position);
        targetMonster.TakeDamage(cardModel.effectData, DamageType.Spell);
    }
}

// 增加生命值
public class IncreaseHealthCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseHealth(targetMonster, cardModel.effectData);
    }
}

// 抽牌
public class DrawCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        InGameStateManager.Instance.DrawCards(cardModel.effectData);
    }
}

// 提高攻击力和rank
public class LevelUpCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        CardEffectFunction.IncreaseAttack(targetMonster, cardModel.effectData);
        targetMonster.cardModel.rank += 1;

        targetMonster.UpdateMonster();
    }
}

// 让你可以移动一只敌方怪兽
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

// 制造一个保留所有buff的怪兽复制
public class MirrorCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        Card newCard = Card.CloneCard(targetMonster.cardModel);

        InGameStateManager.Instance.AddToHand(newCard);
    }
}

// 制造一个1/1的怪兽复制
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

// 召唤一只史莱姆
public class SimpleSlimeSummonCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        MonsterCard Slime = (MonsterCard)Card.CloneCard(CardDataModel.Instance.GetCard(cardModel.effectData));

        BattleManager.Instance.InstaniateMontser(node, Team.Player, Slime);
    }
}

// 召唤3只黑色史莱姆
public class SummonBlackSlimeCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        for (int i = 0; i < 3; i++)
        {
            MonsterCard blackSlime = (MonsterCard)Card.CloneCard(CardDataModel.Instance.GetCard(cardModel.effectData));

            BattleManager.Instance.InstaniateMontser(GridManager.Instance.GetFreeNode(2, 3, true), Team.Player, blackSlime);
        }
    }
}

// 献祭一个怪兽，回复能量并抽牌
public class DemonContractCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.UnitDie(null, true);
        FindAnyObjectByType<PlayerCostManager>().IncreaseCost(cardModel.effectData);
        InGameStateManager.Instance.DrawCards(cardModel.effectData * 2);
    }
}

// 对一数列的敌人造成伤害
public class HunterTrapCardBehavior : CardBehavior
{
    private List<BaseEntity> targetBaseEntities;
    private List<Tile> previousTargetTileColumn = null;

    protected override void SetHighlight(Tile tileUnder)
    {
        base.SetHighlight(tileUnder);

        if (previousTargetTileColumn != null)
        {
            foreach (Tile tile in previousTargetTileColumn)
            {
                tile.SetHighlight(false, false);
            }
        }

        List<Tile> targetTileColumn = new List<Tile>();
        int columnIndex = GridManager.Instance.GetNodeForTile(tileUnder).columnIndex;

        for (int i = 0; i < 5; i ++)
        {
            targetTileColumn.Add(GridManager.Instance.GetTileForRowAndColumn(i, columnIndex));
        }

        foreach (Tile tile in targetTileColumn)
        {
            tile.SetHighlight(true, isValid);
        }

        previousTargetTileColumn = targetTileColumn;
    }

    public override void OnPointUp()
    {
        base.OnPointUp();
        if (previousTargetTileColumn != null)
        {
            foreach (Tile tile in previousTargetTileColumn)
            {
                tile.SetHighlight(false, false);
            }
        }
    }

    public override void CheckLegality(Node node)
    {
        targetBaseEntities = new List<BaseEntity>();
        List<Node> nodes = new List<Node>();

        for (int i = 0; i < 5; i++)
        {
            nodes.Add(GridManager.Instance.GetNodeForRowAndColumn(i, node.columnIndex));
        }

        foreach (Node _node in nodes)
        {
            if (_node.IsOccupied)
            {
                targetBaseEntities.Add(_node.currentEntity);
            }
        }

        // 查看当前竖排是否有敌人
        // 如果没有就return
        if (targetBaseEntities.Count == 0)
        {
            Debug.Log("There are no monster in this column");

            return;
        }

        base.CheckLegality(node);
    }

    public override void CastCard(Node node)
    {
        if (targetBaseEntities == null)
        {
            targetBaseEntities = new List<BaseEntity>();
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < 5; i++)
            {
                nodes.Add(GridManager.Instance.GetNodeForRowAndColumn(i, node.columnIndex));
            }

            foreach (Node _node in nodes)
            {
                if (_node.IsOccupied)
                {
                    targetBaseEntities.Add(_node.currentEntity);
                }
            }
        }
        else
        {
            foreach (BaseEntity baseEntity in targetBaseEntities)
            {
                BleedingStack bleedingStack = baseEntity.GetComponent<BleedingStack>();
                if (bleedingStack == null)
                {
                    bleedingStack = baseEntity.gameObject.AddComponent<BleedingStack>();
                }

                // 施加流血
                bleedingStack.IncreaseStack(cardModel.effectData);
            }
        }
    }
}

// 根据场上己方怪兽的星级造成伤害
public class FallingStarCardBehavior: CardBehavior
{
    public override void CastCard(Node node)
    {
        int totalRank = 0;

        foreach (BaseEntity baseEntity in BattleManager.Instance.GetMyTeamEntities(Team.Player))
        {
            totalRank += baseEntity.cardModel.rank;
        }

        targetMonster.TakeDamage(totalRank * cardModel.effectData, DamageType.Spell);
    }
}

// 抽取抽牌堆中的两只怪兽
public class AssembleCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        InGameCardModel inGameCardModel = FindAnyObjectByType<InGameCardModel>();
        List<Card> cardsTobeDrawed = new List<Card>();

        foreach (Card _card in inGameCardModel.GetDrawPileCard())
        {
            if (_card is MonsterCard)
            {
                cardsTobeDrawed.Add(_card);
            }

            if (cardsTobeDrawed.Count >= cardModel.effectData)
            {
                break;
            }
        }

        if (cardsTobeDrawed.Count > 0)
        {
            foreach (Card card in cardsTobeDrawed)
            {
                InGameStateManager.Instance.DrawSpecificCard(card);
            }
        }
        else
        {
            Debug.Log("No monster card in draw pile");
        }
    }
}

