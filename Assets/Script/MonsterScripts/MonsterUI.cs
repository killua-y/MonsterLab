using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class MonsterUI : MonoBehaviour
{
    // 怪兽UI部分
    public SpriteRenderer spriteRender;
    public Slider healthBar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    private Image fillImage;
    public GameObject bullet;

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
}
