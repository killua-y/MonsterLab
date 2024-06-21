using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBoxBehavior : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{
    public BoxType boxType;
    public int row;
    public int column;
    private PlayerBehavior player;

    private Coroutine scalingCoroutine;
    private float zoomScale = 1.3f;
    private float duration = 0.05f; // Duration of the scaling effect

    public void SetupBox(BoxType _boxType)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        }

        boxType = _boxType;
        string imageLocation = "";
        switch (_boxType)
        {
            case BoxType.NormalFight:
                imageLocation = "UI/TowerBoxIcon/normal";
                break;
            case BoxType.EliteFight:
                imageLocation = "UI/TowerBoxIcon/elite";
                break;
            case BoxType.BossFight:
                imageLocation = "UI/TowerBoxIcon/boss";
                break;
            case BoxType.Events:
                imageLocation = "UI/TowerBoxIcon/event";
                break;
            case BoxType.Merchant:
                imageLocation = "UI/TowerBoxIcon/merchent";
                break;
            case BoxType.Treasure:
                imageLocation = "UI/TowerBoxIcon/treasure";
                break;
        }

        if (imageLocation == "")
        {
            this.GetComponent<Image>().enabled = false;
        }
        else
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>(imageLocation);
        }

        ActsManager.Instance.OnPlayerMove += OnPlayerMove;
    }

    public void OnPointDown()
    {
        // 查看是否可以移动到这
        if (CheckLegality())
        {
            player.transform.position = this.transform.position;
            player.row = row;
            player.column = column;
            ActivateAct();
        }
    }

    private bool CheckLegality()
    {
        // 查看是否在玩家的上方或者右边一位index
        if (((player.row - 1) == row) && (player.column == column))
        {
            return true;
        }
        else if ((player.row == row) && ((player.column + 1) == column))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActivateAct()
    {
        ActsManager.Instance.ActivateAct(boxType);
        this.boxType = BoxType.Passed;
        ActsManager.Instance.OnPlayerMove?.Invoke(this.row, this.column);
    }

    private void OnPlayerMove(int _row, int _column)
    {
        if (boxType != BoxType.Passed)
        {
            if ((this.row > _row) || (this.column < _column))
            {
                ActsManager.Instance.OnPlayerMove -= OnPlayerMove;
                this.boxType = BoxType.CannotGetTo;

                // 将无法到达的格子设置为透明
                Color color = this.GetComponent<Image>().color;
                color.a = 0.4f;
                this.GetComponent<Image>().color = color;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InGameStateManager.inCombat)
        {
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnPointDown();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CheckLegality())
        {
            if (scalingCoroutine != null)
            {
                StopCoroutine(scalingCoroutine);
            }

            scalingCoroutine = StartCoroutine(ScaleTo(new Vector3(zoomScale, zoomScale, 1.0f)));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
        }

        scalingCoroutine = StartCoroutine(ScaleTo(new Vector3(1f, 1f, 1f)));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float time = 0;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
