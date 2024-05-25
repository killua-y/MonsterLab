using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUnitHorizontalLayout : MonoBehaviour
{
    public int spacing = 70;

    public void SortAndPositionChildren()
    {
        List<TurnUnitBehavior> children = new List<TurnUnitBehavior>();

        // Collect all TurnUnitBehavior components from children
        foreach (Transform child in transform)
        {
            TurnUnitBehavior turnUnit = child.GetComponent<TurnUnitBehavior>();
            if (turnUnit != null)
            {
                children.Add(turnUnit);
            }
        }

        // Sort the list based on the index
        children.Sort((a, b) => a.index.CompareTo(b.index));

        // Calculate the total width of all children including spacing
        float totalWidth = 0;
        foreach (var child in children)
        {
            totalWidth += child.GetComponent<RectTransform>().rect.width;
        }
        totalWidth += (children.Count - 1) * spacing;

        // Calculate the starting position
        float startX = -totalWidth / 2;

        // Position the children
        foreach (var child in children)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(startX + rectTransform.rect.width / 2, 0);
            startX += rectTransform.rect.width + spacing;
        }
    }
}
