using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class EnemyBehavior : MonoBehaviour
{
    private List<MonsterCard> monsterList = new List<MonsterCard>();

    private int MaxTurn = 4;

    // 怪兽波次记录
    private int index = 0;
    private List<int> MonsterSummonTurn = new List<int>();

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

        // 该在哪几个回合召唤怪兽
        MonsterSummonTurn.Add(0);
        MonsterSummonTurn.Add(3);
    }

    public virtual List<int> GetTurnList()
    {
        return MonsterSummonTurn;
    }

    public virtual int GetMaxTurn()
    {
        return MaxTurn;
    }

    // 根据当前回合召唤怪兽
    public virtual void SummonEnemy()
    {
        if (index == 0)
        {
            BattleManager.Instance.InstaniateMontser(0, 5, Team.Enemy, normal);
            BattleManager.Instance.InstaniateMontser(1, 5, Team.Enemy, normal);
            BattleManager.Instance.InstaniateMontser(1, 7, Team.Enemy, ranged);
            BattleManager.Instance.InstaniateMontser(3, 7, Team.Enemy, ranged);
            BattleManager.Instance.InstaniateMontser(2, 4, Team.Enemy, tank);
            BattleManager.Instance.InstaniateMontser(4, 4, Team.Enemy, highAttack);
        }
        else if (index == 1)
        {
            BattleManager.Instance.InstaniateMontser(0, 7, Team.Enemy, ranged);

            // 最后一波
            TurnManager.Instance.isFinalWaive = true;
        }

        index += 1;
    }
}
