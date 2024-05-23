using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class UponSummonFunction : MonoBehaviour
{
    public static List<System.Action<BaseEntity>> AllUponSummonFunctionsCalled = new List<System.Action<BaseEntity>>();
    public static List<int> AllEffectData = new List<int>();

    private static bool recordEnabled = true;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnGameStart()
    {
        AllUponSummonFunctionsCalled = new List<System.Action<BaseEntity>>();
        AllEffectData = new List<int>();
        Debug.Log("reset summon function");
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
            // 设置为通常怪兽
            newMonsterCard.scriptLocation = "";
            BattleManager.Instance.InstaniateMontser(node, entity.myTeam, newMonsterCard);
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
        List<BaseEntity> enemyList = new List<BaseEntity>();

        foreach (BaseEntity enemy in BattleManager.Instance.GetEntitiesAgainst(entity.myTeam))
        {
            enemyList.Add(enemy);
        }

        foreach (BaseEntity enemy in enemyList)
        {
            enemy.TakeDamage(entity.cardModel.effectData, null);
        }

        if (recordEnabled)
        {
            AllUponSummonFunctionsCalled.Add(FireWolfUponSummon);
            AllEffectData.Add(entity.cardModel.effectData);
        }
    }
}
