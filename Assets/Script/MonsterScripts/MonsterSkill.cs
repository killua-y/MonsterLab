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
            InGameStateManager.Instance.OnPreparePhaseStart += OnPreparePhaseStart;
            bullet = monsterUI.bullet;
        }
    }

    void OnAttack()
    {
        IncreaseMana(10);
    }

    void IncreaseMana(int amount)
    {
        if (currentMana >= baseEntity.cardModel.Mana)
        {
            return;
        }

        currentMana += amount;

        if (currentMana >= baseEntity.cardModel.Mana)
        {
            ReadyToCastSpell();
        }

        monsterUI.UpdateManaUI(baseEntity.cardModel.Mana, currentMana);
    }

    private void ReadyToCastSpell()
    {
        baseEntity.canCastSkill = true;
    }

    public virtual void CastSpell(Transform attackOrgan = null)
    {
        currentMana = 0;
        monsterUI.UpdateManaUI(baseEntity.cardModel.Mana, currentMana);
    }

    private void OnPreparePhaseStart()
    {
        currentMana = 0;
        monsterUI.UpdateManaUI(baseEntity.cardModel.Mana, currentMana);
    }

    private void OnDestroy()
    {
        baseEntity.OnAttack -= OnAttack;
        InGameStateManager.Instance.OnPreparePhaseStart -= OnPreparePhaseStart;
    }
}
