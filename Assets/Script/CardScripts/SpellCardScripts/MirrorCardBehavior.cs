

public class MirrorCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        Card newCard = Card.CloneCard(targetMonster.cardModel);

        InGameStateManager.Instance.AddToHand(newCard);
    }
}
