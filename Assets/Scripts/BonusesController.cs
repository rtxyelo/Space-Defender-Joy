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
    private List<Sprite> bonusButtonsSpritesList = new List<Sprite>(6);

    private readonly string currentShipKey = "CurrentShip";
    private int currentShipIndex;

    private readonly string shieldBonusKey = "ShieldBonus";
    private readonly string gunBoostBonusKey = "GunBoostBonus";
    private readonly string deathRayBonusKey = "DeathRayBonus";

    private bool isShieldBonusActive = false;
    
    private bool isGunBoostBonusActive = false;

    private bool isDeathRayBonusActive = false;

    public UnityEvent GunBoostEvent;

    public UnityEvent ShieldCounterChangeEvent;
    public UnityEvent GunBoostCounterChangeEvent;
    public UnityEvent DeathRayChangeEvent;

    public enum BonusType
    {
        Shield,
        GunBoost,
        Death
    }

    private void Awake()
    {
        GunBoostEvent = new UnityEvent();
        ShieldCounterChangeEvent = new UnityEvent();
        GunBoostCounterChangeEvent = new UnityEvent();
        DeathRayChangeEvent = new UnityEvent();
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

        ShieldCounterChangeEvent?.Invoke();
        GunBoostCounterChangeEvent?.Invoke();
        DeathRayChangeEvent?.Invoke();
    }

    #region Shield Bonus Methods

    public void ShieldBonusOn()
    {
        if (PlayerPrefs.GetInt(shieldBonusKey, 0) > 0 && !isShieldBonusActive)
        {
            isShieldBonusActive = true;

            bonusButtonsList[0].enabled = false;

            PlayerPrefs.SetInt(shieldBonusKey, PlayerPrefs.GetInt(shieldBonusKey, 1) - 1);

            ShieldCounterChangeEvent?.Invoke();

            bonusButtonsImagesList[0].sprite = bonusButtonsSpritesList[1];

            if (shieldBonusAnimators[currentShipIndex] != null)
            {
                shieldBonusAnimators[currentShipIndex].SetBool("ShieldOff", false);
                shieldBonusAnimators[currentShipIndex].Play("Activate");
            }

            StartCoroutine(ShieldRoutine());
        }
    }

    public IEnumerator ShieldRoutine()
    {

        yield return new WaitForSeconds(5f);

        if (shieldBonusAnimators[currentShipIndex] != null)
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

            if (shieldBonusAnimators[shipIndex] != null)
            {
                shieldBonusAnimators[shipIndex].SetBool("ShieldOff", false);
                shieldBonusAnimators[shipIndex].Play("Activate");
            }

            StartCoroutine(TutorialShieldRoutine(shipIndex));
        }
    }

    public IEnumerator TutorialShieldRoutine(int shipIndex)
    {

        yield return new WaitForSeconds(10f);

        if (shieldBonusAnimators[shipIndex] != null)
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

            DeathRayChangeEvent?.Invoke();

            bonusButtonsImagesList[2].sprite = bonusButtonsSpritesList[5];

            if (deathRayBonusAnimators[currentShipIndex] != null)
            {
                deathRayBonusAnimators[currentShipIndex].SetBool("RayOff", false);
                deathRayBonusAnimators[currentShipIndex].Play("Activate");
            }

            StartCoroutine(DeathRayRoutine());
        }
    }

    public IEnumerator DeathRayRoutine()
    {

        yield return new WaitForSeconds(3f);

        if (deathRayBonusAnimators[currentShipIndex] != null)
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

            if (deathRayBonusAnimators[shipIndex] != null)
            {
                deathRayBonusAnimators[shipIndex].SetBool("RayOff", false);
                deathRayBonusAnimators[shipIndex].Play("Activate");
            }

            StartCoroutine(TutorialDeathRayRoutine(shipIndex));
        }
    }

    public IEnumerator TutorialDeathRayRoutine(int shipIndex)
    {

        yield return new WaitForSeconds(10f);

        if (deathRayBonusAnimators[shipIndex] != null)
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

            GunBoostCounterChangeEvent?.Invoke();

            bonusButtonsImagesList[1].sprite = bonusButtonsSpritesList[3];

            GunBoostEvent?.Invoke();
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
    }



    private void OnDestroy()
    {
        GunBoostEvent?.RemoveAllListeners();
        ShieldCounterChangeEvent?.RemoveAllListeners();
        GunBoostCounterChangeEvent?.RemoveAllListeners();
        DeathRayChangeEvent?.RemoveAllListeners();
    }
}
