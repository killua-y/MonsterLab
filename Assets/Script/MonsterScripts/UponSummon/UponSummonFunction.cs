using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Card;

public class UponSummonFunction : MonoBehaviour
{
    public static List<System.Action<BaseEntity>> AllUponSummonFunctionsCalled = new List<System.Action<BaseEntity>>();
    public static List<int> AllEffectData = new List<int>();

    private static bool recordEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
    }

    private void OnCombatStart()
    {
        AllUponSummonFunctionsCalled = new List<System.Action<BaseEntity>>();
        AllEffectData = new List<int>();
    }

    // 重复所有战吼
    public static void RepeatAllSummonEffects(BaseEntity entity)
    {
        // Temporarily disable recording
        recordEnabled = false;

        for (int i = (AllUponSummonFunctionsCalled.Count - 1); i >= 0; i--)
        {
            entity.cardModel.effectData = AllEffectData[i];
            AllUponSummonFunctionsCalled[i](entity);
        }

        // Re-enable recording
        recordEnabled = true;
    }

    // 复制自己
    public static void ShadowWolfUponSummon(BaseEntity entity)
    {
        bool isPlayerTeam;

        if (entity.myTeam == Team.Player)
        {
            isPlayerTeam = true;
        }
        else
        {
            isPlayerTeam = false;
        }

        Node node = GridManager.Instance.GetFreeNode(entity.CurrentNode.rowIndex, entity.CurrentNode.columnIndex, isPlayerTeam);

        if (node != null)
        {
            MonsterCard newMonsterCard = (MonsterCard)Card.CloneCard(entity.cardModel);
            // 先设置为通常怪兽避免触发战吼
            newMonsterCard.scriptLocation = "none";
            BattleManager.Instance.InstaniateMontser(node, entity.myTeam, newMonsterCard);

            // 再设置回去
            newMonsterCard.scriptLocation = entity.cardModel.scriptLocation;
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(ShadowWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }


    // 抽牌
    public static void BabyWolfUponSummon(BaseEntity entity)
    {
        InGameStateManager.Instance.DrawCards(entity.cardModel.effectData);

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(BabyWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }


    // 对敌方最高血单位造成伤害,并施加流血
    public static void FireWolfUponSummon(BaseEntity entity)
    {
        List<BaseEntity> enemyList = BattleManager.Instance.GetEntitiesAgainst(entity.myTeam);

        // 寻找生命值最高的怪兽
        if (enemyList.Count != 0)
        {
            int heighestHealth = 0;
            BaseEntity highestHealthMonster = null;

            foreach (BaseEntity enemy in enemyList)
            {
                if (enemy.currentHealth > heighestHealth)
                {
                    highestHealthMonster = enemy;
                    heighestHealth = enemy.currentHealth;
                }
            }

            // 造成伤害
            highestHealthMonster.TakeDamage(entity.cardModel.effectData * 10, DamageType.MonsterSkill, entity);

            // 施加流血
            BleedingStack bleedingStack = highestHealthMonster.GetComponent<BleedingStack>();
            if (bleedingStack == null)
            {
                bleedingStack = highestHealthMonster.gameObject.AddComponent<BleedingStack>();
            }
            bleedingStack.IncreaseStack(entity.cardModel.effectData);
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(FireWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }

    // 增加全队攻击力
    public static void IncreaseAllAttackUponSummon(BaseEntity entity)
    {
        List<BaseEntity> AllyList = BattleManager.Instance.GetMyTeamEntities(entity.myTeam);

        foreach (BaseEntity baseEntity in AllyList)
        {
            baseEntity.cardModel.attackPower += entity.cardModel.effectData;
            baseEntity.UpdateMonster();
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(IncreaseAllAttackUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }

    // 获得随机0费item卡
    public static void Gain0CostItemCardUponSummon(BaseEntity entity)
    {
        List<Card> cards = CardDataModel.Instance.GetPlayerDeck();
        bool containsItem = cards.Any(Card => Card is ItemCard);
        if (containsItem == true)
        {
            List<Card> itemCards = new List<Card>();
            foreach (Card card in cards)
            {
                if (card is ItemCard)
                {
                    itemCards.Add(card);
                }
            }
            Card itemCard = Card.CloneCard(HelperFunction.GetRandomItem(itemCards, GameSetting.CurrentActRand));
            InGameStateManager.Instance.AddToHand(itemCard);
        }
        else
        {
            Debug.Log("Player does not have item card");
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(Gain0CostItemCardUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }

    // 根据场上和卡组的base unit 增加攻击力
    public static void KingWolfUponSummon(BaseEntity entity)
    {
        List<MonsterCard> BaseUnitList = new List<MonsterCard>();

        // 获取场上的base unit
        foreach (BaseEntity baseEntity in BattleManager.Instance.GetMyTeamEntities(entity.myTeam))
        {
            if (baseEntity.cardModel.color == CardColor.Base)
            {
                BaseUnitList.Add(baseEntity.cardModel);
            }
        }

        // 获取玩家手牌，额外卡组，抽牌堆，弃牌堆里的所有base unit
        InGameCardModel inGameCardModel = FindAnyObjectByType<InGameCardModel>();
        List<Card> allPlayerCards = new List<Card>();
        allPlayerCards.AddRange(inGameCardModel.GetHandCard());
        allPlayerCards.AddRange(inGameCardModel.GetExtraDeckPileCard());
        allPlayerCards.AddRange(inGameCardModel.GetDrawPileCard());
        allPlayerCards.AddRange(inGameCardModel.GetDiscardPileCard());

        foreach (Card card in allPlayerCards)
        {
            if (card.color == CardColor.Base)
            {
                BaseUnitList.Add((MonsterCard) card);
            }
        }

        // 根据取得的所有BaseUnit来增加属性
        foreach (MonsterCard baseUnit in BaseUnitList)
        {
            entity.cardModel.attackPower += baseUnit.attackPower;
            entity.cardModel.healthPoint += baseUnit.healthPoint;
        }

        // 更新怪物属性
        entity.RestoreAllHealth();
        entity.UpdateMonster();

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(KingWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }
}
