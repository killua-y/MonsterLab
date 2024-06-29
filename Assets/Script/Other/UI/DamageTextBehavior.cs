using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextBehavior : MonoBehaviour
{
    private float timer = 1f;

    public void Setup(int damage)
    {
        this.gameObject.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }

    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
