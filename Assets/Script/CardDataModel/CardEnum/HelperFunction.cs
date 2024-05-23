using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunction : MonoBehaviour
{
    // Generic method to convert a string to any enum type
    public static T ConvertToEnum<T>(string value) where T : struct
    {
        if (Enum.TryParse<T>(value, true, out T result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"Invalid or non-existent enum value provided for type {typeof(T).Name}: {value}");
        }
    }

    public static Tile GetTileUnder()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, LayerMask.GetMask("Tile"));

        if (hit.collider != null)
        {
            //Released over something!
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }

        return null;
    }
}
