using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class CardBehavior : MonoBehaviour
{
    public bool targetCard;

    public Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeCard(Card _card)
    {
        card = _card;

        // 设置卡牌释放类型
        if (card is MonsterCard)
        {
            targetCard = true;
        }
        else if (card is ItemCard)
        {
            targetCard = true;
        }
        else if (card is SpellCard)
        {
            targetCard = false;
        }
    }

    public void CastCard(Tile _tile)
    {

        if (card is MonsterCard)
        {
            Debug.Log("Find tile: " + _tile);
            string path = "Assets/Resources/MonsterPrefab/Slime.prefab";
            BattleManager.Instance.InstaniateMontser(_tile.transform.GetSiblingIndex(), Team.Enemy, Resources.Load<GameObject>(path));

            InGameStateManager.Instance.ExhaustOneCard(card);
            Destroy(this.gameObject);

            if (((MonsterCard)card).modelLocation != "\\r")
            {
                Debug.Log("Do not missing monster card: " + card.cardName + "'s model location");
                GameObject newMonster = Resources.Load<GameObject>(((MonsterCard)card).modelLocation);
            }
            else
            {
                Debug.Log("Missing monster card: " + card.cardName + "'s model location");
            }
        }
    }
}
