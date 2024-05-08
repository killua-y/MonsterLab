using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Manager<BattleManager>
{
    public GameObject monsterPrefab;
    public GameObject EnemyMonsterPrefab;

    public Transform team1Parent;
    public Transform team2Parent;

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();

    // Start is called before the first frame update
    void Start()
    {
        base.Awake();

        InstaniateMontser(16);
        InstaniateMontser(8);
        InstaniateMontser(9);
        InstaniateMontser(10);
        InstaniateMontserForOtherTeam(7);
        InstaniateMontserForOtherTeam(15);
        InstaniateMontserForOtherTeam(14);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstaniateMontser(int index)
    {
        GameObject newMonster;
        newMonster = Instantiate(monsterPrefab, team1Parent);

        BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();

        team1Entities.Add(newEntity);

        newEntity.Setup(Team.Team1, GridManager.Instance.GetNodeForIndex(index));
    }

    public void InstaniateMontserForOtherTeam(int index)
    {
        GameObject newMonster;
        newMonster = Instantiate(EnemyMonsterPrefab, team2Parent);

        BaseEntity newEntity = newMonster.GetComponent<BaseEntity>();

        team2Entities.Add(newEntity);

        newEntity.Setup(Team.Team2, GridManager.Instance.GetNodeForIndex(index));
    }

    public List<BaseEntity> GetEntitiesAgainst(Team against)
    {
        if (against == Team.Team1)
            return team2Entities;
        else
            return team1Entities;
    }

    public void StartFight()
    {

    }
}

public enum Team
{
    Team1,
    Team2
}
