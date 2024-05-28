using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DNARewardBehavior : MonoBehaviour, IPointerClickHandler
{
    public DNA DNAModel;
    public TextMeshProUGUI nameText;

    public void SetUp(DNA _DNAModel)
    {
        DNAModel = _DNAModel;
        nameText.text = DNAModel.DNAName;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerStatesManager.Instance.AcquireDNA(DNAModel);
        Destroy(this.gameObject);
    }
}
