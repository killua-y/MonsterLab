using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MonsterCardBehavior : CardBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CastCard(Tile _tile, Card _card = null)
    {
        if (card is not MonsterCard)
        {
            Debug.Log("Worng card model");
            return;
        }

        MonsterCard cardModel = (MonsterCard)card;

        string path = "";
        if (cardModel.modelLocation == "")
        {
            path = cardModel.modelLocation;
        }
        else
        {
            path = "Assets/Resources/MonsterPrefab/Slime.prefab";
        }

        BattleManager.Instance.InstaniateMontser(_tile.transform.GetSiblingIndex(), Team.Player, Resources.Load<GameObject>(path), cardModel);

        InGameStateManager.Instance.ExhaustOneCard(card);
        Destroy(this.gameObject);
    }

    public override void OnPointDown()
    {
        BattleManager.Instance.DisplayMonsterSpaceText();
    }

    public override void OnPointUp()
    {
        BattleManager.Instance.StopDisplayMonsterSpaceText();
    }
}
