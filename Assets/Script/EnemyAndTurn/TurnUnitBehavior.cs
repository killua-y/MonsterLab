using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUnitBehavior : MonoBehaviour
{
    public GameObject dot;
    public GameObject arrow;
    public GameObject passedTurn;
    public GameObject currentTurn;
    public GameObject futureTurn;
    public GameObject SummonTurn;

    public TurnType turnType;
    public int index;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTurn(int turnIndex)
    {
        if (index == turnIndex)
        {
            if (currentTurn != null)
            {
                currentTurn.SetActive(true);
            }
        }
        else if (index > turnIndex)
        {
            if (futureTurn != null)
            {
                futureTurn.SetActive(true);
            }
        }
        else if (index < turnIndex)
        {
            if (passedTurn != null)
            {
                passedTurn.SetActive(true);
            }
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
