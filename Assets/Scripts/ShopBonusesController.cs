using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBonusesController : MonoBehaviour
{
    [SerializeField]
    private List<Button> bonusButtons = new(3);

    [SerializeField]
    private List<TMP_Text> bonusTexts = new(3);

    private List<int> bonusesPrice = new(3) { 250, 400, 700 };

    private readonly string shieldBonusKey = "ShieldBonus";
    
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    
    private readonly string deathRayBonusKey = "DeathRayBonus";

    private readonly string moneyCountKey = "MoneyCount";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(shieldBonusKey))
            PlayerPrefs.SetInt(shieldBonusKey, 0);

        if (!PlayerPrefs.HasKey(gunBoostBonusKey))
            PlayerPrefs.SetInt(gunBoostBonusKey, 0);

        if (!PlayerPrefs.HasKey(deathRayBonusKey))
            PlayerPrefs.SetInt(deathRayBonusKey, 0);

        bonusTexts[0].text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);
        bonusTexts[1].text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);
        bonusTexts[2].text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);

        bonusButtons[0].onClick.AddListener(() => {

            BuyShield();
        });

        bonusButtons[1].onClick.AddListener(() => {

            BuyGunBooster();
        });

        bonusButtons[2].onClick.AddListener(() => {

            BuyDeathRay();
        });
    }

    private void BuyShield()
    {
        if (PlayerPrefs.GetInt(moneyCountKey, 0) >= bonusesPrice[0])
        {
            PlayerPrefs.SetInt(moneyCountKey, PlayerPrefs.GetInt(moneyCountKey, 0) - bonusesPrice[0]);
            PlayerPrefs.SetInt(shieldBonusKey, PlayerPrefs.GetInt(shieldBonusKey, 0) + 1);
            bonusTexts[0].text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);
        }
    }

    private void BuyGunBooster()
    {
        if (PlayerPrefs.GetInt(moneyCountKey, 0) >= bonusesPrice[1])
        {
            PlayerPrefs.SetInt(moneyCountKey, PlayerPrefs.GetInt(moneyCountKey, 0) - bonusesPrice[1]);
            PlayerPrefs.SetInt(gunBoostBonusKey, PlayerPrefs.GetInt(gunBoostBonusKey, 0) + 1);
            bonusTexts[1].text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);
        }
    }

    private void BuyDeathRay()
    {
        if (PlayerPrefs.GetInt(moneyCountKey, 0) >= bonusesPrice[2])
        {
            PlayerPrefs.SetInt(moneyCountKey, PlayerPrefs.GetInt(moneyCountKey, 0) - bonusesPrice[2]);
            PlayerPrefs.SetInt(deathRayBonusKey, PlayerPrefs.GetInt(deathRayBonusKey, 0) + 1);
            bonusTexts[2].text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);
        }
    }
}
