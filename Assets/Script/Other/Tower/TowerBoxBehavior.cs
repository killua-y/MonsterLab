using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBoxBehavior : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI boxTypeText;
    public BoxType boxType;
    public int row;
    public int column;
    private PlayerBehavior player;

    public void SetupBox(BoxType _boxType)
    {
        boxType = _boxType;
        boxTypeText.text = boxType.ToString();
    }


    public void OnPointDown()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        }

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
}
