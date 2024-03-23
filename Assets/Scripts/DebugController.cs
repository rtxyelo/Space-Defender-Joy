using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    [SerializeField]
    private bool endlessMode;

    [SerializeField]
    private int level;

    [SerializeField]
    private int ship;

    [SerializeField]
    private int money;

    [Space]
    [SerializeField]
    private int shieldBonus;

    [SerializeField]
    private int gunBoostBonus;

    [SerializeField]
    private int deathRayBonus;

    [Space]
    [SerializeField]
    [Range(0, 1)]
    private int debtPay;

    [SerializeField]
    private float volume;

    private readonly string endlessModeKey = "EndlessMode";

    private readonly string currentLevelKey = "CurrentLevel";
    private readonly string currentShipKey = "CurrentShip";

    private readonly string _musicVolumeKey = "MusicVolumeKey";
    
    private readonly string shieldBonusKey = "ShieldBonus";
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    private readonly string deathRayBonusKey = "DeathRayBonus";
    
    private readonly string moneyCountKey = "MoneyCount";
    
    private readonly string debtPayKey = "DebtPay";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(endlessModeKey))
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));

        if (!PlayerPrefs.HasKey(currentLevelKey))
            PlayerPrefs.SetInt(currentLevelKey, 1);

        if (!PlayerPrefs.HasKey(currentShipKey))
            PlayerPrefs.SetInt(currentShipKey, 1);

        if (!PlayerPrefs.HasKey(_musicVolumeKey))
            PlayerPrefs.SetFloat(_musicVolumeKey, 0.2f);

        if (!PlayerPrefs.HasKey(shieldBonusKey))
            PlayerPrefs.SetInt(shieldBonusKey, 0);

        if (!PlayerPrefs.HasKey(gunBoostBonusKey))
            PlayerPrefs.SetInt(gunBoostBonusKey, 0);

        if (!PlayerPrefs.HasKey(deathRayBonusKey))
            PlayerPrefs.SetInt(deathRayBonusKey, 0);

        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        if (!PlayerPrefs.HasKey(debtPayKey))
            PlayerPrefs.SetInt(debtPayKey, 0);

        SetPlayerPrefs();
    }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(endlessMode));
        PlayerPrefs.SetInt(currentLevelKey, 1);
        PlayerPrefs.SetInt(currentLevelKey, level);
        PlayerPrefs.SetInt(currentShipKey, ship);
        PlayerPrefs.SetFloat(_musicVolumeKey, volume);
        PlayerPrefs.SetInt(shieldBonusKey, shieldBonus);
        PlayerPrefs.SetInt(gunBoostBonusKey, gunBoostBonus);
        PlayerPrefs.SetInt(deathRayBonusKey, deathRayBonus);
        PlayerPrefs.SetInt(moneyCountKey, money);
        PlayerPrefs.SetInt(debtPayKey, debtPay);
    }
}