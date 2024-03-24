using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextColorChanger : MonoBehaviour
{
    [Tooltip("Target color.")]
    [SerializeField]
    private Color _targetColor;

    [Space]
    [Tooltip("Text.")]
    [SerializeField]
    private TMP_Text _text;

    public void ChangeTextColor()
    {
        _text.color = _targetColor;
    }
}
