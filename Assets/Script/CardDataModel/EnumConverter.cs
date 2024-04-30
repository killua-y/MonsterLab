using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumConverter : MonoBehaviour
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
}
