using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public static Tools instance;
    private void Awake()
    {
        instance = this;
    }

    public float Normalize(float min, float max, float currentValue)
    {
        // Ensure the currentValue is within the bounds of min and max
        currentValue = Mathf.Clamp(currentValue, min, max);

        // Normalize the value between 0 and 1
        float normalizedValue = (currentValue - min) / (max - min);

        // Return the normalized value
        return normalizedValue;
    }
}
