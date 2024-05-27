using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DNAParent : MonoBehaviour
{
    public static DNAParent Instance;
    public GameObject DNAPreview;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (DNAPreview.activeSelf)
        {
            // Update the position of the text to follow the mouse
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rectTransform = DNAPreview.GetComponent<RectTransform>();

            Vector2 adjustedPosition = mousePosition + new Vector2(rectTransform.rect.width / 2 + 20, -rectTransform.rect.height / 2 - 20);

            rectTransform.position = adjustedPosition;
        }
    }

    public void GenerateDNAPreview(string Name, string description)
    {
        DNAPreview.SetActive(true);
        nameText.text = Name;
        descriptionText.text = description;
    }

    public void HidePreview()
    {
        DNAPreview.SetActive(false);
    }
}
