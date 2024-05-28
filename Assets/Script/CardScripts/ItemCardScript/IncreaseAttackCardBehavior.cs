

public class IncreaseAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower += card.effectData;

        targetMonster.UpdateMonster();
    }
}
