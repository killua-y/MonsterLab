using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageSliderBehavior : MonoBehaviour
{
    public Image smallIcon;
    public Slider damageBar;
    public int amount = 0;
    public TextMeshProUGUI damageText;

    public void UpdateSmallIcon(string spriteLocation)
    {
        smallIcon.sprite = Resources.Load<Sprite>(spriteLocation);
    }

    public void UpdateDamageSlide(int maxValue, int newDamage)
    {
        amount += newDamage;
        damageBar.maxValue = maxValue;
        damageBar.value = amount;

        damageText.text = amount.ToString();
    }
}
