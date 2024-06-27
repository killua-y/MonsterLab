using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private BaseEntity target;
    private int damage;
    private BaseEntity attacker;

    public void Initialize(BaseEntity target, int damage, BaseEntity attacker)
    {
        this.target = target;
        this.damage = damage;
        this.attacker = attacker;
    }

    public void Initialize(BaseEntity target, BaseEntity attacker)
    {
        this.target = target;
        this.attacker = attacker;
    }

    void Update()
    {
        if ((target == null) || (target.dead))
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if bullet has reached the target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            if (!target.dead)
            {
                if (damage != 0)
                {
                    // 有预先传入的伤害
                    target.TakeDamage(damage, attacker);
                }
                else
                {
                    // 没有预先传入的伤害，是普通攻击
                    attacker.Strike(target);
                }
            }

            Destroy(gameObject);
        }
    }
}
