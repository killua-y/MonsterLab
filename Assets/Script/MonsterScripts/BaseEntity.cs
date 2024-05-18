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
    public SpriteRenderer spriteRender;
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;

    // 怪兽属性部分
    public GameObject bullet;
    protected int currentHealth;
    [Range(1.5f, 10)]
    protected float range = 1.5f;
    public float attackSpeed = 1f; //攻击间隔
    public float attackPreparation = 0.5f; //攻击前摇
    public float attackRecover = 0.5f; //攻击后摇
    protected float movementSpeed = 1.3f; //移动速度

    // 储存了怪兽所有数据的卡牌信息
    public MonsterCard cardModel;

    public Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;
    public Node BattleStartNode;

    public Node CurrentNode => currentNode;

    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    protected bool moving;
    protected Node destination;

    protected bool dead = false;
    protected bool canAttack = true;
    protected bool CanBattle = false;
    protected float waitBetweenAttack;

    public virtual void Setup(Team team, Node node, MonsterCard monsterCard, List<BaseEntity> sacrifice = null)
    {
        myTeam = team;
        if (myTeam == Team.Enemy)
        {
            spriteRender.flipX = true;
            fillImage = healthBar.fillRect.GetComponent<Image>();
            fillImage.color = Color.red;
        }

        // 导入MonsterCard
        UpdateMonster(monsterCard);

        this.currentNode = node;
        transform.position = node.worldPosition;
        node.SetOccupied(true);
        node.currentEntity = this;

        // 战吼
        UponSummon();

        // 属性UI设置，最后call
        UpdateUI();

        // 以后记得改
        range = monsterCard.attackRange;
    }
    
    protected void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;
        InGameStateManager.Instance.OnBattlePhaseStart += OnBattlePhaseStart;
        InGameStateManager.Instance.OnBattlePhaseEnd += OnBattlePhaseEnd;
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

        if (!moving)
        {
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

            // 将目标节点设为占领
            path[1].SetOccupied(true);
            path[1].currentEntity = this;
            destination = path[1];

            // 将之前节点设为不再占领
            currentNode.SetOccupied(false);
            currentNode.currentEntity = null;

            SetCurrentNode(destination);
        }

        moving = !MoveTowards(destination);
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    // 更新怪兽的UI属性，不要在战斗中call
    public void UpdateUI()
    {
        currentHealth = cardModel.healthPoint;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
        attackText.text = cardModel.attackPower + "";
        healthText.text = currentHealth + "";
    }

    // 更新怪兽的属性，不要在战斗中call
    public void UpdateMonster(MonsterCard card = null)
    {
        if (card != null)
        {
            cardModel = card;
        }

        UpdateUI();
    }

    // 准备阶段开始
    public virtual void OnPreparePhaseStart()
    {
        this.gameObject.SetActive(true);

        CanBattle = false;

        SetCurrentNode(BattleStartNode);
        BattleStartNode.SetOccupied(true);
        BattleStartNode.currentEntity = this;
        transform.position = BattleStartNode.worldPosition;

        // 玩家的怪兽会回复全部生命值
        if (myTeam == Team.Player)
        {
            UpdateUI();

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
    public virtual void OnPreparePhaseEnd()
    {

    }

    // 战斗阶段开始
    public virtual void OnBattlePhaseStart()
    {
        CanBattle = true;
        BattleStartNode = currentNode;
    }

    // 战斗阶段结束
    public virtual void OnBattlePhaseEnd()
    {
        currentTarget = null;

        if (currentNode != null)
        {
            currentNode.SetOccupied(false);
            currentNode.currentEntity = null;
        }
    }

    public void TakeDamage(int amount, BaseEntity from = null)
    {
        if(dead)
        {
            Debug.Log("Should not take damage");
            return;
        }

        currentHealth -= amount;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "";

        // 死亡
        if (currentHealth <= 0 && !dead)
        {
            UnitDie(from);
        }
    }

    public void UnitDie(BaseEntity from = null, bool activeUponDeath = true)
    {
        dead = true;
        currentNode.SetOccupied(false);
        currentNode.currentEntity = null;

        if (myTeam == Team.Enemy)
        {
            InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
            InGameStateManager.Instance.OnPreparePhaseEnd -= OnPreparePhaseEnd;
            InGameStateManager.Instance.OnBattlePhaseStart -= OnBattlePhaseStart;
            InGameStateManager.Instance.OnBattlePhaseEnd -= OnBattlePhaseEnd;
        }

        // 亡语效果
        if (activeUponDeath)
        {
            UponDeath();
        }

        BattleManager.Instance.UnitDead(this);
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
        if(bullet == null)
        {
            if(currentTarget == null)
            {
                yield break;
            }
            currentTarget.TakeDamage(cardModel.attackPower, this);
        }
        else
        {
            // 远程单位，构建攻击子弹
        }
        // 播放攻击后摇
        yield return new WaitForSeconds(attackRecover);
        canAttack = true;
    }

    public virtual void UponSummon()
    {

    }

    public virtual void UponDeath()
    {

    }
}
