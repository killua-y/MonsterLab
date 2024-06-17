using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public GameObject damageText;
    public Canvas canvas; // Reference to the Canvas

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (canvas == null)
        {
            canvas = this.GetComponent<Canvas>();
        }
    }

    public void GenerateDamageText(Vector2 position, int amount)
    {
        Debug.Log("Position is : " + position);
        // Convert world position to screen position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);

        // Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 canvasPosition);

        // 生成伤害
        GameObject instance = Instantiate(damageText, canvas.transform);
        instance.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        instance.GetComponent<DamageTextBehavior>().Setup(amount);
    }
}
