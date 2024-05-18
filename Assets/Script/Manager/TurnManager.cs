using System;
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

    public List<MonsterCard> monsterList = new List<MonsterCard>();

    private EnemyBehavior enemy;
    // Start is called before the first frame update
    void Start()
    {
        cardDataModel = cardDataManager.GetComponent<CardDataModel>();
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;

        // 加载所有怪兽数据
        monsterList = cardDataModel.GetEnemyMonster();

        // 加载当前战斗敌人
        LoadEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadEnemy()
    {
        this.gameObject.AddComponent(Type.GetType("EnemyBehavior"));
        enemy = this.GetComponent<EnemyBehavior>();

        // 让敌人加载自己拥有的怪兽
        enemy.LoadEnemy();
    }

    // 每个准备阶段开始都call一下enmy看看要不要召唤怪兽
    public void OnPreparePhaseStart()
    {
        enemy.SummonEnemyThisTurn(currentTurn);
    }

    public void OnPreparePhaseEnd()
    {
        // 回合数+1
        currentTurn += 1;
    }

}
