using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Card;

public class TurnManager : Singleton<TurnManager>
{
    // 卡牌管理区
    public GameObject turnParent;
    public GameObject singleTurnPrefab;

    // Turn管理
    public bool isFinalWaive;
    private int currentTurn;
    private int finalTurn;
    public List<MonsterCard> monsterList = new List<MonsterCard>();
    private List<int> MonsterSummonTurn;
    private List<TurnUnitBehavior> allTurns;

    private EnemyBehavior enemy;

    // Start is called before the first frame update
    void Start()
    {
        InGameStateManager.Instance.OnCombatStart += OnCombatStart;
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;

        // 加载所有怪兽数据
        monsterList = CardDataModel.Instance.GetEnemyMonster();
    }

    void OnCombatStart()
    {
        // 加载当前战斗敌人
        LoadEnemy(ActsManager.currentEnemy);
    }

    void PassTheBattle()
    {
        // 清除战斗的回合
        foreach (Transform child in turnParent.transform)
        {
            // Destroy each child GameObject
            Destroy(child.gameObject);
        }

        int remainningTurn = finalTurn - currentTurn;
        EnemyBehavior enemy = this.GetComponent<EnemyBehavior>();
        Destroy(enemy);
        InGameStateManager.Instance.CombatEnd(remainningTurn);
    }

    private void LoadEnemy(String enemyScriptLocatiom)
    {
        isFinalWaive = false;

        // 这一行会load当前战斗的敌人
        this.gameObject.AddComponent(Type.GetType(enemyScriptLocatiom));

        enemy = this.GetComponent<EnemyBehavior>();

        // 让敌人加载自己拥有的怪兽
        enemy.LoadEnemy();

        currentTurn = 0;
        finalTurn = enemy.GetMaxTurn();
        MonsterSummonTurn = new List<int>();
        MonsterSummonTurn = enemy.GetTurnList();
        SetUpTurnUI();
        UpdateTurnView();
    }

    private void SetUpTurnUI()
    {
        allTurns = new List<TurnUnitBehavior>();

        for (int i = 0; i <= finalTurn; i++)
        {
            GameObject newTurn = Instantiate(singleTurnPrefab, turnParent.transform);
            TurnUnitBehavior turnBehavior = newTurn.GetComponent<TurnUnitBehavior>();
            allTurns.Add(turnBehavior);
            turnBehavior.index = i;

            if (MonsterSummonTurn.Contains(i))
            {
                turnBehavior.SetToSummonMonster();
            }
            else
            {
                turnBehavior.turnType = TurnType.Rest;
            }
        }

        turnParent.GetComponent<TurnUnitHorizontalLayout>().SortAndPositionChildren();
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
                // 过关
                PassTheBattle();
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
