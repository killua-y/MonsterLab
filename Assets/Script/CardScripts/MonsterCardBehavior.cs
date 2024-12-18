using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class MonsterCardBehavior : CardBehavior
{
    private List<BaseEntity> sacrfices = null;
    protected MonsterCard monsterCard;

    public override void CheckLegality(Node node)
    {
        if (cardModel is not MonsterCard)
        {
            Debug.Log("Worng card model");
            return;
        }

        monsterCard = (MonsterCard)cardModel;

        targetNode = node;

        // 查看费用是否合理
        if (playerCostManager.currentCost < monsterCard.cost)
        {
            return;
        }

        // 查看是否需要祭品
        if (monsterCard.rank != 0)
        {
            if (BattleManager.Instance.GetEntitiesAgainst(Team.Enemy).Count >= monsterCard.rank)
            {
                StartCoroutine(GetTiles(monsterCard.rank));
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

            CastComplete(node);
        }
    }

    // The function to get multiple tiles based on user clicks
    public IEnumerator GetTiles(int n)
    {
        List<Tile> clickedTiles = new List<Tile>();
        InGameStateManager.gamePased = true;
        CanvasManager.Instance.ShowIndicationText("Select the monster to be the tribute 0/" + n);

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
                        CanvasManager.Instance.ShowIndicationText("Select the monster to be the tribute " + clickedTiles.Count + "/" + n);
                    }
                    else
                    {
                        Node clickedNode = GridManager.Instance.GetNodeForTile(clickedTile);

                        if ((clickedNode.IsPlayerArea == true) && (clickedNode.IsOccupied))
                        {
                            clickedTile.SetHighlight(true, true);
                            clickedTiles.Add(clickedTile);
                            CanvasManager.Instance.ShowIndicationText("Select the monster to be the tribute " + clickedTiles.Count + "/" + n);
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
                CanvasManager.Instance.HideIndicationText();
                yield break;
            }

            yield return null; // Wait for the next frame
        }

        foreach (Tile tile in clickedTiles)
        {
            tile.SetHighlight(false, true);
        }

        InGameStateManager.gamePased = false;
        CanvasManager.Instance.HideIndicationText();

        // 召唤
        sacrfices = new List<BaseEntity>();
        foreach (Tile tile in clickedTiles)
        {
            sacrfices.Add(GridManager.Instance.GetNodeForTile(tile).currentEntity.GetComponent<BaseEntity>());
        }

        // 合法，释放卡牌效果
        CastCard(targetNode);

        CastComplete(targetNode);
    }

    public override void CastCard(Node node)
    {
        BattleManager.Instance.InstaniateMontser(node, Team.Player, monsterCard, sacrfices);
    }

    public override void OnPointDown()
    {
        base.OnPointDown();
        BattleManager.Instance.DisplayMonsterSpaceText();
    }

    public override void OnPointUp()
    {
        base.OnPointUp();
        BattleManager.Instance.StopDisplayMonsterSpaceText();
    }
}
