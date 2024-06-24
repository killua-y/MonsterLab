using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public Transform buttonParent;
    public TextMeshProUGUI eventText;
    public Image eventImage;
    public GameObject optionButtonPrefab;

    private bool isOpen = false;

    private List<string> layer1Event = new List<string>()
    {
        "SelectOneCardEvent",
        "GainGoldEvent"
    };

    public void ChangePosition()
    {
        if (isOpen)
        {
            StartCoroutine(SmoothMoveCoroutine(0, 1080));
            isOpen = false;
        }
        else
        {
            StartCoroutine(SmoothMoveCoroutine(1080, 0));
            isOpen = true;
        }
    }

    private IEnumerator SmoothMoveCoroutine(float startY, float endY)
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(startPosition.x, endY, startPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current position using Lerp
            float newY = Mathf.Lerp(startY, endY, elapsedTime / duration);
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set
        transform.localPosition = targetPosition;
    }

    public void LoadEvent(int layerNumber)
    {
        ChangePosition();

        if (layerNumber == 1)
        {
            this.gameObject.AddComponent(Type.GetType(layer1Event[0]));
            EventBehavior newEvent = this.gameObject.GetComponent<EventBehavior>();
            newEvent.SetUp();
        }
    }
}
