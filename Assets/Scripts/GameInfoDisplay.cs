using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameInfoDisplay : MonoBehaviour
{
    [Header("Text fields")]
    [Space(10)]
    [Tooltip("Current game level text field.")]
    [SerializeField]
    private TMP_Text _level;

    [Tooltip("Collected money counter text field.")]
    [SerializeField]
    private TMP_Text _money;

    [Tooltip("Ship health counter text field.")]
    [SerializeField]
    private TMP_Text _health;

    [Tooltip("Shield bonuses counter text field.")]
    [SerializeField]
    private TMP_Text _shieldBonus;

    [Tooltip("Gun boost counter text field.")]
    [SerializeField]
    private TMP_Text _gunBoostBonus;

    [Tooltip("Death ray bonus counter text field.")]
    [SerializeField]
    private TMP_Text _deathRayBonus;

    private readonly string shieldBonusKey = "ShieldBonus";
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    private readonly string deathRayBonusKey = "DeathRayBonus";
    private readonly string currentLevelKey = "CurrentLevel";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(shieldBonusKey))
            PlayerPrefs.SetInt(shieldBonusKey, 0);

        if (!PlayerPrefs.HasKey(gunBoostBonusKey))
            PlayerPrefs.SetInt(gunBoostBonusKey, 0);

        if (!PlayerPrefs.HasKey(deathRayBonusKey))
            PlayerPrefs.SetInt(deathRayBonusKey, 0);

        BonusesController bonusesController = FindAnyObjectByType<BonusesController>();
        
        if (bonusesController != null )
        {
            bonusesController.ShieldCounterChangeEvent.AddListener(ShieldCounterChange);
            bonusesController.GunBoostCounterChangeEvent.AddListener(GunBoostCounterChange);
            bonusesController.DeathRayChangeEvent.AddListener(DeathRayCounterChange);
        }
    }

    private void Start()
    {
        if (_level.isActiveAndEnabled)
            _level.text = "Level " + PlayerPrefs.GetInt(currentLevelKey, 1).ToString();
    }

    public void HealthCounterChange(int health)
    {
        _health.text = "x" + health.ToString();
    }

    private void ShieldCounterChange()
    {
        _shieldBonus.text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);
    }

    private void GunBoostCounterChange()
    {
        _gunBoostBonus.text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);
    }

    private void DeathRayCounterChange()
    {
        _deathRayBonus.text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);
    }
}
