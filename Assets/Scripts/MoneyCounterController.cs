using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounterController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText;

    private readonly string moneyCountKey = "MoneyCount";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        MoneyChange(PlayerPrefs.GetInt(moneyCountKey, 0));
    }

    private void Update()
    {
        MoneyChange(PlayerPrefs.GetInt(moneyCountKey, 0));
    }

    private void MoneyChange(int money)
    {
        moneyText.text = money.ToString();
    }
}
