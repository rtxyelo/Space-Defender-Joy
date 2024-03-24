using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessModeController : MonoBehaviour
{
    private readonly string endlessModeKey = "EndlessMode";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(endlessModeKey))
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));
    }

    public void EndlessMode(bool isEndlessMode)
    {
        PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(isEndlessMode));
    }
}
