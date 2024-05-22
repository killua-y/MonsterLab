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

    public TextMeshProUGUI monsterSpaceText;
    // Start is called before the first frame update
    void Start()
    {
        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int ConvertRowColumnToIndex(int row, int column)
    {
        int index = row * 8 + column;
        return index;
    }

    public void InstaniateMontser(Node node, Team team, MonsterCard monsterCard, List<BaseEntity> sacrifices = null)
    {
        // 必须传入monsterCard
        if (monsterCard is null)
        {
            return;
        }

        // 生成怪兽模型
        // 如果没有模型地址就默认安排一个
        string path = "";
        if (monsterCard.modelLocation != "")
        {
            path = monsterCard.modelLocation;
        }
        else
        {
            // 根据Team安排不同的默认模型, 以后可以删除
            if (team == Team.Player)
            {
                path = "MonsterPrefab/Slime";
            }
            else
            {
                path = "Enemy/Slime/EnemySlime";
            }
        }

        GameObject monsterPrefab = Resources.Load<GameObject>(path);

        // 加载怪兽script
        // 如果没有script地址就默认安排
        string scriptPath = "";
        if (monsterCard.scriptLocation != "")
        {
            scriptPath = monsterCard.scriptLocation;
        }
        else
        {
            scriptPath = "NormalMonsterEntity";
        }

        GameObject newMonster;

        // 生成怪兽script
        // 不搞了，全部跟着模型走

        // 根据team生成怪物
        if (team == Team.Player)
        {
            newMonster = Instantiate(monsterPrefab, playerParent);
            newMonster.AddComponent(Type.GetType(scriptPath));
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            playerEntities.Add(newEntity);
            newEntity.Setup(team, node, monsterCard, sacrifices);
        }
        else
        {
            newMonster = Instantiate(monsterPrefab, enemyParent);
            newMonster.AddComponent(Type.GetType(scriptPath));
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            enemyEntities.Add(newEntity);
            newEntity.Setup(team, node, monsterCard, sacrifices);
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
        UpdateMonsterSpaceText();
        monsterSpaceText.gameObject.SetActive(false);
    }

    public void UpdateMonsterSpaceText()
    {
        monsterSpaceText.text = playerEntities.Count + "/5";
    }
}

public enum Team
{
    Player,
    Enemy
}
