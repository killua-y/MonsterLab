using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class BaseEntity : MonoBehaviour
{
    // 怪兽UI部分
    private MonsterUI monsterUI;

    // 怪兽属性部分
    private GameObject bullet;
    protected int currentHealth;
    [Range(1.5f, 10)]
    protected float range = 1.5f;
    public float attackSpeed = 1f; //攻击间隔
    public float attackPreparation = 0.5f; //攻击前摇
    public float attackRecover = 0.5f; //攻击后摇
    protected float movementSpeed = 1.3f; //移动速度

    // 储存了怪兽所有数据的卡牌信息
    public MonsterCard cardModel;

    // 自走棋部分
    public Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;
    public Node BattleStartNode;
    public Node CurrentNode => currentNode;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    protected bool moving;
    protected Node destination;

    //其他
    protected bool dead = false;
    protected bool canAttack = true;
    protected bool CanBattle = false;
    protected float waitBetweenAttack;
    public Action OnDeath;

    public virtual void Setup(Team team, Node node, MonsterCard monsterCard, List<BaseEntity> sacrifices = null)
    {
        // 加载怪兽UI script
        monsterUI = this.gameObject.GetComponent<MonsterUI>();

        myTeam = team;

        if (myTeam == Team.Enemy)
        {
            monsterUI.EnemyMonster();
        }

        this.currentNode = node;
        transform.position = node.worldPosition;
        node.SetOccupied(true);
        node.currentEntity = this;

        // 导入MonsterCard, 并更新UI
        UpdateMonster(monsterCard);

        // 消耗祭品
        if (sacrifices != null)
        {
            Consume(sacrifices);
        }

        // 把当前生命值设置为最大
        currentHealth = cardModel.healthPoint;

        // 战吼
        UponSummon();

        // 以后记得改
        range = monsterCard.attackRange;

        // 加载弹道prefab
        bullet = this.gameObject.GetComponent<MonsterUI>().bullet;

        // 如果是远程单位但并没有子弹，则会添加默认子弹
        if ((range > 1.5) && (bullet == null))
        {
            bullet = Resources.Load<GameObject>("MonsterPrefab/bullet");
        }

        // 重新施加记录下的装备
        if (monsterCard.equipedCard.Count > 0)
        {
            Debug.Log("Start recast equipedCard");
            foreach (Card card in monsterCard.equipedCard)
            {
                System.Type scriptType = System.Type.GetType(card.scriptLocation);

                if (scriptType != null)
                {
                    // Use an existing GameObject or create a new one
                    GameObject tempObject = new GameObject("TempCardBehaviorObject");

                    // Add the component to the GameObject
                    MonoBehaviour scriptInstance = tempObject.AddComponent(scriptType) as MonoBehaviour;

                    // Ensure the script instance implements ICardBehavior
                    if (scriptInstance is CardBehavior cardBehavior)
                    {
                        cardBehavior.card = card;
                        // Call the CastCard method
                        cardBehavior.CastCard(currentNode);
                    }
                    else
                    {
                        Debug.LogWarning($"The script type {scriptType} does not implement CardBehavior.");
                    }

                    // Optionally, destroy the temporary object if it's no longer needed
                    Destroy(tempObject);
                }
                else
                {
                    Debug.LogError($"The script type '{card.scriptLocation}' could not be found.");
                }
            }
        }

        // 最后再update一次UI
        UpdateMonster();
    }
    
    protected void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;

        IndividualStart();
    }

    protected virtual void IndividualStart()
    {

    }

    public virtual void Update()
    {
        if (CanBattle)
        {
            if (!HasEnemy)
            {
                FindTarget();
            }

            if (IsInRange && !moving)
            {
                //In range for attack!
                if (canAttack)
                {
                    Attack();
                }
            }
            else
            {
                GetInRange();
            }
        }
    }

    // 寻找距离最近的敌人
    protected void FindTarget()
    {
        var allEnemies = BattleManager.Instance.GetEntitiesAgainst(myTeam);
        float minDistance = Mathf.Infinity;
        BaseEntity entity = null;
        foreach (BaseEntity e in allEnemies)
        {
            if (Vector3.Distance(e.transform.position, this.transform.position) <= minDistance)
            {
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                entity = e;
            }
        }

        currentTarget = entity;
    }

    protected bool MoveTowards(Node nextNode)
    {
        Vector3 direction = (nextNode.worldPosition - this.transform.position);
        if (direction.sqrMagnitude <= 0.005f)
        {
            transform.position = nextNode.worldPosition;
            return true;
        }

        this.transform.position += direction.normalized * movementSpeed * Time.deltaTime;
        return false;
    }

    protected void GetInRange()
    {
        if (currentTarget == null)
            return;

        // 说明已经到达新的节点，需要寻找下一个路径
        if (!moving)
        {
            //重制target
            FindTarget();
            if(currentTarget == null)
            {
                return;
            }

            destination = null;
            List<Node> candidates = GridManager.Instance.GetNodesCloseTo(currentTarget.CurrentNode);
            candidates = candidates.OrderBy(x => Vector2.Distance(x.worldPosition, this.transform.position)).ToList();
            for (int i = 0; i < candidates.Count; i++)
            {
                if (!candidates[i].IsOccupied)
                {
                    destination = candidates[i];
                    break;
                }
            }
            if (destination == null)
                return;

            var path = GridManager.Instance.GetPath(currentNode, destination);
            if (path == null || path.Count == 1)
                return;

            if (path[1].IsOccupied)
                return;

            StandUp();
            SitDown(path[1]);
        }

        moving = !MoveTowards(currentNode);
    }

    public void StandUp()
    {
        // 当前格子设置不占用，同时SetOccupied会把自己格子内的怪兽设为null
        currentNode.SetOccupied(false);
    }

    // 把currentnode设置为传入的node
    public void SitDown(Node node)
    {
        currentNode = node;
        currentNode.SetOccupied(true);
        currentNode.currentEntity = this;
    }

    // 更新怪兽的属性，不要在战斗中call
    public void UpdateMonster(MonsterCard card = null)
    {
        if (card != null)
        {
            cardModel = card;
        }

        currentHealth = cardModel.healthPoint;
        monsterUI.UpdateUI(cardModel);
    }

    // 准备阶段开始
    protected virtual void OnPreparePhaseStart()
    {
        this.gameObject.SetActive(true);

        CanBattle = false;

        canAttack = true;

        SitDown(BattleStartNode);
        transform.position = BattleStartNode.worldPosition;

        // 玩家的怪兽会回复全部生命值
        if (myTeam == Team.Player)
        {
            currentHealth = cardModel.healthPoint;
            monsterUI.UpdateHealth(currentHealth);

            // 复活，重新把自己添加回索敌list
            if (dead)
            {
                // 这一行是因为如果在攻击过程中死亡不会重制canAttack计数器
                canAttack = true;
                dead = false;
                BattleManager.Instance.AddToTeam(this);
            }
        }
    }

    // 准备阶段结束
    protected virtual void OnPreparePhaseEnd()
    {

    }

    // 战斗阶段开始
    protected virtual void OnBattlePhaseStart()
    {
        CanBattle = true;
        BattleStartNode = currentNode;
    }

    // 战斗阶段结束
    protected virtual void OnBattlePhaseEnd()
    {
        currentTarget = null;

        if (currentNode != null)
        {
            StandUp();
        }
    }

    public void TakeDamage(int amount, BaseEntity from = null)
    {
        if(dead)
        {
            return;
        }

        currentHealth -= amount;
        monsterUI.UpdateHealth(currentHealth);

        // 死亡
        if (currentHealth <= 0 && !dead)
        {
            UnitDie(from);
        }
    }

    public void UnitDie(BaseEntity from = null, bool isSacrifice = false)
    {
        dead = true;
        StandUp();

        if (myTeam == Team.Enemy)
        {
            InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
            InGameStateManager.Instance.OnPreparePhaseEnd -= OnPreparePhaseEnd;
            InGameStateManager.Instance.OnBattlePhaseStart -= OnBattlePhaseStart;
            InGameStateManager.Instance.OnBattlePhaseEnd -= OnBattlePhaseEnd;
        }

        // 亡语效果
        if (!isSacrifice)
        {
            UponDeath();
        }

        // 把自己从索敌list里移除
        BattleManager.Instance.UnitDead(this);

        // 敌方怪兽会被destory，我放怪兽会被设置为disactive
        if (myTeam == Team.Enemy)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // 如果是献祭就直接删除gameobject
            // 反之说明是战斗中死亡
            if (!isSacrifice)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
                InGameStateManager.Instance.OnPreparePhaseEnd -= OnPreparePhaseEnd;
                InGameStateManager.Instance.OnBattlePhaseStart -= OnBattlePhaseStart;
                InGameStateManager.Instance.OnBattlePhaseEnd -= OnBattlePhaseEnd;
                Destroy(this.gameObject);
            }
        }
    }

    protected virtual void Attack()
    {
        if (!canAttack)
            return;

        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        if (currentTarget.dead)
        {
            FindTarget();
            yield break;
        }

        canAttack = false;

        // 开始播放攻击动画
        yield return new WaitForSeconds(attackPreparation);

        // 攻击击中敌方
        if (bullet == null)
        {
            if (currentTarget == null)
            {
                yield break;
            }
            currentTarget.TakeDamage(cardModel.attackPower, this);
        }
        else
        {
            // 远程单位，构建攻击子弹
            // Instantiate and initialize the bullet
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            if (bulletInstance.GetComponent<Bullet>() == null)
            {
                bulletInstance.AddComponent<Bullet>().Initialize(currentTarget, cardModel.attackPower, this);
            }
            else
            {
                bulletInstance.GetComponent<Bullet>().Initialize(currentTarget, cardModel.attackPower, this);
            }
        }
        // 播放攻击后摇
        yield return new WaitForSeconds(attackRecover);
        canAttack = true;
    }

    protected virtual void Consume(List<BaseEntity> sacrfices)
    {
        foreach(BaseEntity sacrfice in sacrfices)
        {
            sacrfice.UnitDie(null, true);
        }
    }

    public virtual void UponSummon()
    {

    }

    public virtual void UponDeath()
    {
        OnDeath?.Invoke();
    }
}
