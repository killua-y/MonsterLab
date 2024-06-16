using System.Collections;
using System.Collections.Generic;
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
        InGameStateManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnGameStart()
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
            newMonsterCard.scriptLocation = "";
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
        for (int i = 0; i < entity.cardModel.effectData; i++)
        {
            InGameStateManager.Instance.DrawOneCard();
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(BabyWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }


    // 对所有敌方造成伤害
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
            highestHealthMonster.TakeDamage(entity.cardModel.effectData, entity);
        }


        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(FireWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }

    // 获得能量
    public static void GainEnergyUponSummon(BaseEntity entity)
    {
        Debug.Log("get here");
        PlayerCostManager.Instance.IncreaseCost(entity.cardModel.effectData);
    }


}
