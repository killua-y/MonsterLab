using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Card;

public class MonsterUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // public 部分
    public GameObject bullet;
    public Transform attackOrgan;

    // 怪兽UI部分
    private Canvas MonsterCanvas;
    private Slider healthBar;
    private Slider manaBar;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI attackText;
    private Transform StatusBar;
    private GameObject StatusUnit;

    private void Awake()
    {
        // 如果没有子弹，则会添加默认子弹
        if (bullet == null)
        {
            bullet = Resources.Load<GameObject>("MonsterPrefab/bullet");
        }

        // Get Status bar
        MonsterCanvas = GetComponentInChildren<Canvas>();
        StatusBar = MonsterCanvas.transform.GetChild(0);

        // Get生命值bar和mana bar
        Slider[] childSlider = GetComponentsInChildren<Slider>();
        foreach (Slider slider in childSlider)
        {
            if (slider.gameObject.name == "HealthBar")
            {
                healthBar = slider;
            }
            else if (slider.gameObject.name == "ManaBar")
            {
                manaBar = slider;
                manaBar.gameObject.SetActive(false);
            }
        }

        // Get Text
        TextMeshProUGUI[] childText = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in childText)
        {
            if (text.gameObject.name == "HealthText")
            {
                healthText = text;
            }
            else if (text.gameObject.name == "AttackText")
            {
                attackText = text;
            }
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

        Image fillImage;
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

    public StatusUnitBehavior AddNewStatus()
    {
        if (StatusUnit == null)
        {
            StatusUnit = Resources.Load<GameObject>("UI/MonsterUI/StatusUnit");
        }

        if (StatusUnit != null)
        {
            StatusUnitBehavior newStatusUnit = Instantiate(StatusUnit, StatusBar).GetComponent<StatusUnitBehavior>();
            return newStatusUnit;
        }
        else
        {
            Debug.Log("StatusUnit Prefab Missing, Please add to UI/MonsterUI/StatusUnit Under Resource");
            return null;
        }
    }

    private void OnDestroy()
    {
        CanvasManager.Instance.HideCardPreview();
    }
}
