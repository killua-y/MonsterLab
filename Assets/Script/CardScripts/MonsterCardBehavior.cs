using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MonsterCardBehavior : CardBehavior
{
    private Node tToSummon;
    private List<BaseEntity> sacrfices = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckLegality(Node node)
    {
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
            tToSummon = node;
        }
        else
        {
            // 合法，释放卡牌效果
            CastCard(node);

            CastComplete();
        }
    }

    void OnTilesCollected(List<Tile> tiles)
    {
        sacrfices = new List<BaseEntity>();
        foreach (Tile tile in tiles)
        {
            sacrfices.Add(GridManager.Instance.GetNodeForTile(tile).currentEntity.GetComponent<BaseEntity>());
        }

        // 合法，释放卡牌效果
        CastCard(tToSummon);

        CastComplete();
    }

    public override void CastCard(Node node)
    {
        if (card is not MonsterCard)
        {
            Debug.Log("Worng card model");
            return;
        }

        MonsterCard cardModel = (MonsterCard)card;

        BattleManager.Instance.InstaniateMontser(node, Team.Player, cardModel, sacrfices);
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
