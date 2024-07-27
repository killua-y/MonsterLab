using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public string ShootEffect;
    public string ExplosionEffect;
    private BaseEntity target;
    private int damage;
    private BaseEntity attacker;

    public void Initialize(BaseEntity target, int damage, BaseEntity attacker)
    {
        this.target = target;
        this.damage = damage;
        this.attacker = attacker;

        // 播放生成特效
        if ((ShootEffect != null) && (ShootEffect != ""))
        {
            EffectManager.Instance.PlayEffect(ShootEffect, transform.position);
        }
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

        this.transform.LookAt(target.transform.position);

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
                    target.TakeDamage(damage, DamageType.MonsterSkill, attacker);
                }
                else
                {
                    // 没有预先传入的伤害，是普通攻击
                    attacker.Strike(target);
                }
            }

            Finish();
        }
    }

    void Finish()
    {
        // 播放结束爆炸特效
        if ((ExplosionEffect != null) && (ExplosionEffect != ""))
        {
            EffectManager.Instance.PlayEffect(ExplosionEffect, transform.position);
        }

        Destroy(gameObject);
    }
}
