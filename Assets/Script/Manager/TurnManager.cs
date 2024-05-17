using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class TurnManager : Manager<TurnManager>
{
    // 卡牌管理区
    public GameObject cardDataManager;
    private CardDataModel cardDataModel;

    private int currentTurn = 1;
    //private int maxTurn = 5;

    private List<MonsterCard> monsterList = new List<MonsterCard>();
    // Start is called before the first frame update
    void Start()
    {
        cardDataModel = cardDataManager.GetComponent<CardDataModel>();
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;

        LoadEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadEnemy()
    {
        monsterList.Add(cardDataModel.GetEnemyMonster(0));
        monsterList.Add(cardDataModel.GetEnemyMonster(1));
        monsterList.Add(cardDataModel.GetEnemyMonster(2));
        monsterList.Add(cardDataModel.GetEnemyMonster(3));
    }

    public void OnPreparePhaseStart()
    {
        SummonEnemyThisTurn(currentTurn);
    }

    public void OnPreparePhaseEnd()
    {
        // 回合数+1
        currentTurn += 1;
    }

    // 根据当前回合召唤怪兽
    private void SummonEnemyThisTurn(int currentTurn)
    {
        if (currentTurn == 1)
        {
            BattleManager.Instance.InstaniateMontser(2, 4, Team.Enemy, monsterList[2]);
            BattleManager.Instance.InstaniateMontser(0, 5, Team.Enemy, monsterList[0]);
            BattleManager.Instance.InstaniateMontser(1, 5, Team.Enemy, monsterList[0]);
            BattleManager.Instance.InstaniateMontser(3, 7, Team.Enemy, monsterList[1]);
            BattleManager.Instance.InstaniateMontser(4, 4, Team.Enemy, monsterList[3]);
        }
    }

}
