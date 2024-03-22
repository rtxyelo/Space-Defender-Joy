using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BonusesController : MonoBehaviour, IBonusesController
{
    [Header("Bonuses animators")]
    [Space]
    [SerializeField]
    private List<Animator> shieldBonusAnimators = new List<Animator>(6);

    [SerializeField]
    private List<Animator> deathRayBonusAnimators = new List<Animator>(6);

    [Header("Bonuses buttons")]
    [Space]
    [SerializeField]
    private List<Button> bonusButtonsList = new List<Button>(3);

    [SerializeField]
    private List<Image> bonusButtonsImagesList = new List<Image>(3);

    [SerializeField]
    private List<TMP_Text> bonusCountersList = new List<TMP_Text>(3);

    [SerializeField]
    private List<Sprite> bonusButtonsSpritesList = new List<Sprite>(6);

    private readonly string currentShipKey = "CurrentShip";
    private int currentShipIndex;

    private readonly string shieldBonusKey = "ShieldBonus";
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    private readonly string deathRayBonusKey = "DeathRayBonus";

    private bool isShieldBonusActive = false;
    
    private bool isGunBoostBonusActive = false;

    private bool isDeathRayBonusActive = false;

    private UnityEvent gunBoostEvent;

    public enum BonusType
    {
        Shield,
        GunBoost,
        Death
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(shieldBonusKey))
            PlayerPrefs.SetInt(shieldBonusKey, 0);   
        
        if (!PlayerPrefs.HasKey(gunBoostBonusKey))
            PlayerPrefs.SetInt(gunBoostBonusKey, 0);  
        
        if (!PlayerPrefs.HasKey(deathRayBonusKey))
            PlayerPrefs.SetInt(deathRayBonusKey, 0);     
        
        if (!PlayerPrefs.HasKey(currentShipKey))
            PlayerPrefs.SetInt(currentShipKey, 1);
        else
            currentShipIndex = PlayerPrefs.GetInt(currentShipKey, 1) - 1;

        bonusCountersList[0].text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);
        bonusCountersList[1].text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);
        bonusCountersList[2].text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);

    }

    private void Update()
    {
        
    }

    #region Shield Bonus Methods

    public void ShieldBonusOn()
    {
        if (PlayerPrefs.GetInt(shieldBonusKey, 0) > 0 && !isShieldBonusActive)
        {
            isShieldBonusActive = true;

            bonusButtonsList[0].enabled = false;

            PlayerPrefs.SetInt(shieldBonusKey, PlayerPrefs.GetInt(shieldBonusKey, 1) - 1);

            bonusCountersList[0].text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);

            bonusButtonsImagesList[0].sprite = bonusButtonsSpritesList[1];

            shieldBonusAnimators[currentShipIndex].SetBool("ShieldOff", false);
            shieldBonusAnimators[currentShipIndex].Play("Activate");

            StartCoroutine(ShieldRoutine());
        }
    }

    public IEnumerator ShieldRoutine()
    {

        yield return new WaitForSeconds(5f);

        shieldBonusAnimators[currentShipIndex].SetBool("ShieldOff", true);

        bonusButtonsImagesList[0].sprite = bonusButtonsSpritesList[0];

        bonusButtonsList[0].enabled = true;

        isShieldBonusActive = false;
    }

    public void TutorialShieldBonusOn(int shipIndex)
    {
        if (!isShieldBonusActive)
        {
            isShieldBonusActive = true;

            bonusButtonsList[0].enabled = false;

            bonusButtonsImagesList[0].sprite = bonusButtonsSpritesList[1];

            shieldBonusAnimators[shipIndex].SetBool("ShieldOff", false);
            shieldBonusAnimators[shipIndex].Play("Activate");

            StartCoroutine(TutorialShieldRoutine(shipIndex));
        }
    }

    public IEnumerator TutorialShieldRoutine(int shipIndex)
    {

        yield return new WaitForSeconds(10f);

        shieldBonusAnimators[shipIndex].SetBool("ShieldOff", true);

        bonusButtonsImagesList[0].sprite = bonusButtonsSpritesList[0];

        bonusButtonsList[0].enabled = true;

        isShieldBonusActive = false;
    }

    #endregion

    #region Death Ray Bonus Methods
    public void DeathRayBonusOn()
    {
        if (PlayerPrefs.GetInt(deathRayBonusKey, 0) > 0 && !isDeathRayBonusActive)
        {
            isDeathRayBonusActive = true;

            bonusButtonsList[2].enabled = false;

            PlayerPrefs.SetInt(deathRayBonusKey, PlayerPrefs.GetInt(deathRayBonusKey, 1) - 1);

            bonusCountersList[2].text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);

            bonusButtonsImagesList[2].sprite = bonusButtonsSpritesList[5];

            deathRayBonusAnimators[currentShipIndex].SetBool("RayOff", false);
            deathRayBonusAnimators[currentShipIndex].Play("Activate");

            StartCoroutine(DeathRayRoutine());
        }
    }

    public IEnumerator DeathRayRoutine()
    {

        yield return new WaitForSeconds(3f);

        deathRayBonusAnimators[currentShipIndex].SetBool("RayOff", true);

        bonusButtonsImagesList[2].sprite = bonusButtonsSpritesList[4];

        bonusButtonsList[2].enabled = true;

        isDeathRayBonusActive = false;
    }

    public void TutorialDeathRayBonusOn(int shipIndex)
    {
        if (!isDeathRayBonusActive)
        {
            isDeathRayBonusActive = true;

            bonusButtonsList[2].enabled = false;

            bonusButtonsImagesList[2].sprite = bonusButtonsSpritesList[5];

            deathRayBonusAnimators[shipIndex].SetBool("RayOff", false);
            deathRayBonusAnimators[shipIndex].Play("Activate");

            StartCoroutine(TutorialDeathRayRoutine(shipIndex));
        }
    }

    public IEnumerator TutorialDeathRayRoutine(int shipIndex)
    {

        yield return new WaitForSeconds(10f);

        deathRayBonusAnimators[shipIndex].SetBool("RayOff", true);

        bonusButtonsImagesList[2].sprite = bonusButtonsSpritesList[4];

        bonusButtonsList[2].enabled = true;

        isDeathRayBonusActive = false;
    }

    #endregion

    #region Gun Boost Methods

    public void GunBoostBonusOn()
    {
        if (PlayerPrefs.GetInt(gunBoostBonusKey, 0) > 0 && !isGunBoostBonusActive)
        {
            isDeathRayBonusActive = true;

            bonusButtonsList[1].enabled = false;

            PlayerPrefs.SetInt(gunBoostBonusKey, PlayerPrefs.GetInt(gunBoostBonusKey, 1) - 1);

            bonusCountersList[1].text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);

            bonusButtonsImagesList[1].sprite = bonusButtonsSpritesList[3];
        }
    }

    #endregion

    /// <summary>
    /// Call when player 
    /// </summary>
    public void ResetBonuses()
    {
        isShieldBonusActive= false;
        isDeathRayBonusActive = false;
        isGunBoostBonusActive = false;

        bonusButtonsList[0].enabled = true;
        bonusButtonsList[1].enabled = true;
        bonusButtonsList[2].enabled = true;

        bonusCountersList[0].text = "x" + PlayerPrefs.GetInt(shieldBonusKey, 0);
        bonusCountersList[1].text = "x" + PlayerPrefs.GetInt(gunBoostBonusKey, 0);
        bonusCountersList[2].text = "x" + PlayerPrefs.GetInt(deathRayBonusKey, 0);
    }
}
