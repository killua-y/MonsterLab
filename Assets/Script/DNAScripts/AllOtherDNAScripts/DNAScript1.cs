using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

// red DNA

// 所有rank大于等于1的怪兽减少一费
public class BloodSacrificeDNABehavior : DNABehavior
{
    // private
    private InGameCardModel inGameCardModel;

    private void Awake()
    {
        inGameCardModel = FindAnyObjectByType<InGameCardModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
    }

    private void OnCombatStart()
    {
        List<MonsterCard> cards = new List<MonsterCard>();

        foreach (Card card in inGameCardModel.GetDrawPileCard())
        {
            if (card is MonsterCard)
            {
                cards.Add((MonsterCard)card);
            }
        }

        foreach (Card card in inGameCardModel.GetExtraDeckPileCard())
        {
            if (card is MonsterCard)
            {
                cards.Add((MonsterCard)card);
            }
        }

        foreach (MonsterCard card in cards)
        {
            if (card.rank >= 1)
            {
                card.cost -= 1;
            }
        }
    }
}

// 所有怪兽攻击力提高
public class AttackPowerDNABehavior : DNABehavior
{
    private void Start()
    {
        BattleManager.Instance.OnUnitSummon += OnSummon;
    }

    private void OnSummon(BaseEntity baseEntity)
    {
        if (baseEntity.myTeam == Team.Player)
        {
            baseEntity.cardModel.attackPower += DNAModel.effectData;
            baseEntity.UpdateMonster();
        }
    }
}

// 所有怪兽生命值提高
public class HealthPointDNABehavior : DNABehavior
{
    private void Start()
    {
        BattleManager.Instance.OnUnitSummon += OnSummon;
    }

    private void OnSummon(BaseEntity baseEntity)
    {
        if (baseEntity.myTeam == Team.Player)
        {
            baseEntity.cardModel.healthPoint += DNAModel.effectData;
            baseEntity.RestoreHealth(DNAModel.effectData);
        }
    }
}

// 增加最高能量
public class MaxEnergyDNABehavior : DNABehavior
{
    public static bool firstAcquire = true;

    public override void OnAcquire()
    {
        PlayerStatesManager.maxCost += DNAModel.effectData;

        firstAcquire = false;
    }
}

// 增加最大teamsize
public class TeamSizeDNABehavior : DNABehavior
{
    public override void OnAcquire()
    {
        PlayerStatesManager.maxUnit += DNAModel.effectData;
    }
}

// 使用spell抽卡
public class DrawCardIfCastSpellDNA : DNABehavior
{
    private int Counter = 0;
    private bool effectTriggered;
    // Start is called before the first frame update

    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnSpellCardPlayed += OnSpellCardPlayed;
    }

    void OnSpellCardPlayed(CardBehavior cardBehavior, BaseEntity targetMonster)
    {
        if (!effectTriggered)
        {
            Counter += 1;
            if (Counter >= 2)
            {
                DrawCard();
                effectTriggered = true;
            }
        }
    }

    void OnPreparePhaseStart()
    {
        effectTriggered = false;
        Counter = 0;
    }

    void DrawCard()
    {
        InGameStateManager.Instance.DrawCards(DNAModel.effectData);
    }
}

// 每回合召唤的第一只怪兽获得攻击力生命值提升
public class FirstMonsterBuffDNA : DNABehavior
{
    private bool isFirst = true;

    void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        BattleManager.Instance.OnUnitSummon += OnUnitSummon;
    }

    void OnUnitSummon(BaseEntity baseEntity)
    {
        if (baseEntity.myTeam == Team.Player)
        {
            if (isFirst)
            {
                baseEntity.cardModel.attackPower += DNAModel.effectData;
                baseEntity.cardModel.healthPoint += (DNAModel.effectData * 10);
                baseEntity.RestoreHealth((DNAModel.effectData * 10));
                baseEntity.UpdateMonster();
                isFirst = false;
            }
        }
    }

    void OnPreparePhaseStart()
    {
        isFirst = true;
    }
}


public class PiggyBankDNABehavior : MonoBehaviour
{

}

// 每场战斗第一个回合获得两点能量
public class CombatStartGainEnergy : DNABehavior
{
    private bool canActive = true;

    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
    }

    void OnCombatStart()
    {
        canActive = true;
    }

    void OnPreparePhaseStart()
    {
        Invoke("IncreaseCostHelper", 0);
    }

    void IncreaseCostHelper()
    {
        if (canActive)
        {
            FindAnyObjectByType<PlayerCostManager>().IncreaseCost(DNAModel.effectData);
            canActive = false;
        }
    }
}

// 每场战斗第一个回合抽3张牌
public class CombatStartDrawCard : DNABehavior
{
    private bool canActive = true;

    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
    }

    void OnCombatStart()
    {
        canActive = true;
    }

    void OnPreparePhaseStart()
    {
        if (canActive)
        {
            InGameStateManager.Instance.DrawCards(DNAModel.effectData);
            canActive = false;
        }
    }
}

// 流血伤害触发两次
public class BleedingTriggerTwice : DNABehavior
{
    public static bool canActiveTwice = false;

    void Start()
    {
        canActiveTwice = true;
    }
}

