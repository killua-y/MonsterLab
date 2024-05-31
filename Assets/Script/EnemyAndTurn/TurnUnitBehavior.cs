using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUnitBehavior : MonoBehaviour
{
    public GameObject dot;
    public GameObject arrow;
    public GameObject SummonTurn;

    public TurnType turnType;
    public int index;

    public void UpdateTurn(int turnIndex)
    {
        if (index == turnIndex)
        {
            this.GetComponent<Image>().color = Color.yellow;
        }
        else if (index > turnIndex)
        {
            this.GetComponent<Image>().color = Color.white;
        }
        else if (index < turnIndex)
        {
            this.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            Debug.Log("index did not assigned");
        }
    }

    public void SetToSummonMonster()
    {
        turnType = TurnType.EnemySummon;
        SummonTurn.SetActive(true);
    }
}

public enum TurnType
{
    EnemySummon,
    Rest
}
