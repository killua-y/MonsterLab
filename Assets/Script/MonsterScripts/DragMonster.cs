using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMonster : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 dragOffset = new Vector3(0, 0, 0);

    private Camera cam;
    private SpriteRenderer spriteRenderer;

    private Vector3 oldPosition;
    private int oldSortingOrder;
    private Tile previousTile = null;

    private bool IsDragging = false;

    private bool isPlayerMontser;

    private void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        OnDragging();
    }

    public void OnStartDrag()
    {
        isPlayerMontser = (this.GetComponent<BaseEntity>().myTeam == Team.Player);

        oldPosition = this.transform.position;
        oldSortingOrder = spriteRenderer.sortingOrder;

        spriteRenderer.sortingOrder = 20;
        IsDragging = true;
    }

    public void OnDragging()
    {
        if (!IsDragging)
            return;

        Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        newPosition.z = 0;
        this.transform.position = newPosition;

        Tile tileUnder = HelperFunction.GetTileUnder();
        if (tileUnder != null)
        {
            if (previousTile != null && tileUnder != previousTile)
            {
                //We are over a different tile.
                previousTile.SetHighlight(false, false);

                tileUnder.SetHighlight(true, isValid(GridManager.Instance.GetNodeForTile(tileUnder)));
            }

            previousTile = tileUnder;
        }
    }

    public void OnEndDrag()
    {
        if (!IsDragging)
            return;

        if (!TryRelease())
        {
            //Nothing was found, return to original position.
            this.transform.position = oldPosition;
        }

        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
            previousTile = null;
        }

        spriteRenderer.sortingOrder = oldSortingOrder;

        IsDragging = false;
    }

    private bool TryRelease()
    {
        //Released over something!
        Tile t = HelperFunction.GetTileUnder();
        if (t != null)
        {
            //It's a tile!
            BaseEntity thisEntity = GetComponent<BaseEntity>();
            Node candidateNode = GridManager.Instance.GetNodeForTile(t);
            if (candidateNode != null && thisEntity != null)
            {
                if (isValid(candidateNode))
                {
                    //Let's move this unity to that node
                    thisEntity.StandUp();
                    thisEntity.SitDown(candidateNode);
                    thisEntity.transform.position = candidateNode.worldPosition;

                    return true;
                }
            }
        }
        return false;
    }

    private bool isValid(Node node)
    {
        if ((!node.IsOccupied) && (isPlayerMontser == node.IsPlayerArea))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InGameStateManager.gamePased)
        {
            return;
        }

        // Check if the left mouse button was pressed
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnStartDrag();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnEndDrag();
        }
    }
}
