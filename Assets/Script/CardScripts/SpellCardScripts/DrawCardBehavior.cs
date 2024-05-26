using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
