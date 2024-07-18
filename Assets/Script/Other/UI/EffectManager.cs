using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public GameObject damageText;
    public Canvas canvas; // Reference to the Canvas

    public GameObject BlueSummonEffect;
    public GameObject RedSummonEffect;
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
        // Convert world position to screen position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(position);

        // Convert screen position to canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 canvasPosition);

        // 生成伤害
        GameObject instance = Instantiate(damageText, canvas.transform);
        instance.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        instance.GetComponent<DamageTextBehavior>().Setup(amount);
    }

    public void GenerateSummonEffect(Vector2 position, Team team)
    {
        if (team == Team.Player)
        {
            Instantiate(BlueSummonEffect,
                position + new Vector2(BlueSummonEffect.transform.position.x, BlueSummonEffect.transform.position.y),
                BlueSummonEffect.transform.rotation);
        }
        else if (team == Team.Enemy)
        {
            Instantiate(RedSummonEffect,
                position + new Vector2(RedSummonEffect.transform.position.x, RedSummonEffect.transform.position.y),
                RedSummonEffect.transform.rotation);
        }
    }
}
