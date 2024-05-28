

public class BabyWolfEntity : BaseEntity
{
    // Upon Summon: Draw n card
    public override void UponSummon()
    {
        UponSummonFunction.BabyWolfUponSummon(this);
    }
}
