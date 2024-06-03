using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 单体造成额外伤害
public class SilvermaneSkill : MonsterSkill
{
    protected override void CastSpell()
    {
        // 计算伤害
        float damage = ((float)baseEntity.cardModel.effectData / 100) * baseEntity.cardModel.attackPower;
        int intDamage = (int)damage;

        BaseEntity target = null;
        if (baseEntity.HasEnemy)
        {
            target = baseEntity.currentTarget;
        }
        else
        {
            return;
        }

        // 构建攻击子弹
        GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInstance.GetComponent<SpriteRenderer>().color = Color.blue;
        if (bulletInstance.GetComponent<Bullet>() == null)
        {
            bulletInstance.AddComponent<Bullet>().Initialize(target, intDamage, baseEntity);
        }
        else
        {
            bulletInstance.GetComponent<Bullet>().Initialize(target, intDamage, baseEntity);
        }

        base.CastSpell();
    }
}

// 对所有敌人造成伤害
public class FrostFangSkill : MonsterSkill
{
    protected override void CastSpell()
    {
        float damage = ((float)baseEntity.cardModel.effectData / 100) * baseEntity.cardModel.attackPower;
        int intDamage = (int)damage;

        List<BaseEntity> allEnemy = BattleManager.Instance.GetEntitiesAgainst(baseEntity.myTeam);

        if (allEnemy.Count == 0)
        {
            return;
        }

        foreach (BaseEntity target in allEnemy)
        {
            // 构建攻击子弹
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.GetComponent<SpriteRenderer>().color = Color.blue;
            if (bulletInstance.GetComponent<Bullet>() == null)
            {
                bulletInstance.AddComponent<Bullet>().Initialize(target, intDamage, baseEntity);
            }
            else
            {
                bulletInstance.GetComponent<Bullet>().Initialize(target, intDamage, baseEntity);
            }
        }

        base.CastSpell();
    }
}
