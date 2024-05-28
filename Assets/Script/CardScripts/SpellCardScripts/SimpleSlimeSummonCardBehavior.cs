using static Card;

public class SimpleSlimeSummonCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        MonsterCard Slime = (MonsterCard) Card.CloneCard(CardDataModel.Instance.GetCard(card.effectData));

        BattleManager.Instance.InstaniateMontser(node, Team.Player, Slime);
    }
}
