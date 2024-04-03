using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtonsBehaviour : MonoBehaviour
{
    private readonly string _soundKey = "SoundKey";
    private readonly string _musicKey = "MusicKey";

    [SerializeField] private Image _soundOnImg;
    [SerializeField] private Image _soundOffImg;
    [SerializeField] private Image _musicOnImg;
    [SerializeField] private Image _musicOffImg;

    [SerializeField] private TMP_Text _soundOnText;
    [SerializeField] private TMP_Text _soundOffText;
    [SerializeField] private TMP_Text _musicOnText;
    [SerializeField] private TMP_Text _musicOffText;

    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;

    [SerializeField] private Color _colorOn;
    [SerializeField] private Color _colorOff;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_soundKey))
            PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(false));

        if (!PlayerPrefs.HasKey(_musicKey))
            PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(false));
    
    
        if (PlayerPrefs.GetInt(_soundKey) == 1)
        {
            _soundOnText.color = _colorOn;
            _soundOffText.color = _colorOff;
            _soundOnImg.sprite = _spriteOn;
            _soundOffImg.sprite = _spriteOff;
        }
        else
        {
            _soundOnText.color = _colorOff;
            _soundOffText.color = _colorOn;
            _soundOnImg.sprite = _spriteOff;
            _soundOffImg.sprite = _spriteOn;
        }

        if (PlayerPrefs.GetInt(_musicKey) == 1)
        {
            _musicOnText.color = _colorOn;
            _musicOffText.color = _colorOff;
            _musicOnImg.sprite = _spriteOn;
            _musicOffImg.sprite = _spriteOff;
        }
        else
        {
            _musicOnText.color = _colorOff;
            _musicOffText.color = _colorOn;
            _musicOnImg.sprite = _spriteOff;
            _musicOffImg.sprite = _spriteOn;
        }
    }

    public void SoundOn()
    {
        PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(true));
        _soundOnText.color = _colorOn;
        _soundOffText.color = _colorOff;
        _soundOnImg.sprite = _spriteOn;
        _soundOffImg.sprite = _spriteOff;
    }

    public void SoundOff()
    {
        PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(false));
        _soundOnText.color = _colorOff;
        _soundOffText.color = _colorOn;
        _soundOnImg.sprite = _spriteOff;
        _soundOffImg.sprite = _spriteOn;
    }

}
