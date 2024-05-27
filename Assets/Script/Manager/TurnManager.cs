using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class TurnManager : Manager<TurnManager>
{
    // 卡牌管理区
    public GameObject turnParent;
    public GameObject singleTurnPrefab;
    public TextMeshProUGUI turnText;
    private CardDataModel cardDataModel;

    // Turn管理
    public bool isFinalWaive = false;
    private int currentTurn = 0;
    private int finalTurn = 0;
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

    private void LoadEnemy()
    {
        // 这一行会load当前战斗的敌人
        this.gameObject.AddComponent(Type.GetType("EnemyBehavior"));

        enemy = this.GetComponent<EnemyBehavior>();

        // 让敌人加载自己拥有的怪兽
        enemy.LoadEnemy();

        finalTurn = enemy.GetMaxTurn();
        MonsterSummonTurn = enemy.GetTurnList();
        SetUpTurnUI();
        UpdateTurnView();
    }

    private void SetUpTurnUI()
    {
        for (int i = 0; i <= finalTurn; i++)
        {
            GameObject newTurn = Instantiate(singleTurnPrefab, turnParent.transform);
            TurnUnitBehavior turnBehavior = newTurn.GetComponent<TurnUnitBehavior>();
            allTurns.Add(turnBehavior);
            turnBehavior.index = i;

            turnParent.GetComponent<TurnUnitHorizontalLayout>().SortAndPositionChildren();

            if (MonsterSummonTurn.Contains(i))
            {
                turnBehavior.SetToSummonMonster();
            }
            else
            {
                turnBehavior.turnType = TurnType.Rest;
            }
        }
    }

    // 每个准备阶段开始都call一下enemy看看要不要召唤怪兽
    private void OnPreparePhaseStart()
    {
        TurnUnitBehavior thisTurn = GetCurrentTurn(currentTurn);

        if (thisTurn != null)
        {
            if (thisTurn.turnType == TurnType.EnemySummon)
            {
                enemy.SummonEnemy();
            }
        }
    }

    private void OnBattlePhaseEnd()
    {
        NextTurn();
    }

    private void NextTurn()
    {
        List<BaseEntity> enemyEntity = BattleManager.Instance.GetEntitiesAgainst(Team.Player);

        // 说明玩家全歼了敌方
        if (enemyEntity.Count == 0)
        {
            if (isFinalWaive)
            {
                int remainningTurn = finalTurn - currentTurn;
                // 过关
                Debug.Log("You win!, remainning turn = " + remainningTurn);
            }
            else
            {
                // 提前歼灭当前波次
                int preventInfiniteIndex = currentTurn;

                while (preventInfiniteIndex < finalTurn)
                {
                    TurnUnitBehavior nextTurn = GetCurrentTurn(currentTurn + 1);

                    if (nextTurn != null)
                    {
                        if (nextTurn.turnType == TurnType.Rest)
                        {
                            AddToBotton(nextTurn);
                        }
                        else
                        {
                            // 打破循环
                            break;
                        }
                    }
                    preventInfiniteIndex += 1;
                }
            }
        }
        // 查看是否是最后一个回合
        else if (currentTurn == finalTurn)
        {
            // 玩家生命值扣除
            if (PlayerStatesManager.playerHealthPoint > 0)
            {
                PlayerStatesManager.Instance.DecreaseHealth(1);
            }
            else
            {
                Debug.Log("GameOver");
            }

            // 添加一个以玩家生命值为代价的额外回合
            GameObject newTurn = Instantiate(singleTurnPrefab, turnParent.transform);
            TurnUnitBehavior turnBehavior = newTurn.GetComponent<TurnUnitBehavior>();
            allTurns.Add(turnBehavior);
            turnBehavior.index = finalTurn + 1;
            finalTurn += 1;
            turnBehavior.turnType = TurnType.Rest;

            turnParent.GetComponent<TurnUnitHorizontalLayout>().SortAndPositionChildren();
        }

        // 回合数+1
        currentTurn += 1;

        UpdateTurnView();
    }

    private void AddToBotton(TurnUnitBehavior _turnUnitBehavior)
    {
        int thisTurnIndex = _turnUnitBehavior.index;
        _turnUnitBehavior.index = finalTurn;

        foreach (TurnUnitBehavior turnUnitBehavior in allTurns)
        {
            if (turnUnitBehavior.index > thisTurnIndex)
            {
                turnUnitBehavior.index -= 1;
            }
        }

        turnParent.GetComponent<TurnUnitHorizontalLayout>().SortAndPositionChildren();
    }

    private void UpdateTurnView()
    {
        turnText.text = "Turn: " + (currentTurn + 1);

        foreach (TurnUnitBehavior turnUnitBehavior in allTurns)
        {
            turnUnitBehavior.UpdateTurn(currentTurn);
        }
    }

    private TurnUnitBehavior GetCurrentTurn(int turnIndex)
    {
        foreach (TurnUnitBehavior turnUnitBehavior in allTurns)
        {
            if (turnUnitBehavior.index == turnIndex)
            {
                return turnUnitBehavior;
            }
        }
        return null;
    }
}
