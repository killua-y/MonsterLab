using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    // 所有canvas
    public Canvas HighPriorityCanvas;
    public Canvas MapCanvas;

    // 用于holding dna preview
    public GameObject DNAPreview;
    public TextMeshProUGUI DNANameText;
    public TextMeshProUGUI DNADescriptionText;

    // 用于holding card preview
    public GameObject cardPreview;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InGameStateManager.Instance.OnGameEnd += OnGameEnd;
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
        else if (cardPreview != null)
        {
            // Update the position of the text to follow the mouse
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rectTransform = cardPreview.GetComponent<RectTransform>();

            Vector2 adjustedPosition = mousePosition + new Vector2(rectTransform.rect.width / 2 + 100, 0);

            rectTransform.position = adjustedPosition;
        }
    }

    public void GenerateDNAPreview(string Name, string description)
    {
        DNAPreview.SetActive(true);
        DNANameText.text = Name;
        DNADescriptionText.text = description;
    }

    public void HideDNAPreview()
    {
        if (DNAPreview.activeSelf)
        {
            DNAPreview.SetActive(false);
        }
    }

    public void GenerateCardPreview(Card cardModel)
    {
        Card card = cardModel;
        cardPreview = CardDisplayView.Instance.DisPlaySingleCard(card, HighPriorityCanvas.transform);

        // Update the position of the text to follow the mouse
        Vector2 mousePosition = Input.mousePosition;

        RectTransform rectTransform = cardPreview.GetComponent<RectTransform>();

        Vector2 adjustedPosition = mousePosition + new Vector2(rectTransform.rect.width / 2 + 100, 0);

        rectTransform.position = adjustedPosition;
    }

    public void HideCardPreview()
    {
        if (cardPreview != null)
        {
            Destroy(cardPreview);
        }
    }

    public void SetMapCanvasActive(bool active)
    {
        MapCanvas.gameObject.SetActive(active);
    }

    public void OpenMap()
    {
        if (MapCanvas.gameObject.activeSelf)
        {
            MapCanvas.gameObject.SetActive(false);
        }
        else
        {
            MapCanvas.gameObject.SetActive(true);
        }
    }

    private void OnGameEnd()
    {
        SetMapCanvasActive(true);
    }
}
