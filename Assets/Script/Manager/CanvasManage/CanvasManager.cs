using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasManager : Singleton<CanvasManager>
{
    // 所有canvas
    [Header("All canvas")]
    public Canvas HighPriorityCanvas;
    public Canvas MapCanvas;

    // 用于holding dna preview
    [Header("DNA preview")]
    public GameObject DNAPreview;
    public TextMeshProUGUI DNANameText;
    public TextMeshProUGUI DNADescriptionText;

    // 用于holding card preview
    [Header("Monster Card Preview")]
    private GameObject cardPreview;
    private RectTransform cardPreviewRectTransform;
    private int disPlayOffset = 100;

    [Header("CardHolder")]
    [SerializeField]
    public Transform extraDeck;
    public Transform drawPileParent;
    public Transform discardPileParent;

    // 用于展示提示
    [Header("Other")]
    public TextMeshProUGUI IndicationText;

    private void Start()
    {
        InGameStateManager.Instance.OnCombatEnd += OnCombatEnd;
    }

    void Update()
    {
        // 右键点击清除所有额外界面
        if (Input.GetMouseButtonDown(1))
        {
            HideAllOtherPanel();
        }

        if (DNAPreview.activeSelf)
        {
            // Update the position of the text to follow the mouse
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rectTransform = DNAPreview.GetComponent<RectTransform>();

            Vector2 adjustedPosition = mousePosition +
                new Vector2(200 * GameSetting.scaleFactor, -50 * GameSetting.scaleFactor);

            rectTransform.position = adjustedPosition;
        }
        else if (cardPreviewRectTransform != null)
        {
            // Update the position of the text to follow the mouse
            Vector2 mousePosition = Input.mousePosition;

            // Convert mouse position to canvas space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(HighPriorityCanvas.transform as RectTransform, mousePosition, HighPriorityCanvas.worldCamera, out Vector2 localPoint);

            // Calculate the new anchored position
            Vector2 newPosition = localPoint;

            newPosition = newPosition + new Vector2((cardPreviewRectTransform.rect.width / 2 + 100) * disPlayOffset, 0);

            // Get the canvas size
            RectTransform canvasRect = HighPriorityCanvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta - new Vector2(100, 100);

            // Get the size of the image
            Vector2 imageSize = cardPreviewRectTransform.sizeDelta;

            // Adjust the position to stay within screen boundaries
            newPosition.y = Mathf.Clamp(newPosition.y, -canvasSize.y / 2 + imageSize.y / 2, canvasSize.y / 2 - imageSize.y / 2);

            // Set the new anchored position
            cardPreviewRectTransform.anchoredPosition = newPosition;
        }
    }

    private void HideAllOtherPanel()
    {
        if (InGameStateManager.inCombat)
        {
            if (extraDeck.gameObject.activeSelf)
            {
                extraDeck.gameObject.SetActive(false);
            }
            else if (drawPileParent.gameObject.activeSelf)
            {
                drawPileParent.gameObject.SetActive(false);
            }
            else if (discardPileParent.gameObject.activeSelf)
            {
                discardPileParent.gameObject.SetActive(false);
            }
            else if (MapCanvas.gameObject.activeSelf)
            {
                MapCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void OnCombatEnd()
    {
        SetMapCanvasActive(true);
    }

    public void OpenDeck()
    {
        DeckManage.Instance.OpenDeck();
    }

    public void GenerateDNAPreview(string Name, string description)
    {
        DNAPreview.GetComponent<AdjustImageSize>().Setup(Name, description);
        DNAPreview.SetActive(true);
        //DNAPreview.GetComponent<AdjustImageSize>().AdjustImageSizeToText();
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
        cardPreviewRectTransform = cardPreview.GetComponent<RectTransform>();
        cardPreview.GetComponent<CardDisplay>().ShowKeyWord();

        // Update the position of the text to follow the mouse
        Vector2 mousePosition = Input.mousePosition;

        // Determine if the mouse is on the left or right side of the screen
        if (mousePosition.x < Screen.width / 2)
        {
            disPlayOffset = 1;
        }
        else
        {
            disPlayOffset = -1;
            cardPreview.GetComponent<CardDisplay>().FlipKeyWord(true);
        }
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
            HideAllOtherPanel();
            MapCanvas.gameObject.SetActive(true);
        }
    }

    public void ShowExtraDeck()
    {
        if (extraDeck.gameObject.activeSelf)
        {
            extraDeck.gameObject.SetActive(false);
        }
        else
        {
            HideAllOtherPanel();
            extraDeck.gameObject.SetActive(true);
        }
    }

    public void ShowDarwPile()
    {
        if (drawPileParent.gameObject.activeSelf)
        {
            drawPileParent.gameObject.SetActive(false);
        }
        else
        {
            HideAllOtherPanel();
            InGameStateManager.Instance.ShowDarwPile();
        }
    }

    public void ShowDiscardPile()
    {
        if (discardPileParent.gameObject.activeSelf)
        {
            discardPileParent.gameObject.SetActive(false);
        }
        else
        {
            HideAllOtherPanel();
            InGameStateManager.Instance.ShowDiscardPile();
        }
    }

    public void ShowIndicationText(string textString)
    {
        IndicationText.text = textString;
        IndicationText.gameObject.SetActive(true);
    }

    public void HideIndicationText()
    {
        IndicationText.gameObject.SetActive(false);
    }
}
