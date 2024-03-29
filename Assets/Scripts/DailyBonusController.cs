using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyBonusController : MonoBehaviour
{
    public static Action<int> OnRewardTaken;

    private int _daysToReward = 1;
    private int _rewardValue = 2000;

    private DateTime _lastBonusDate;
    private string _lastBonusDateKey = "LastBonusDate";
    private readonly string moneyCountKey = "MoneyCount";

    private void Awake()
    {
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(_lastBonusDateKey))
        {
            _lastBonusDate = DateTime.Parse(PlayerPrefs.GetString(_lastBonusDateKey));
            Debug.Log("Last bonus date " + _lastBonusDate);
        }

        if (CalculateDaysSinceLastBonus() >= _daysToReward)
        {
            PlayerPrefs.SetInt(moneyCountKey, PlayerPrefs.GetInt(moneyCountKey) + _rewardValue);
            Debug.Log("Reward " + _rewardValue + " | Current money " + PlayerPrefs.GetInt(moneyCountKey));
            _lastBonusDate = DateTime.Now;
            PlayerPrefs.SetString(_lastBonusDateKey, _lastBonusDate.ToString());
        }
    }

    int CalculateDaysSinceLastBonus()
    {
        DateTime currentDate = DateTime.Now;
        DateTime lastBonus = Convert.ToDateTime(_lastBonusDate);
        TimeSpan timeSinceLastBonus = currentDate.Subtract(lastBonus);
        return timeSinceLastBonus.Days;
    }
}
