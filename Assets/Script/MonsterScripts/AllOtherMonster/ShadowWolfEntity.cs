using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class ShadowWolfEntity : BaseEntity
{
    public override void UponSummon()
    {
        bool isPlayerTeam;

        if (myTeam == Team.Player)
        {
            isPlayerTeam = true;
        }
        else
        {
            isPlayerTeam = false;
        }

        Node node = GridManager.Instance.GetFreeNode(currentNode.rowIndex, currentNode.columnIndex, isPlayerTeam);

        if (node != null)
        {
            MonsterCard newMonsterCard = (MonsterCard)Card.CloneCard(cardModel);
            // 设置为通常怪兽
            newMonsterCard.scriptLocation = "";
            BattleManager.Instance.InstaniateMontser(node, myTeam, newMonsterCard);
        }
    }
}
