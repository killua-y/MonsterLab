using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class EnemyBehavior : MonoBehaviour
{
    private List<MonsterCard> monsterList = new List<MonsterCard>();

    // 从0开始数
    public virtual int MaxTurn { get; set; } = 4;

    // 怪兽波次记录
    protected int index = 0;
    protected List<int> MonsterSummonTurn = new List<int>();

    // 引用的script
    private TurnManager turnManager;

    private void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    // 该敌人拥有的怪兽
    public virtual void LoadEnemy()
    {

    }

    public virtual List<int> GetTurnList()
    {
        return MonsterSummonTurn;
    }

    // 根据当前回合召唤怪兽
    public virtual void SummonEnemy()
    {
        index += 1;

        // 如果所有怪兽都召唤完成
        if (MonsterSummonTurn.Count == index)
        {
            // 告诉TurnManager这是最后一波
            turnManager.isFinalWaive = true;
        }
    }

    public static void SummonEnenmy(int rowIndex, int columnIndex, MonsterCard card)
    {
        int attackPower = card.attackPower;
        int healthPoint = card.healthPoint;

        SummonEnenmy(rowIndex, columnIndex, card, attackPower, healthPoint);
    }

    public static void SummonEnenmy(int rowIndex, int columnIndex, MonsterCard card, int attackPower, int healthPoint)
    {
        MonsterCard newCard = (MonsterCard)Card.CloneCard(card);
        newCard.attackPower = attackPower;
        newCard.healthPoint = healthPoint;
        BattleManager.Instance.InstaniateMontser(GridManager.Instance.GetFreeNode(rowIndex, columnIndex, false), Team.Enemy, newCard);
    }
}

[System.Serializable]
public class Enemy
{
    public string name;
    public int layer;
    public EnemyType enemyType;
    public bool easy;
    public string scriptLocation;

    public Enemy(string _name, int _layer, EnemyType _enemyType, string _scriptLocation, bool _easy)
    {
        this.name = _name;
        this.layer = _layer;
        this.enemyType = _enemyType;
        this.scriptLocation = _scriptLocation;
        this.easy = _easy;
    }
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}