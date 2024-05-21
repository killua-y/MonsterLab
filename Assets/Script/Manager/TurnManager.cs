using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class TurnManager : Manager<TurnManager>
{
    // 卡牌管理区
    public Transform turnParent;
    public GameObject singleTurnPrefab;
    public TextMeshProUGUI turnText;
    private CardDataModel cardDataModel;

    private int currentTurn = 0;
    private int maxTurn = 0;
    private List<int> MonsterSummonTurn = new List<int>();
    public List<MonsterCard> monsterList = new List<MonsterCard>();
    private List<TurnUnitBehavior> allTurns = new List<TurnUnitBehavior>();

    private EnemyBehavior enemy;
    // Start is called before the first frame update
    void Start()
    {
        cardDataModel = FindObjectOfType<CardDataModel>();
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;

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
        // 这一行会load当前战斗的敌人
        this.gameObject.AddComponent(Type.GetType("EnemyBehavior"));

        enemy = this.GetComponent<EnemyBehavior>();

        // 让敌人加载自己拥有的怪兽
        enemy.LoadEnemy();

        maxTurn = enemy.GetMaxTurn();
        MonsterSummonTurn = enemy.GetTurnList();
        SetUpTurnUI();
        UpdateTurnView();
    }

    private void SetUpTurnUI()
    {
        for (int i = 0; i <= maxTurn; i++)
        {
            GameObject newTurn = Instantiate(singleTurnPrefab, turnParent);
            TurnUnitBehavior turnBehavior = newTurn.GetComponent<TurnUnitBehavior>();
            allTurns.Add(turnBehavior);
            turnBehavior.index = i;

            if(MonsterSummonTurn.Contains(i))
            {
                turnBehavior.SetToSummonMonster();
            }
        }
    }

    // 每个准备阶段开始都call一下enemy看看要不要召唤怪兽
    private void OnPreparePhaseStart()
    {
        enemy.SummonEnemyThisTurn(currentTurn);
    }

    private void OnBattlePhaseEnd()
    {
        NextTurn();
    }

    private void NextTurn()
    {
        // 回合数+1
        currentTurn += 1;

        UpdateTurnView();
    }

    private void UpdateTurnView()
    {
        turnText.text = "Turn: " + (currentTurn + 1);

        foreach (TurnUnitBehavior turnUnitBehavior in allTurns)
        {
            turnUnitBehavior.UpdateTurn(currentTurn);
        }
    }
}
