using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DNAUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DNA DNAmodel;

    public void SetUp(DNA DNAModel)
    {
        DNAmodel = DNAModel;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DNAParent.Instance.GenerateDNAPreview(DNAmodel.DNAName, DNAmodel.effectText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DNAParent.Instance.HidePreview();
    }
}
