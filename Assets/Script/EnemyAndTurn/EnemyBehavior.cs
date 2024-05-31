using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class EnemyBehavior : MonoBehaviour
{
    private List<MonsterCard> monsterList = new List<MonsterCard>();

    // 从0开始数
    protected int MaxTurn = 4;

    // 怪兽波次记录
    protected int index = 0;
    protected List<int> MonsterSummonTurn = new List<int>();

    // 该敌人拥有的怪兽

    public virtual void LoadEnemy()
    {

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
        index += 1;

        // 如果所有怪兽都召唤完成
        if (MonsterSummonTurn.Count == (index + 1))
        {
            // 告诉TurnManager这是最后一波
            TurnManager.Instance.isFinalWaive = true;
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

public class Enemy
{
    public string name;
    public int layer;
    public EnemyType enemyType;
    public string scriptLocation;

    public Enemy(string _name, int _layer, EnemyType _enemyType, string _scriptLocation)
    {
        this.name = _name;
        this.layer = _layer;
        this.enemyType = _enemyType;
        this.scriptLocation = _scriptLocation;
    }
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}