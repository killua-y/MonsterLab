using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer highlightSprite;
    public Color validColor;
    public Color wrongColor;

    public void SetHighlight(bool active, bool valid)
    {
        highlightSprite.gameObject.SetActive(active);

        if (valid)
        {
            highlightSprite.color = validColor;
        }
        else
        {
            highlightSprite.color = wrongColor;
        }
    }
}
