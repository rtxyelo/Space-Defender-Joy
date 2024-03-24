using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeBehaviour : MonoBehaviour
{
    [SerializeField]
    private Button endlessModeButton;

    [SerializeField]
    private SceneBehaviour sceneBehaviour;

    private readonly string endlessModeKey = "EndlessMode";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(endlessModeKey))
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));

        endlessModeButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));
            sceneBehaviour.LoadSceneByName("MainGameScene");
        });
    }
}
