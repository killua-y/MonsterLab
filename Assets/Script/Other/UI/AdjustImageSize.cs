using UnityEngine;
using TMPro;

public class AdjustImageSize : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;

    private RectTransform imageRectTransform;
    private float padding = 70f; // Padding around the text

    public void Setup(string _headerText, string _contentText)
    {
        imageRectTransform = this.GetComponent<RectTransform>();

        headerText.text = _headerText;
        contentText.text = _contentText;
    }

    public void AdjustImageSizeToText()
    {
        // Force update the canvas to ensure text info is updated
        Canvas.ForceUpdateCanvases();

        if (headerText == null || contentText == null)
        {
            Debug.LogError("TextMeshProUGUI components are not assigned.");
            return;
        }

        // Get the number of lines in the text
        int lineCount = contentText.textInfo.lineCount;

        // Get the height of a single line
        float lineHeight = contentText.fontSize;

        // Calculate the new height based on the number of lines and padding
        float newHeight = lineCount * lineHeight + headerText.fontSize + padding;

        // Adjust the size of the image
        imageRectTransform.sizeDelta = new Vector2(imageRectTransform.sizeDelta.x, newHeight);
    }

    private void OnEnable()
    {
        AdjustImageSizeToText();
    }
}
