using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class BattleManager : Singleton<BattleManager>
{
    public Transform playerParent;
    public Transform enemyParent;

    List<BaseEntity> playerEntities = new List<BaseEntity>();
    List<BaseEntity> enemyEntities = new List<BaseEntity>();

    public Action<BaseEntity> OnUnitDied;
    public Action<BaseEntity> OnUnitSummon;
    public Action<int, DamageType, BaseEntity, BaseEntity> OnUnitTakingDamage;
    // 战斗结束前结算效果
    public Action BeforeBattlePhase;

    public TextMeshProUGUI monsterSpaceText;
    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnCombatEnd += OnCombatEnd;
    }

    public int ConvertRowColumnToIndex(int row, int column)
    {
        int index = row * 8 + column;
        return index;
    }

    public void InstaniateMontser(Node node, Team team, MonsterCard monsterCard, List<BaseEntity> sacrifices = null)
    {
        // 查看场上是否满了，如果满了直接return
        if (playerEntities.Count >= PlayerStatesManager.maxUnit)
        {
            // 如果需要祭品可以执行，反之不能
            if (sacrifices == null)
            {
                return;
            }
        }

        // 必须传入monsterCard
        if (monsterCard is null)
        {
            return;
        }

        // 生成怪兽模型
        // 如果没有模型地址就默认安排一个
        string modelPath = "";
        if (monsterCard.modelLocation != "")
        {
            modelPath = monsterCard.modelLocation;
        }
        else
        {
            if (team == Team.Player)
            {
                modelPath = "MonsterPrefab/NormalWolf";
            }
            else
            {
                modelPath = "MonsterPrefab/Slime/GreenSlime";
            }
        }

        GameObject monsterPrefab = Resources.Load<GameObject>(modelPath);

        // 加载怪兽script
        // 如果没有script地址就默认安排
        string scriptPath = "";
        if ((monsterCard.scriptLocation == "") || (monsterCard.scriptLocation == "none"))
        {
            scriptPath = "BaseEntity";
        }
        else
        {
            scriptPath = monsterCard.scriptLocation;
        }

        GameObject newMonster;

        // 根据team生成怪物
        if (team == Team.Player)
        {
            newMonster = Instantiate(monsterPrefab, playerParent);
            newMonster.AddComponent(Type.GetType(scriptPath));
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            playerEntities.Add(newEntity);
            newEntity.Setup(team, node, monsterCard, sacrifices);
            EffectManager.Instance.PlayEffect("BlueSummonEffect", node.worldPosition);
            OnUnitSummon?.Invoke(newEntity);
        }
        else
        {
            newMonster = Instantiate(monsterPrefab, enemyParent);
            newMonster.AddComponent(Type.GetType(scriptPath));
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            if (newEntity == null)
            {
                Debug.Log("cannot get enemy : " + scriptPath);
            }
            enemyEntities.Add(newEntity);
            newEntity.Setup(team, node, monsterCard, sacrifices);
            EffectManager.Instance.PlayEffect("RedSummonEffect", node.worldPosition);
            OnUnitSummon?.Invoke(newEntity);
        }
    }

    public void InstaniateMontser(int index, Team team, MonsterCard monsterCard, List<BaseEntity> sacrifices = null)
    {
        InstaniateMontser(GridManager.Instance.GetNodeForIndex(index), team, monsterCard, sacrifices);
    }

    public void InstaniateMontser(int row, int column, Team team, MonsterCard monsterCard, List<BaseEntity> sacrifices = null)
    {
        InstaniateMontser(GridManager.Instance.GetNodeForRowAndColumn(row, column), team, monsterCard, sacrifices);
    }

    public List<BaseEntity> GetEntitiesAgainst(Team against)
    {
        if (against == Team.Player)
            return enemyEntities;
        else
            return playerEntities;
    }

    public List<BaseEntity> GetMyTeamEntities(Team myteam)
    {
        if (myteam == Team.Player)
            return playerEntities;
        else
            return enemyEntities;
    }

    public void UnitDead(BaseEntity entity)
    {
        playerEntities.Remove(entity);
        enemyEntities.Remove(entity);

        OnUnitDied?.Invoke(entity);

        if ((playerEntities.Count == 0) || (enemyEntities.Count == 0))
        {
            StartCoroutine(NewTurn());
        }
    }

    public void UnitTakingDamage(int amount, DamageType damageType, BaseEntity from, BaseEntity to)
    {
        OnUnitTakingDamage?.Invoke(amount, damageType, from, to);
    }

    // helper，用于延迟call一下新回合
    IEnumerator NewTurn()
    {
        yield return new WaitForSeconds(0.5f);
        BeforeBattlePhase?.Invoke();

        yield return new WaitForSeconds(1.5f);
        if (InGameStateManager.BattelPhase)
        {
            InGameStateManager.Instance.BattlePhaseEnd();
        }
    }

    public void AddToTeam(BaseEntity entity)
    {
        if(entity.myTeam == Team.Player)
        {
            playerEntities.Add(entity);
        }
        else
        {
            Debug.Log("Enemy monster should not call this function");
        }
    }    

    public void DisplayMonsterSpaceText()
    {
        UpdateMonsterSpaceText();
        monsterSpaceText.gameObject.SetActive(true);
    }

    public void StopDisplayMonsterSpaceText()
    {
        monsterSpaceText.gameObject.SetActive(false);
    }

    public void UpdateMonsterSpaceText()
    {
        monsterSpaceText.text = playerEntities.Count + " / " + PlayerStatesManager.maxUnit;
    }

    public void OnBattlePhaseStart()
    {
        if ((playerEntities.Count == 0) || (enemyEntities.Count == 0))
        {
            StartCoroutine(NewTurn());
        }
    }

    public void OnCombatEnd()
    {
        foreach (Transform child in playerParent)
        {
            BaseEntity entity = child.GetComponent<BaseEntity>();
            playerEntities.Remove(entity);
            enemyEntities.Remove(entity);
            Destroy(entity.gameObject);
        }

        foreach (Transform child in enemyParent)
        {
            BaseEntity entity = child.GetComponent<BaseEntity>();
            playerEntities.Remove(entity);
            enemyEntities.Remove(entity);
            Destroy(entity.gameObject);
        }
    }
}

public enum Team
{
    Player,
    Enemy
}
