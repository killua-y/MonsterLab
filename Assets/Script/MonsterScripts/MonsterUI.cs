using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Card;

public class MonsterUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 怪兽UI部分
    public SpriteRenderer spriteRender;
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;
    public GameObject bullet;

    public void EnemyMonster()
    {
        spriteRender.flipX = true;
        fillImage = healthBar.fillRect.GetComponent<Image>();
        fillImage.color = Color.red;
    }

    public void UpdateHealth(int currentHealth)
    {
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "";
    }

    public void UpdateUI(MonsterCard cardModel)
    {
        healthBar.maxValue = cardModel.healthPoint;
        healthBar.value = cardModel.healthPoint;
        attackText.text = cardModel.attackPower + "";
        healthText.text = cardModel.healthPoint + "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Card card = null;

        card = this.GetComponent<BaseEntity>().cardModel;

        if (card != null)
        {
            CanvasManager.Instance.GenerateCardPreview(card);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasManager.Instance.HideCardPreview();
    }
}
