using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    // 所有canvas
    [Header("All canvas")]
    public Canvas HighPriorityCanvas;
    public Canvas MapCanvas;
    public Canvas ShopCanvas;

    // 用于holding dna preview
    [Header("DNA preview")]
    public GameObject DNAPreview;
    public TextMeshProUGUI DNANameText;
    public TextMeshProUGUI DNADescriptionText;

    // 用于holding card preview
    [Header("Monster Card Preview")]
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
            RectTransform rectTransform = cardPreview.GetComponent<RectTransform>();
            // Update the position of the text to follow the mouse
            Vector2 mousePosition = Input.mousePosition;

            // Convert mouse position to canvas space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(HighPriorityCanvas.transform as RectTransform, mousePosition, HighPriorityCanvas.worldCamera, out Vector2 localPoint);

            // Calculate the new anchored position
            Vector2 newPosition = localPoint;

            // Get the canvas size
            RectTransform canvasRect = HighPriorityCanvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta;

            // Get the size of the image
            Vector2 imageSize = rectTransform.sizeDelta;

            // Adjust the position to stay within screen boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, -canvasSize.x / 2 + imageSize.x / 2, canvasSize.x / 2 - imageSize.x / 2);
            newPosition.y = Mathf.Clamp(newPosition.y, -canvasSize.y / 2 + imageSize.y / 2, canvasSize.y / 2 - imageSize.y / 2);

            Vector2 adjustedPosition = newPosition + new Vector2(rectTransform.rect.width / 2 + 100, 0);

            // Set the new anchored position
            rectTransform.anchoredPosition = adjustedPosition;
        }
    }

    private void OnGameEnd()
    {
        SetMapCanvasActive(true);
    }

    public void HideAllOtherPanel()
    {

    }

    public void OpenDeck()
    {
        DeckManage.Instance.OpenDeck();
    }

    public void OpenShopCanvas()
    {
        ShopCanvas.gameObject.SetActive(true);
        ShopManager.Instance.GenerateShop();
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
}
