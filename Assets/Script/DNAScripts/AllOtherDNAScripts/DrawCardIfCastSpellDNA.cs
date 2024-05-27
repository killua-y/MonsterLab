using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardIfCastSpellDNA : DNABehavior
{
    private int Counter = 0;
    // Start is called before the first frame update

    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnSpellCardPlayed += OnSpellCardPlayed;
    }

    void OnSpellCardPlayed(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        Counter += 1;
        if (Counter >= 3)
        {
            DrawCard();
        }
    }

    void OnPreparePhaseStart()
    {
        Counter = 0;
    }

    void DrawCard()
    {
        for (int i = 0; i < DNAModel.effectData; i++)
        {
            InGameStateManager.Instance.DrawOneCard();
        }
    }
}
