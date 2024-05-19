using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class EnemyBehavior : MonoBehaviour
{
    private List<MonsterCard> monsterList = new List<MonsterCard>();

    // 该敌人拥有的怪兽
    private MonsterCard normal;
    private MonsterCard ranged;
    private MonsterCard tank;
    private MonsterCard highAttack;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void LoadEnemy()
    {
        normal = TurnManager.Instance.monsterList[0];
        ranged = TurnManager.Instance.monsterList[1];
        tank = TurnManager.Instance.monsterList[2];
        highAttack = TurnManager.Instance.monsterList[3];
    }

    // 根据当前回合召唤怪兽
    public virtual void SummonEnemyThisTurn(int currentTurn)
    {
        if (currentTurn == 1)
        {
            BattleManager.Instance.InstaniateMontser(0, 5, Team.Enemy, normal);
            BattleManager.Instance.InstaniateMontser(1, 5, Team.Enemy, normal);
            BattleManager.Instance.InstaniateMontser(1, 7, Team.Enemy, ranged);
            BattleManager.Instance.InstaniateMontser(3, 7, Team.Enemy, ranged);
            BattleManager.Instance.InstaniateMontser(2, 4, Team.Enemy, tank);
            BattleManager.Instance.InstaniateMontser(4, 4, Team.Enemy, highAttack);
        }
        else if (currentTurn == 2)
        {
            BattleManager.Instance.InstaniateMontser(2, 7, Team.Enemy, ranged);
        }
    }
}
