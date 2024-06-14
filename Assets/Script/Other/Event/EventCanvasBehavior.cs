using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventCanvasBehavior : MonoBehaviour
{
    public static EventCanvasBehavior instance;
    public Transform buttonParent;
    public TextMeshProUGUI eventText;
    public Image eventImage;
    public GameObject optionButtonPrefab;

    private List<string> layer1Event = new List<string>()
    {
        "SelectOneCardEvent",
        "GainGoldEvent"
    };

    private bool isOpen = false;

    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    public void ChangePosition()
    {
        if (isOpen)
        {
            StartCoroutine(SmoothMoveCoroutine(956, 1160));
            isOpen = false;
        }
        else
        {
            StartCoroutine(SmoothMoveCoroutine(1160, 956));
            isOpen = true;
        }
    }

    private IEnumerator SmoothMoveCoroutine(float startX, float endX)
    {
        float elapsedTime = 0;
        float duration = 0.2f;
        Vector3 startPosition = transform.localPosition;
        Vector3 targetPosition = new Vector3(endX, startPosition.y, startPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the current position using Lerp
            float newX = Mathf.Lerp(startX, endX, elapsedTime / duration);
            transform.localPosition = new Vector3(newX, startPosition.y, startPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position is set
        transform.localPosition = targetPosition;
    }

    public void LoadEvent(int layerNumber)
    {
        this.gameObject.SetActive(true);
        if (layerNumber == 1)
        {
            this.gameObject.AddComponent(Type.GetType(layer1Event[0]));
            EventBehavior newEvent = this.gameObject.GetComponent<EventBehavior>();
            newEvent.SetUp();

        }
    }
}
