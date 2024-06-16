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
    public Slider healthBar;
    public Slider manaBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;
    public GameObject bullet;
    public Transform attackOrgan;

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
        // 如果是敌方则需要翻转x轴
        Vector3 newScale = transform.localScale;
        newScale.x = -Mathf.Abs(newScale.x);
        transform.localScale = newScale;

        // 防止血条也被翻转
        Transform canvasTransform = GetComponentInChildren<Canvas>().transform;
        Vector3 canvasScale = canvasTransform.localScale;
        canvasScale.x = 1f / newScale.x;
        canvasTransform.localScale = canvasScale;

        fillImage = healthBar.fillRect.GetComponent<Image>();
        fillImage.color = Color.red;
        DragMonster dragComponent = this.GetComponent<DragMonster>();
        if (dragComponent != null)
        {
            Destroy(dragComponent);
        }
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
