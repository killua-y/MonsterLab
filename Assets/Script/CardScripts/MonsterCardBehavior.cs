using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MonsterCardBehavior : CardBehavior
{
    private Tile tToSummon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckLegality(Tile _tile)
    {
        Node node = GridManager.Instance.GetNodeForTile(_tile);

        if (card.castType == CastType.PlayerEmptyTile)
        {
            if (node.currentEntity != null)
            {
                return;
            }
        }

        // 查看是否需要祭品
        if (card.cost != 0)
        {
            StartCoroutine(CardOnPlay.Instance.GetTiles(card.cost, OnTilesCollected));
            tToSummon = _tile;
        }
        else
        {
            // 合法，释放卡牌效果
            CastCard(_tile);

            CastComplete();
        }
    }

    void OnTilesCollected(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            GridManager.Instance.GetNodeForTile(tile).currentEntity.GetComponent<BaseEntity>().UnitDie(null, true);
        }

        // 合法，释放卡牌效果
        CastCard(tToSummon);

        CastComplete();
    }

    public override void CastCard(Tile _tile)
    {
        if (card is not MonsterCard)
        {
            Debug.Log("Worng card model");
            return;
        }

        MonsterCard cardModel = (MonsterCard)card;

        BattleManager.Instance.InstaniateMontser(_tile.transform.GetSiblingIndex(), Team.Player, cardModel);
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
