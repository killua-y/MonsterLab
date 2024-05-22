using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyWolfEntity : BaseEntity
{
    // Upon Summon: Draw n card
    public override void UponSummon()
    {
        for (int i = 0; i < cardModel.effectData; i++)
        {
            InGameStateManager.Instance.DrawOneCard();
        }
    }
}
