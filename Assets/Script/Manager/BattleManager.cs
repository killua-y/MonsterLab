using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class BattleManager : Manager<BattleManager>
{
    public Transform playerParent;
    public Transform enemyParent;

    List<BaseEntity> playerEntities = new List<BaseEntity>();
    List<BaseEntity> enemyEntities = new List<BaseEntity>();

    public Action<BaseEntity> OnUnitDied;
    public Action<BaseEntity> OnUnitSummon;

    public TextMeshProUGUI monsterSpaceText;
    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattleTurnStart;
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
            // 根据Team安排不同的默认模型, 以后可以删除
            if (team == Team.Player)
            {
                modelPath = "MonsterPrefab/Slime";
            }
            else
            {
                modelPath = "Enemy/Slime/EnemySlime";
            }
        }

        GameObject monsterPrefab = Resources.Load<GameObject>(modelPath);

        // 加载怪兽script
        // 如果没有script地址就默认安排
        string scriptPath = "";
        if (monsterCard.scriptLocation != "")
        {
            scriptPath = monsterCard.scriptLocation;
        }
        else
        {
            scriptPath = "BaseEntity";
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
            OnUnitSummon?.Invoke(newEntity);
        }
        else
        {
            newMonster = Instantiate(monsterPrefab, enemyParent);
            newMonster.AddComponent(Type.GetType(scriptPath));
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            enemyEntities.Add(newEntity);
            newEntity.Setup(team, node, monsterCard, sacrifices);
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

    public void UnitDead(BaseEntity entity)
    {
        playerEntities.Remove(entity);
        enemyEntities.Remove(entity);

        OnUnitDied?.Invoke(entity);

        if (playerEntities.Count == 0)
        {
            Invoke("NewTurn", 2f);
        }
        else if (enemyEntities.Count == 0)
        {
            Invoke("NewTurn", 2f);
        }
    }

    // helper，用于延迟call一下新回合
    public void NewTurn()
    {
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

    public void OnBattleTurnStart()
    {
        if (playerEntities.Count == 0)
        {
            Invoke("NewTurn", 2f);
        }
        else if (enemyEntities.Count == 0)
        {
            Invoke("NewTurn", 2f);
        }
    }
}

public enum Team
{
    Player,
    Enemy
}
