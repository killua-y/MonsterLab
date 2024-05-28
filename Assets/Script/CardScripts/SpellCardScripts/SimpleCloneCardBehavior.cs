using static Card;

public class SimpleCloneCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        MonsterCard newCard = (MonsterCard) Card.CloneCard(targetMonster.cardModel);
        newCard.attackPower = 1;
        newCard.healthPoint = 1;

        InGameStateManager.Instance.AddToHand(newCard);
    }
}
