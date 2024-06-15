using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Card;

public class DamageSliderBehavior : MonoBehaviour
{
    public Image smallIcon;
    public Slider damageBar;
    public int amount = 0;
    public TextMeshProUGUI damageText;

    public void UpdateSmallIcon(MonsterCard cardModel)
    {
        if (cardModel.smallIconLocation != "")
        {
            smallIcon.sprite = Resources.Load<Sprite>(cardModel.smallIconLocation);
        }
        else if (cardModel.imageLocation != "")
        {
            smallIcon.sprite = Resources.Load<Sprite>(cardModel.imageLocation);
        }
    }

    public void UpdateDamageSlide(int newDamage)
    {
        amount += newDamage;
        damageBar.value = amount;

        damageText.text = amount.ToString();
    }

    public void UpdateMaxDamage(int maxValue)
    {
        damageBar.maxValue = maxValue;
    }
}
