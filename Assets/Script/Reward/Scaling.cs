using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scaling : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private float zoomScale = 1.2f;

    void Start()
    {

    }

    //当鼠标进入UI后执行的事件执行的
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(zoomScale, zoomScale, 1.0f);
    }

    //当鼠标离开UI后执行的事件执行的
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
