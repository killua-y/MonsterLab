using static Card;

public class TemperedArcherEnemy : EnemyBehavior
{
    public override int MaxTurn { get; set; } = 4;

    // 该敌人拥有的怪兽
    private MonsterCard TemperedArcher;
    private MonsterCard TemperedSlime;

    public override void LoadEnemy()
    {
        TemperedArcher = CardDataModel.Instance.GetEnemyCard(14);
        TemperedSlime = CardDataModel.Instance.GetEnemyCard(10);

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(3);
    }

    // 根据当前回合召唤怪兽
    public override void SummonEnemy()
    {
        if (index == 0)
        {
            SummonEnenmy(2, 7, TemperedArcher);
            SummonEnenmy(1, 5, TemperedSlime);
            SummonEnenmy(3, 5, TemperedSlime);
        }
        else if (index == 1)
        {
            SummonEnenmy(3, 5, TemperedSlime);
        }

        base.SummonEnemy();
    }
}