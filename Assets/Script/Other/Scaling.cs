using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scaling : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private float zoomScale = 1.1f;
    private float duration = 0.05f; // Duration of the scaling effect

    private Coroutine scalingCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
        }
        scalingCoroutine = StartCoroutine(ScaleTo(new Vector3(zoomScale, zoomScale, 1.0f)));
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
