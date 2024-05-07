using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public SpriteRenderer spriteRender;

    public int baseDamage = 1;
    public int baseHealth = 3;
    [Range(1.5f, 10)]
    public float range = 1.5f;
    public float attackSpeed = 1f; //Attacks per second
    public float movementSpeed = 1f; //Attacks per second

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

    public void Setup(Team team, Node currentNode)
    {
        myTeam = team;
        if (myTeam == Team.Team2)
        {
            spriteRender.flipX = true;
        }

        this.currentNode = currentNode;
        transform.position = currentNode.worldPosition;
        currentNode.SetOccupied(true);
    }

    protected void Start()
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
            if (path == null && path.Count >= 1)
                return;

            if (path[1].IsOccupied)
                return;

            path[1].SetOccupied(true);
            destination = path[1];
        }

        moving = !MoveTowards(destination);
        if (!moving)
        {
            //Free previous node
            currentNode.SetOccupied(false);
            SetCurrentNode(destination);
        }
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("take damage: " + amount);
        //baseHealth -= amount;
        //healthbar.UpdateBar(baseHealth);

        //if (baseHealth <= 0 && !dead)
        //{
        //    dead = true;
        //    currentNode.SetOccupied(false);
        //    GameManager.Instance.UnitDead(this);
        //}
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack!!");
        //if (!canAttack)
        //    return;

        //waitBetweenAttack = 1 / attackSpeed;
        //StartCoroutine(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        canAttack = false;
        yield return null;
        yield return new WaitForSeconds(waitBetweenAttack);
        canAttack = true;
    }
}
