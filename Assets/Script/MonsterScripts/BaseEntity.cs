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
    public int attackPower = 5;
    public int healthPoint = 40;
    public int mana = 0;
    [Range(1.5f, 10)]
    public float range = 1.5f;
    public float attackSpeed = 1f; //攻击间隔
    public float attackPreparation = 0.5f; //攻击前摇
    public float attackRecover = 0.5f; //攻击后摇
    protected float movementSpeed = 1.3f; //移动速度

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

    // private
    private int currentHealth;
    private Coroutine attackCoroutine;

    public void Setup(Team team, Node currentNode = null, MonsterCard monsterCard = null)
    {
        myTeam = team;
        if (myTeam == Team.Enemy)
        {
            spriteRender.flipX = true;
            fillImage = healthBar.fillRect.GetComponent<Image>();
            fillImage.color = Color.red;
        }

        if (currentNode != null)
        {
            BattleStartNode = currentNode;
        }

        if (monsterCard != null)
        {
            cardModel = monsterCard;
            UpdateMonster(cardModel);
        }

        this.currentNode = BattleStartNode;
        transform.position = BattleStartNode.worldPosition;
        BattleStartNode.SetOccupied(true);
        BattleStartNode.currentEntity = this;

        // 属性UI设置
        UpdateUI();
    }

    protected void Start()
    {
        InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
        InGameStateManager.Instance.OnPreparePhaseEnd += OnPreparePhaseEnd;
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
        FindTarget();
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    // 将自己瞬间移动到指定的node
    public void MoveToNode(Node node)
    {
        if(currentNode != null)
        {
            currentNode.SetOccupied(false);
            currentNode.currentEntity = null;
        }
        SetCurrentNode(node);
        node.SetOccupied(true);
        node.currentEntity = this;
        transform.position = node.worldPosition;
    }

    // 更新怪兽的UI属性，不要在战斗中call
    public void UpdateUI()
    {
        currentHealth = healthPoint;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
        attackText.text = attackPower + "";
        healthText.text = currentHealth + "";
    }

    // 更新怪兽的属性，不要在战斗中call
    public void UpdateMonster(MonsterCard card = null)
    {
        if (card != null)
        {
            cardModel = card;
        }

        attackPower = cardModel.attackPower;
        healthPoint = cardModel.healthPoint;
        range = cardModel.attackRange;

        UpdateUI();
    }

    // 战斗结束，开始新的回合
    public void OnPreparePhaseStart()
    {
        CanBattle = false;
        MoveToNode(BattleStartNode);

        // 玩家怪兽会回复全部生命值
        if(myTeam == Team.Player)
        {
            UpdateUI();
        }
    }

    // 战斗开始，记录开始战斗时的node
    public void OnPreparePhaseEnd()
    {
        CanBattle = true;
        BattleStartNode = currentNode;
    }

    public void TakeDamage(int amount, BaseEntity from = null)
    {
        if(dead)
        {
            return;
        }

        currentHealth -= amount;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "";

        // 死亡
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            currentNode.SetOccupied(false);
            currentNode.currentEntity = null;

            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
            InGameStateManager.Instance.OnPreparePhaseEnd -= OnPreparePhaseEnd;

            BattleManager.Instance.UnitDead(this);
        }
    }

    protected virtual void Attack()
    {
        if (!canAttack)
            return;

        attackCoroutine = StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
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
            currentTarget.TakeDamage(attackPower, this);
        }
        else
        {
            // 远程单位，构建攻击子弹
        }
        // 播放攻击后摇
        yield return new WaitForSeconds(attackRecover);
        canAttack = true;
    }
}
