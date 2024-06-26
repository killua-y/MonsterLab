using System;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOneCardEvent : EventBehavior
{
    // StartScene
    protected override List<string> optionsText { get; set; } = new List<string>()
    {
        "Remove one card from your deck",
        "Not what I want, leave"
    };
    protected override List<string> eventText { get; set; } = new List<string>()
    {
        "Would you like to try this hot spring that can make your steps lighter?",
        "You have left the hot spring."
    };
    protected override string eventImageLocation { get; set; } = "";

    // LeaveSene
    private List<string> optionsText2 = new List<string>()
    {
        "leave"
    };
    private string eventImageLocation2 { get; set; } = "";
    private List<Action> optionsAction2;

    protected override void bindAction()
    {
        optionsAction = new List<Action>()
        {
            Option1,
            Option2
        };

        optionsAction2 = new List<Action>()
        {
            Option2
        };
    }

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == optionsText[0])
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
        SetUpEventScene(eventImageLocation2, eventText[1], optionsText2, optionsAction2);
    }

    private void Option2()
    {
        CloseEvent();
        LeaveScene();
    }
}

public class SelectOneCardEvent : EventBehavior
{
    protected override List<string> optionsText { get; set; } = new List<string>()
    {
        "Select One Card",
    };

    protected override List<string> eventText { get; set; } = new List<string>()
    {
        "Select one card event"
    };

    protected override string eventImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        optionsAction = new List<Action>()
        {
            Option1,
        };
    }

    private void Option1()
    {
        RewardManager.Instance.GenerateCardReward(1);
        CloseEvent();
    }
}

public class GainGoldEvent : EventBehavior
{
    protected override List<string> optionsText { get; set; } = new List<string>()
    {
        "Gain 150 gold",
    };

    protected override List<string> eventText { get; set; } = new List<string>()
    {
        "You find a dead body and while search on it"
    };
    protected override string eventImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        optionsAction = new List<Action>()
        {
            Option1,
        };
    }

    private void Option1()
    {
        PlayerStatesManager.Instance.IncreaseGold(150);
        CloseEvent();
    }
}

public class DecreaseCostForBase : EventBehavior
{
    protected override List<string> optionsText { get; set; } = new List<string>()
    {
        "Reduce the cost of 1 of your basic units by 1 point.",
        "40 Gold: Reduce the cost of 2 of your basic units by 1 point."
    };

    protected override List<string> eventText { get; set; } = new List<string>();
    protected override string eventImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        optionsAction = new List<Action>()
        {
            Option1,
            Option2
        };
    }

    protected override bool CheckOptionValidity(string _optionText)
    {
        if (_optionText == optionsText[0])
        {
            return CardDataModel.Instance.GetPlayerDeck().Count > 0;
        }
        else if (_optionText == optionsText[1])
        {

        }

        return base.CheckOptionValidity();
    }

    private void Option1()
    {
        FindAnyObjectByType<CardSelectPanelBehavior>().SelectCardFromDeck(DeleteCardHelper);
    }

    private void DeleteCardHelper(Card card)
    {
        CardDataModel.Instance.DeleteCard(card);
        CloseEvent();
    }

    private void Option2()
    {
        CloseEvent();
        LeaveScene();
    }
}