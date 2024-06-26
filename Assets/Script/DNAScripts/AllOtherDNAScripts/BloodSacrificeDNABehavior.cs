using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class BloodSacrificeDNABehavior : DNABehavior
{
    // private
    private InGameCardModel inGameCardModel;

    private void Awake()
    {
        inGameCardModel = FindAnyObjectByType<InGameCardModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
    }

    private void OnCombatStart()
    {
        List<MonsterCard> cards = new List<MonsterCard>();

        foreach (Card card in inGameCardModel.GetDrawPileCard())
        {
            if (card is MonsterCard)
            {
                cards.Add((MonsterCard)card);
            }
        }

        foreach (Card card in inGameCardModel.GetExtraDeckPileCard())
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
