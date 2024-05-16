using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class BattleManager : Manager<BattleManager>
{
    public GameObject monsterPrefab;
    public GameObject EnemyMonsterPrefab;

    public Transform playerParent;
    public Transform enemyParent;

    List<BaseEntity> playerEntities = new List<BaseEntity>();
    List<BaseEntity> enemyEntities = new List<BaseEntity>();

    public TextMeshProUGUI monsterSpaceText;
    // Start is called before the first frame update
    void Start()
    {
        base.Awake();

        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(0, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(1, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(2, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(3, 3)).SetOccupied(true);
        //GridManager.Instance.GetNodeForIndex(ConvertRowColumnToIndex(4, 3)).SetOccupied(true);
        InstaniateMontser(0, 0, Team.Player);
        InstaniateMontser(0, 1, Team.Player);
        InstaniateMontser(1, 6, Team.Enemy, EnemyMonsterPrefab);
        InstaniateMontser(0, 7, Team.Enemy, EnemyMonsterPrefab);
        InstaniateMontser(2, 7, Team.Enemy, EnemyMonsterPrefab);
        InstaniateMontser(3, 7, Team.Enemy, EnemyMonsterPrefab);
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

    public void InstaniateMontser(int index, Team team, GameObject _monsterPrefab = null, MonsterCard monsterCard = null)
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
            newEntity.Setup(team, GridManager.Instance.GetNodeForIndex(index), monsterCard);
        }
        else
        {
            newMonster = Instantiate(_monsterPrefab, enemyParent);
            BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();
            enemyEntities.Add(newEntity);
            newEntity.Setup(team, GridManager.Instance.GetNodeForIndex(index), monsterCard);
        }
    }

    public void InstaniateMontser(int row, int column, Team team, GameObject _monsterPrefab = null, MonsterCard monsterCard = null)
    {
        int index = ConvertRowColumnToIndex(row, column);

        InstaniateMontser(index, team, _monsterPrefab, monsterCard);
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

        Destroy(entity.gameObject);

        if (playerEntities.Count == 0)
        {
            Invoke("NewTurn", 1f);
        }
        else if (enemyEntities.Count == 0)
        {
            Invoke("NewTurn", 1f);
        }
    }

    // helper，用于延迟call一下新回合
    public void NewTurn()
    {
        InGameStateManager.Instance.TurnStart();
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
