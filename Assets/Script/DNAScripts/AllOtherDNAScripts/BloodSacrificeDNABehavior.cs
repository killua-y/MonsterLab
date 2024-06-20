using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BloodSacrificeDNABehavior : DNABehavior
{
    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
    }

    private void OnCombatStart()
    {
        List<MonsterCard> cards = new List<MonsterCard>();

        foreach (Card card in InGameCardModel.Instance.GetDrawPileCard())
        {
            if (card is MonsterCard)
            {
                cards.Add((MonsterCard)card);
            }
        }

        foreach (Card card in InGameCardModel.Instance.GetExtraDeckPileCard())
        {
            if (card is MonsterCard)
            {
                cards.Add((MonsterCard)card);
            }
        }

        foreach (MonsterCard card in cards)
        {
            if (card.rank >= 1)
            {
                card.cost -= 1;
            }
        }
    }
}
