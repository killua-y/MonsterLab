using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MonsterCardBehavior : CardBehavior
{
    private List<BaseEntity> sacrfices = null;

    public override void CheckLegality(Node node)
    {
        targetNode = node;

        // 查看是否需要祭品
        if (card.cost != 0)
        {
            if (BattleManager.Instance.GetEntitiesAgainst(Team.Enemy).Count >= card.cost)
            {
                StartCoroutine(GetTiles(card.cost));
            }
            else
            {
                // 场上祭品不够
                return;
            }
        }
        else
        {
            // 场上满了，无法召唤
            if (BattleManager.Instance.GetEntitiesAgainst(Team.Enemy).Count >= PlayerStatesManager.maxUnit)
            {
                return;
            }

            // 合法，释放卡牌效果
            CastCard(node);

            CastComplete();
        }
    }

    // The function to get multiple tiles based on user clicks
    public IEnumerator GetTiles(int n)
    {
        List<Tile> clickedTiles = new List<Tile>();
        InGameStateManager.gamePased = true;

        while (clickedTiles.Count < n)
        {
            if (Input.GetMouseButtonDown(0)) // Check for mouse click
            {
                Tile clickedTile = HelperFunction.GetTileUnder();
                if (clickedTile != null)
                {
                    // 如果重复点击则取消选中
                    if (clickedTiles.Contains(clickedTile))
                    {
                        clickedTile.SetHighlight(false, true);
                        clickedTiles.Remove(clickedTile);
                    }
                    else
                    {
                        Node clickedNode = GridManager.Instance.GetNodeForTile(clickedTile);

                        if ((clickedNode.IsPlayerArea == true) && (clickedNode.IsOccupied))
                        {
                            clickedTile.SetHighlight(true, true);
                            clickedTiles.Add(clickedTile);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1)) // Check for mouse click
            {
                foreach (Tile tile in clickedTiles)
                {
                    tile.SetHighlight(false, true);
                }

                InGameStateManager.gamePased = false;
                yield break;
            }

            yield return null; // Wait for the next frame
        }

        foreach (Tile tile in clickedTiles)
        {
            tile.SetHighlight(false, true);
        }

        InGameStateManager.gamePased = false;

        // 召唤
        sacrfices = new List<BaseEntity>();
        foreach (Tile tile in clickedTiles)
        {
            sacrfices.Add(GridManager.Instance.GetNodeForTile(tile).currentEntity.GetComponent<BaseEntity>());
        }

        // 合法，释放卡牌效果
        CastCard(targetNode);

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
        IsDragging = true;
        BattleManager.Instance.DisplayMonsterSpaceText();
    }

    public override void OnPointUp()
    {
        IsDragging = false;
        BattleManager.Instance.StopDisplayMonsterSpaceText();
        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
        }
    }
}
