using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseAttackCardBehavior : CardBehavior
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
        targetMonster.cardModel.attackPower += card.effectData;

        targetMonster.UpdateMonster();
    }
}
