using System;
using System.Collections.Generic;
using UnityEngine;

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

// 基础单位减费事件
public class DecreaseCostForBase : EventBehavior
{
    protected override string startSceneImageLocation { get; set; } = "";
    protected override string startSceneEventText { get; set; } = "";
    protected override List<string> startSceneOptionsText { get; set; } = new List<string>()
    {
        "Reduce the cost of 1 of your basic units by 1 point.",
        "40 Gold: Reduce the cost of 2 of your basic units by 1 point.",
        "80 Gold: Reduce the cost of 3 of your basic units by 1 point.",
        "Leave"
    };

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