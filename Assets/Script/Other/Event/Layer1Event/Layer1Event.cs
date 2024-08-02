using System;
using System.Collections.Generic;
using UnityEngine;
using static Card;

// 删卡事件
public class DeleteOneCardEvent : EventBehavior
{
    // StartScene
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "Would you like to try this hot spring that can make your steps lighter?";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Remove one card from your deck",
        "Not what I want, leave"
    };

    // LeaveSene
    private string eventText2 = "You have left the hot spring.";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            LeaveEvent
        };
    }

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == startSceneOptionsText[0])
        {
            return CardDataModel.Instance.GetPlayerDeck().Count > 0;
        }

        return base.CheckOptionValidity();
    }

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(Option1Helper);
    }

    private void Option1Helper(Card card)
    {
        CardDataModel.Instance.DeleteCard(card);
        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 选卡事件
public class SelectOneCardEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "Select one card event";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Select One Card",
    };

    protected override string startSceneImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
        };
    }

    private void Option1()
    {
        RewardManager.Instance.GenerateCardReward(1);
        CloseEventPanel();
    }
}

// 给钱的事件
public class GainGoldEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "You find a dead body and while search on it";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Gain 150 gold",
    };

    protected override string startSceneImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
        };
    }

    private void Option1()
    {
        PlayerStatesManager.Instance.IncreaseGold(150);
        CloseEventPanel();
    }
}

// 增强怪兽卡事件
public class EnhanceMonsterEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "altar.";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Increase the 10 attack power of one of your monster",
        "Increase the 80 health point of one of your monster",
        "50 Gold: Increase 10 attack and 80 health of one of your monster",
        "Leave"
    };

    protected override string startSceneImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        startSceneOptionsAction = new List<Action>()
        {
            Option1,
            Option2,
            Option3,
            LeaveEvent
        };
    }

    // LeaveSene
    private string eventText2 = "Your unit has become stronger.";

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == startSceneOptionsText[0])
        {
            if (GetMonsterCardHelper().Count < 1)
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[1])
        {
            if (GetMonsterCardHelper().Count < 1)
            {
                return false;
            }
        }
        else if (_optionText == startSceneOptionsText[2])
        {
            if ((GetMonsterCardHelper().Count < 1) ||
                (PlayerStatesManager.Gold < 50))
            {
                return false;
            }
        }

        return base.CheckOptionValidity();
    }

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetMonsterCardHelper(), 1, IncreaseAttackHelper);
    }

    private void Option2()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetMonsterCardHelper(), 1, IncreaseHealthHelper);
    }

    private void Option3()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetMonsterCardHelper(), 1, IncreaseBothHelper);
    }

    private List<Card> GetMonsterCardHelper()
    {
        List<Card> MonsterCards = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if (card is MonsterCard)
            {
                MonsterCards.Add(card);
            }
        }

        return MonsterCards;
    }

    private void IncreaseAttackHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            MonsterCard monsterCard = (MonsterCard)card;
            monsterCard.attackPower += 10;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }

    private void IncreaseHealthHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            MonsterCard monsterCard = (MonsterCard)card;
            monsterCard.healthPoint += 80;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }

    private void IncreaseBothHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            MonsterCard monsterCard = (MonsterCard)card;
            monsterCard.attackPower += 10;
            monsterCard.healthPoint += 80;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 获得基础单位事件
public class GainBaseUnitEvent : EventBehavior
{
    protected override string startSceneEventText { get; set; } = "";

    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Gain 2 8/60 base Unit",
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

    private void Option1()
    {
        MonsterCard newBaseUnit = (MonsterCard)Card.CloneCard(CardDataModel.Instance.GetCard(1));
        newBaseUnit.attackPower = 8;
        newBaseUnit.healthPoint = 60;

        CardDataModel.Instance.ObtainCard(newBaseUnit);
        CardDataModel.Instance.ObtainCard(newBaseUnit);

        LeaveEvent();
    }
}

// 基础单位减费事件
public class DecreaseCostForBase : EventBehavior
{
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "";
    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Reduce the cost of 1 of your base units by 1 point.",
        "40 Gold: Reduce the cost of 2 of your base units by 1 point.",
        "80 Gold: Reduce the cost of 3 of your base units by 1 point.",
        "150 Gold: Reduce the cost of all your base units by 1 point.",
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
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 1, DecreaseCostForBaseHelper);
    }

    private void Option2()
    {
        PlayerStatesManager.Instance.DecreaseGold(40);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 2, DecreaseCostForBaseHelper);
    }

    private void Option3()
    {
        PlayerStatesManager.Instance.DecreaseGold(80);
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(GetBaseUnitHelper(), 3, DecreaseCostForBaseHelper);
    }

    private void Option4()
    {
        PlayerStatesManager.Instance.DecreaseGold(150);
        DecreaseCostForBaseHelper(GetBaseUnitHelper());
    }

    private List<Card> GetBaseUnitHelper()
    {
        List<Card> baseUnits = new List<Card>();

        foreach (Card card in CardDataModel.Instance.GetPlayerDeck())
        {
            if ((card.color == CardColor.Base) && (card.cost >= 1))
            {
                baseUnits.Add(card);
            }
        }

        return baseUnits;
    }

    private void DecreaseCostForBaseHelper(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            card.cost -= 1;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}

// 基础单位加攻加血事件
public class IncreaseStatesForBase : EventBehavior
{
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "";
    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "1 of your base units gain 3 attack and 20 health.",
        "40 Gold: 2 of your base units gain 3 attack and 20 health.",
        "80 Gold: 3 of your base units gain 3 attack and 20 health.",
        "150 Gold: All of your base units gain 3 attack and 20 health.",
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
            monsterCard.attackPower += 3;
            monsterCard.healthPoint += 20;
        }

        SetUpLeaveEventScene(startSceneImageLocation, eventText2);
    }
}