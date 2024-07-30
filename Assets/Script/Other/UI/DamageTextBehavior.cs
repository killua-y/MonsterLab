using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextBehavior : MonoBehaviour
{
    private float timer = 0.6f;

    public void Setup(int damage)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = damage.ToString();

        if (damage >= 20)
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else if (damage >= 100)
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }

        Destroy(this.gameObject, timer);
    }
}
