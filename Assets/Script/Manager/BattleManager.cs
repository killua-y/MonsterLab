using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager>
{
    public GameObject monsterPrefab;
    public GameObject EnemyMonsterPrefab;

    public Transform playerParent;
    public Transform enemyParent;
    public Action<BaseEntity> OnUnitDied;

    List<BaseEntity> playerEntities = new List<BaseEntity>();
    List<BaseEntity> enemyEntities = new List<BaseEntity>();

    // Start is called before the first frame update
    void Start()
    {
        base.Awake();

        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(0, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(1, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(2, 3)).SetOccupied(true);
        InstaniateMontser(0, 0, Team.Player);
        InstaniateMontser(0, 1, Team.Player);
        InstaniateMontser(0, 2, Team.Player);
        InstaniateMontser(1, 0, Team.Player);
        InstaniateMontser(1, 1, Team.Player);
        InstaniateMontser(1, 2, Team.Player);
        InstaniateMontser(2, 0, Team.Player);
        InstaniateMontser(2, 1, Team.Player);
        InstaniateMontser(2, 2, Team.Player);
        InstaniateMontser(1, 6, Team.Enemy, EnemyMonsterPrefab);
        InstaniateMontser(0, 7, Team.Enemy, EnemyMonsterPrefab);
        InstaniateMontser(2, 7, Team.Enemy, EnemyMonsterPrefab);
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

    public void InstaniateMontser(int index, Team team, GameObject _monsterPrefab = null)
    {
        // 如果没有传入montser prefab就默认assign一个
        if (_monsterPrefab is null)
        {
            _monsterPrefab = monsterPrefab;
        }

        GameObject newMonster;

        // 根据team生成怪物
        if (team == Team.Player)
        {
            newMonster = Instantiate(_monsterPrefab, playerParent);
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            playerEntities.Add(newEntity);
            newEntity.Setup(team, GridManager.Instance.GetNodeForIndex(index));
        }
        else
        {
            newMonster = Instantiate(_monsterPrefab, enemyParent);
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            enemyEntities.Add(newEntity);
            newEntity.Setup(team, GridManager.Instance.GetNodeForIndex(index));
        }
    }

    public void InstaniateMontser(int row, int column, Team team, GameObject _monsterPrefab = null)
    {
        int index = ConvertRowColumnToIndex(row, column);

        InstaniateMontser(index, team, _monsterPrefab);
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

        Destroy(entity.gameObject);
    }
}

public enum Team
{
    Player,
    Enemy
}
