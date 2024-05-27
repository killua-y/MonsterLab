using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DNARewardBehavior : MonoBehaviour, IPointerClickHandler
{
    public DNA DNAModel;

    public void SetUp(DNA _DNAModel)
    {
        DNAModel = _DNAModel;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerStatesManager.Instance.AcquireDNA(DNAModel);
        Destroy(this.gameObject);
    }
}
