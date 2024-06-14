using System;
using System.Collections.Generic;

public class GoldForDeleteOneCardEvent : EventBehavior
{
    public new int optionNumber = 2;

    public override List<string> optionsText { get; set; } = new List<string>()
    {
        "Remove one card from your deck",
        "Not what I want, leave"
    };

    public override List<string> eventText { get; set; } = new List<string>();
    public override string eventImageLocation { get; set; } = "";

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
    public override List<string> optionsText { get; set; } = new List<string>()
    {
        "Select One Card",
    };

    public override List<string> eventText { get; set; } = new List<string>()
    {
        "Select one card event"
    };

    public override string eventImageLocation { get; set; } = "";

    protected override void bindAction()
    {
        optionsAction = new List<Action>()
        {
            Option1,
        };
        optionNumber = 1;
    }

    private void Option1()
    {
        RewardManager.Instance.GenerateReward(1, 0);
        Leave();
    }
}

public class GainGoldEvent : EventBehavior
{
    public new int optionNumber = 1;

    public override List<string> optionsText { get; set; } = new List<string>()
    {
        "Gain 150 gold",
    };

    public override List<string> eventText { get; set; } = new List<string>()
    {
        "You find a dead body and while search on it"
    };
    public override string eventImageLocation { get; set; } = "";

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