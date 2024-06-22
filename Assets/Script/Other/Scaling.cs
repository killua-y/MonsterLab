using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scaling : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private float zoomScale = 1.1f;
    private float duration = 0.05f; // Duration of the scaling effect

    private Coroutine scalingCoroutine;

    private Canvas canvas;
    private int originalSortingOrder;

    void Start()
    {
        // Ensure the GameObject has a Canvas component
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }
        //originalSortingOrder = this.gameObject.transform.parent.GetComponent<Canvas>().sortingOrder;
        canvas.overrideSorting = true;
        originalSortingOrder = 19;
        canvas.sortingOrder = originalSortingOrder;

        // Ensure the GameObject has a GraphicRaycaster component
        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
        }

        // Bring to front layer
        canvas.sortingOrder += 1; // Set a high sorting order to bring it to the front

        scalingCoroutine = StartCoroutine(ScaleTo(new Vector3(zoomScale, zoomScale, 1.0f)));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scalingCoroutine != null)
        {
            StopCoroutine(scalingCoroutine);
        }

        // Restore original layer order
        canvas.sortingOrder = originalSortingOrder;

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
