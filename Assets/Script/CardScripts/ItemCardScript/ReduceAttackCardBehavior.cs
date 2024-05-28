

public class ReduceAttackCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster.cardModel.attackPower -= card.effectData;

        // 攻击力不会小于1
        if (targetMonster.cardModel.attackPower <= 0)
        {
            targetMonster.cardModel.attackPower = 1;
        }

        targetMonster.UpdateMonster();
    }
}
