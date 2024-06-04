using UnityEngine;

public class IncreaseHealthOnDeathCardBehavior : CardBehavior
{
    public override void CastCard(Node node)
    {
        targetMonster = node.currentEntity;

        // 如果对象已经有了
        if (targetMonster.GetComponent<IncreaseHealthOnDeathMonsterBehavior>() == null)
        {
            Card newCard = Card.CloneCard(this.card);
            targetMonster.gameObject.AddComponent<IncreaseHealthOnDeathMonsterBehavior>().CardModel = newCard;
        }
        else
        {
            targetMonster.GetComponent<IncreaseHealthOnDeathMonsterBehavior>().CardModel.effectData += card.effectData;
        }
    }

    public override void IndividualCastComplete(Node node)
    {
        node.currentEntity.cardModel.equippedCard.Add(card);
    }
}

public class IncreaseHealthOnDeathMonsterBehavior : MonoBehaviour
{
    private BaseEntity baseEntity;
    public Card CardModel;

    private void Start()
    {
        baseEntity = this.gameObject.GetComponent<BaseEntity>();
        if (baseEntity != null)
        {
            baseEntity.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        baseEntity.cardModel.healthPoint += CardModel.effectData;
        baseEntity.UpdateMonster();
    }

    private void OnDestroy()
    {
        baseEntity.OnDeath -= OnDeath;
    }
}
