using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUnitBehavior : MonoBehaviour
{
    public Image StatusPicture;
    public TextMeshProUGUI textMeshProUGUI;

    public void Setup(string imageLocation)
    {
        if (imageLocation != "")
        {
            StatusPicture.sprite = Resources.Load<Sprite>(imageLocation);
        }
    }

    public void UpdateStatusNumber(int number)
    {
        textMeshProUGUI.text = number.ToString();
    }
}
