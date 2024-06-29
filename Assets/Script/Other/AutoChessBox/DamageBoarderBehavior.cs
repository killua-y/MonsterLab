using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBoarderBehavior : MonoBehaviour
{
    private float spacing = -10f; // Space between elements

    public void UpdateLayout()
    {
        SortChildren();
        float currentY = 0f;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Set child's position based on its sibling index and spacing
            child.localPosition = new Vector2(0, -currentY);

            // Update currentY to account for the child's height and spacing
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                currentY += rectTransform.rect.height + spacing;
            }
        }
    }

    private void SortChildren()
    {
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        children.Sort((a, b) =>
        {
            DamageSliderBehavior aBehavior = a.GetComponent<DamageSliderBehavior>();
            DamageSliderBehavior bBehavior = b.GetComponent<DamageSliderBehavior>();
            if (aBehavior == null || bBehavior == null)
            {
                return 0;
            }
            return bBehavior.amount.CompareTo(aBehavior.amount); // Descending order
        });

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }
}
