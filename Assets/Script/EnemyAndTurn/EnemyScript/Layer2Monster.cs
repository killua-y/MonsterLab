using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BleedWolfEntity : BaseEntity
{
    // private
    private Card BleedCard;

    protected override void Start()
    {
        BattleManager.Instance.OnUnitDied += OnUnitDead;
        BleedCard = CardDataModel.Instance.GetCard(cardModel.effectData);

        base.Start();
    }

    private void OnUnitDead(BaseEntity baseEntity)
    {
        // 如果在战斗中
        if (InGameStateManager.BattelPhase)
        {
            // 如果死亡单位是敌方
            if (baseEntity.myTeam != myTeam)
            {
                // 往玩家卡组中加入一张裂伤牌
                InGameStateManager.Instance.AddToDrawPile(Card.CloneCard(BleedCard));
            }
        }
    }

    protected override void OnDestroy()
    {
        BattleManager.Instance.OnUnitDied -= OnUnitDead;
        base.OnDestroy();
    }
}
