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
        Destroy(this.gameObject, timer);
    }
}
