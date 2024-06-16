using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageBoarderBehavior : VerticalLayoutGroup
{
    public override void SetLayoutVertical()
    {
        // Check if the base components and children are valid
        if (this == null || transform == null)
            return;

        base.SetLayoutVertical();
        SortChildren();
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
