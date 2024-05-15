using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [Range(1.5f, 10)]
    public float range = 1.5f;
    public float attackSpeed = 1f; //攻击间隔
    public float attackPreparation = 0.5f; //攻击前摇
    public float attackRecover = 0.5f; //攻击后摇
    public float movementSpeed = 1f; //移动速度

    protected Team myTeam;
    protected BaseEntity currentTarget = null;
    protected Node currentNode;

    public Node CurrentNode => currentNode;

    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;
    protected bool moving;
    protected Node destination;

    protected bool dead = false;
    protected bool canAttack = true;
    protected float waitBetweenAttack;

    // private
    private int currentHealth;

    public void Setup(Team team, Node currentNode)
    {
        myTeam = team;
        if (myTeam == Team.Enemy)
        {
            spriteRender.flipX = true;
            fillImage = healthBar.fillRect.GetComponent<Image>();
            fillImage.color = Color.red;
        }

        this.currentNode = currentNode;
        transform.position = currentNode.worldPosition;
        currentNode.SetOccupied(true);
        currentNode.currentEntity = this;

        // 属性UI设置
        currentHealth = healthPoint;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
        attackText.text = attackPower + "";
        healthText.text = currentHealth + "";
    }

    protected void Awake()
    {

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
        //Debug.Log("Find enemy with position: " + entity.transform.position);
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
        CurrentNode.SetOccupied(false);
        SetCurrentNode(node);
        node.SetOccupied(true);
        transform.position = node.worldPosition;
    }

    public void TakeDamage(int amount, BaseEntity from = null)
    {
        currentHealth -= amount;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "";

        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            currentNode.SetOccupied(false);
            currentNode.currentEntity = null;
            BattleManager.Instance.UnitDead(this);
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
        canAttack = false;
        // 开始播放攻击动画
        yield return new WaitForSeconds(attackPreparation);
        // 攻击击中敌方
        if(bullet == null)
        {
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
