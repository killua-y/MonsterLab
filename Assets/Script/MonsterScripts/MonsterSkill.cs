using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkill : MonoBehaviour
{
    protected BaseEntity baseEntity;
    protected MonsterUI monsterUI;
    protected int currentMana;
    protected GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {

        monsterUI = this.gameObject.GetComponent<MonsterUI>();
        baseEntity = this.gameObject.GetComponent<BaseEntity>();

        if (baseEntity != null)
        {
            int Mana = baseEntity.cardModel.Mana;
            if (Mana == 0)
            {
                Debug.Log("wrong, this monster has not been assign a mana");
                return;
            }

            baseEntity.OnAttack += OnAttack;
            baseEntity.OnTakeDamage += OnTakeDamage;
            InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
            bullet = monsterUI.bullet;
        }
    }

    private void Update()
    {
        if (currentMana >= baseEntity.cardModel.Mana)
        {
            CastSpell();
        }
    }

    void OnAttack()
    {
        IncreaseMana(10);
    }

    void OnTakeDamage(int damageAmount)
    {
        IncreaseMana(damageAmount);
    }

    void IncreaseMana(int amount)
    {
        currentMana += amount;

        monsterUI.UpdateManaUI(baseEntity.cardModel.Mana, currentMana);
    }

    protected virtual void CastSpell()
    {
        currentMana = 0;
    }

    private void OnPreparePhaseStart()
    {
        currentMana = 0;
        monsterUI.UpdateManaUI(baseEntity.cardModel.Mana, currentMana);
    }

    private void OnDestroy()
    {
        baseEntity.OnAttack -= OnAttack;
        baseEntity.OnTakeDamage -= OnTakeDamage;
        InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
    }
}
