using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Card;

public class MonsterUI : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    // 怪兽UI部分
    public SpriteRenderer spriteRender;
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;
    public GameObject bullet;
    public GameObject cardPreviewParent;

    // 卡片预览
    BaseEntity baseEntity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void RightPointDown()
    {
        if (cardPreviewParent != null)
        {
            if (cardPreviewParent.activeSelf)
            {
                PointExist();
                return;
            }

            if (baseEntity == null)
            {
                baseEntity = GetComponent<BaseEntity>();
            }

            if (baseEntity != null)
            {
                MonsterCard card = GetComponent<BaseEntity>().cardModel;
                GameObject newCardObject = CardDisplayView.Instance.DisPlaySingleCard(card, cardPreviewParent.transform);
                newCardObject.AddComponent<Canvas>().sortingOrder = 10;
            }

            cardPreviewParent.SetActive(true);
        }
    }

    public void PointExist()
    {
        if (cardPreviewParent != null)
        {
            if (cardPreviewParent.activeSelf)
            {
                cardPreviewParent.SetActive(false);

                // Iterate through each child of the parent
                foreach (Transform child in cardPreviewParent.transform)
                {
                    // Destroy the child GameObject
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightPointDown();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointExist();
    }
}
