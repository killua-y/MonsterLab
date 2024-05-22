using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardBehavior : CardBehavior
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void CastCard(Node node)
    {
        //n张抽牌
        for (int i = 0; i < card.effectData; i++)
        {
            InGameStateManager.Instance.DrawOneCard();
        }
    }
}
