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
        CanvasManager.Instance.GenerateDNAPreview(DNAmodel.DNAName, DNAmodel.effectText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasManager.Instance.HideDNAPreview();
    }
}
