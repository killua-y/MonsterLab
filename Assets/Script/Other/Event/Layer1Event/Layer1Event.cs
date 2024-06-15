using System;
using System.Collections.Generic;

public class GoldForDeleteOneCardEvent : EventBehavior
{
    protected override int optionNumber { get; set; } = 2;

    protected override List<string> optionsText { get; set; } = new List<string>()
    {
        "Remove one card from your deck",
        "Not what I want, leave"
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

    private void Option1()
    {
        PlayerStatesManager.Instance.DecreaseGold(100);
    }

    private void Option2()
    {
        Leave();
    }
}

public class SelectOneCardEvent : EventBehavior
{
    protected override int optionNumber { get; set; } = 1;

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
        RewardManager.Instance.GenerateReward(1, 0);
        Leave();
    }
}

public class GainGoldEvent : EventBehavior
{
    protected override int optionNumber { get; set; } = 1;

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
        Leave();
    }
}