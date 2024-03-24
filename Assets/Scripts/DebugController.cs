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
    private int maxLevel;

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
    [Tooltip("It's bit mask. Example, if player have 1 and 3 ships => ownedShips = 5 (001 & 100 = 101)")]
    [SerializeField]
    private int ownedShips;

    [SerializeField]
    private bool debtPay;

    [SerializeField]
    private bool sound;

    [SerializeField]
    private bool music;

    [SerializeField]
    private float volume;

    private string _lastBonusDateKey = "LastBonusDate";

    private readonly string endlessModeKey = "EndlessMode";
    private readonly string maxLevelKey = "MaxLevel";

    private readonly string currentLevelKey = "CurrentLevel";
    private readonly string currentShipKey = "CurrentShip";

    private readonly string _soundKey = "SoundKey";
    private readonly string _musicKey = "MusicKey";
    private readonly string _musicVolumeKey = "MusicVolumeKey";
    
    private readonly string shieldBonusKey = "ShieldBonus";
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    private readonly string deathRayBonusKey = "DeathRayBonus";
    
    private readonly string moneyCountKey = "MoneyCount";
    
    private readonly string debtPayKey = "DebtPay";

    private readonly string ownedShipsKey = "OwnedShips";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(_soundKey))
            PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(true));

        if (!PlayerPrefs.HasKey(_musicKey))
            PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(true));

        if (!PlayerPrefs.HasKey(endlessModeKey))
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));

        if (!PlayerPrefs.HasKey(maxLevelKey))
            PlayerPrefs.SetInt(maxLevelKey, 1);

        if (!PlayerPrefs.HasKey(currentLevelKey))
            PlayerPrefs.SetInt(currentLevelKey, 1);

        if (!PlayerPrefs.HasKey(currentShipKey))
            PlayerPrefs.SetInt(currentShipKey, 1);

        if (!PlayerPrefs.HasKey(_musicVolumeKey))
            PlayerPrefs.SetFloat(_musicVolumeKey, 1f);

        if (!PlayerPrefs.HasKey(shieldBonusKey))
            PlayerPrefs.SetInt(shieldBonusKey, 0);

        if (!PlayerPrefs.HasKey(gunBoostBonusKey))
            PlayerPrefs.SetInt(gunBoostBonusKey, 0);

        if (!PlayerPrefs.HasKey(deathRayBonusKey))
            PlayerPrefs.SetInt(deathRayBonusKey, 0);

        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        if (!PlayerPrefs.HasKey(debtPayKey))
            PlayerPrefs.SetInt(debtPayKey, Convert.ToInt32(false));

        if (!PlayerPrefs.HasKey(ownedShipsKey))
            PlayerPrefs.SetInt(ownedShipsKey, 1);

        SetPlayerPrefs();
    }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt(_soundKey, Convert.ToInt32(sound));
        PlayerPrefs.SetInt(_musicKey, Convert.ToInt32(music));
        PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(endlessMode));
        PlayerPrefs.SetInt(maxLevelKey, maxLevel);
        PlayerPrefs.SetInt(currentLevelKey, level);
        PlayerPrefs.SetInt(currentShipKey, ship);
        PlayerPrefs.SetFloat(_musicVolumeKey, volume);
        PlayerPrefs.SetInt(shieldBonusKey, shieldBonus);
        PlayerPrefs.SetInt(gunBoostBonusKey, gunBoostBonus);
        PlayerPrefs.SetInt(deathRayBonusKey, deathRayBonus);
        PlayerPrefs.SetInt(moneyCountKey, money);
        PlayerPrefs.SetInt(debtPayKey, Convert.ToInt32(debtPay));
        PlayerPrefs.SetInt(ownedShipsKey, ownedShips);
    }
}