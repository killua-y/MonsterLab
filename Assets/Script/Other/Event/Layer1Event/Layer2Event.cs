using System.Collections.Generic;
using static Card;
using System;

// 复制卡牌事件
public class CopyCardEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "You found a mirror in the forest that seems to have magical powers.";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Select one card, create a copy.",
        "Leave"
    };

    protected override string startSceneImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            LeaveEvent
        };
    }

    private string eventText2 = "The mirror becomes normal after spitting out a card.";

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(CopyCardHelper);
    }

    private void CopyCardHelper(Card _card)
    {
        CardDataModel.Instance.ObtainCard(_card);

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 锻造装备卡事件
public class ForgeWeaponEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "Anvil.";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Add Reuse to one of your Item Card. (Reuse: This card will go to discard pile after cast)",
        "Reduce the cost of 1 your Item Card by 1 point.",
        "Leave"
    };

    protected override string startSceneImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            Option2,
            LeaveEvent
        };
    }

    private string eventText2 = "Embark on a journey with stronger weapon.";

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetNonReuseWeaponHelper(), 1, AddReuseHelper);
    }

    private List<Card> GetNonReuseWeaponHelper()
    {
        List<Card> ItemCards = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if ((card is ItemCard) && (!card.keyWords.Contains("Reuse")))
            {
                ItemCards.Add(card);
            }
        }

        return ItemCards;
    }

    private void AddReuseHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            HelperFunction.AddKeyWordToCard(card, "Reuse");
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }

    private void Option2()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetNonZeroCostWeaponHelper(), 1, DecreaseWeaponCostHelper);
    }

    private List<Card> GetNonZeroCostWeaponHelper()
    {
        List<Card> ItemCards = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if ((card is ItemCard) && (card.cost >= 1))
            {
                ItemCards.Add(card);
            }
        }

        return ItemCards;
    }

    private void DecreaseWeaponCostHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            card.cost -= 1;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 基础单位加不休关键词
public class AddUncasingForBase : EventBehavior
{
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "Uncasing : Draw one card after this card is played.";
    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "1 of your basic units gain Uncasing.",
        "40 Gold: 2 of your basic units gain Uncasing.",
        "80 Gold: 3 of your basic units gain Uncasing.",
        "150 Gold: All of your basic units gain Uncasing.",
        "Leave"
    };

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            Option2,
            Option3,
            Option4,
            LeaveEvent
        };
    }

    // LeaveSene
    private string eventText2 = "Your units has become stronger.";

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == startSceneOptionsText[0])
        {
            if (GetBaseUnitHelper().Count < 1)
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[1])
        {
            if ((GetBaseUnitHelper().Count < 2) ||
                (PlayerStatesManager.Gold < 40))
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[2])
        {
            if ((GetBaseUnitHelper().Count < 3) ||
                (PlayerStatesManager.Gold < 80))
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[3])
        {
            if ((GetBaseUnitHelper().Count < 4) ||
                (PlayerStatesManager.Gold < 150))
            {
                return false;
            }
        }

        return base.CheckOptionValidity();
    }

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 1, IncreaseStatesForBaseHelper);
    }

    private void Option2()
    {
        PlayerStatesManager.Instance.DecreaseGold(40);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 2, IncreaseStatesForBaseHelper);
    }

    private void Option3()
    {
        PlayerStatesManager.Instance.DecreaseGold(80);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 3, IncreaseStatesForBaseHelper);
    }

    private void Option4()
    {
        PlayerStatesManager.Instance.DecreaseGold(150);
        IncreaseStatesForBaseHelper(GetBaseUnitHelper());
    }

    private List<Card> GetBaseUnitHelper()
    {
        List<Card> baseUnits = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if ((card.color == CardColor.Base) && (!card.keyWords.Contains("Unceasing")))
            {
                baseUnits.Add(card);
            }
        }

        return baseUnits;
    }

    private void IncreaseStatesForBaseHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            HelperFunction.AddKeyWordToCard(card, "Unceasing");
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 基础单位加攻加血事件
public class Layer2IncreaseStatesForBase : EventBehavior
{
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "";
    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "1 of your basic units gain 5 attack and 40 health.",
        "40 Gold: 2 of your basic units gain 5 attack and 40 health.",
        "80 Gold: 3 of your basic units gain 5 attack and 40 health.",
        "150 Gold: All of your base units gain 5 attack and 40 health.",
        "Leave"
    };

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            Option2,
            Option3,
            Option4,
            LeaveEvent
        };
    }

    // LeaveSene
    private string eventText2 = "Your units has become stronger.";

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == startSceneOptionsText[0])
        {
            if (GetBaseUnitHelper().Count < 1)
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[1])
        {
            if ((GetBaseUnitHelper().Count < 2) ||
                (PlayerStatesManager.Gold < 40))
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[2])
        {
            if ((GetBaseUnitHelper().Count < 3) ||
                (PlayerStatesManager.Gold < 80))
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[3])
        {
            if ((GetBaseUnitHelper().Count < 4) ||
                (PlayerStatesManager.Gold < 150))
            {
                return false;
            }
        }

        return base.CheckOptionValidity();
    }

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 1, IncreaseStatesForBaseHelper);
    }

    private void Option2()
    {
        PlayerStatesManager.Instance.DecreaseGold(40);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 2, IncreaseStatesForBaseHelper);
    }

    private void Option3()
    {
        PlayerStatesManager.Instance.DecreaseGold(80);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 3, IncreaseStatesForBaseHelper);
    }

    private void Option4()
    {
        PlayerStatesManager.Instance.DecreaseGold(150);
        IncreaseStatesForBaseHelper(GetBaseUnitHelper());
    }

    private List<Card> GetBaseUnitHelper()
    {
        List<Card> baseUnits = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if (card.color == CardColor.Base)
            {
                baseUnits.Add(card);
            }
        }

        return baseUnits;
    }

    private void IncreaseStatesForBaseHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            MonsterCard monsterCard = (MonsterCard)card;
            monsterCard.attackPower += 5;
            monsterCard.healthPoint += 40;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}