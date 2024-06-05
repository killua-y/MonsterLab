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
    public Slider manaBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;
    public GameObject bullet;

    private void Awake()
    {
        // 如果没有子弹，则会添加默认子弹
        if (bullet == null)
        {
            bullet = Resources.Load<GameObject>("MonsterPrefab/bullet");
        }
    }

    public void EnemyMonster()
    {
        spriteRender.flipX = true;
        fillImage = healthBar.fillRect.GetComponent<Image>();
        fillImage.color = Color.red;
        DragMonster dragComponent = this.GetComponent<DragMonster>();
        if (dragComponent != null)
        {
            Destroy(dragComponent);
        }

        // 这一行是为了更加方便分辨怪兽所属，以后要删除
        spriteRender.color = Color.red;
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

    public void UpdateManaUI(int maxAmount, int currentAmount)
    {
        if (!manaBar.gameObject.activeSelf)
        {
            manaBar.gameObject.SetActive(true);
        }

        manaBar.maxValue = maxAmount;
        manaBar.value = currentAmount;
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

    private void OnDestroy()
    {
        CanvasManager.Instance.HideCardPreview();
    }
}
