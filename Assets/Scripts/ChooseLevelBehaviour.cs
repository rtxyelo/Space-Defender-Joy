using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource asseptSound;
    [SerializeField] private AudioSource declineSound;
    [SerializeField] private List<Image> listOfButtonImages = new List<Image>();
    [SerializeField] private List<GameObject> locksList = new List<GameObject>();

    [SerializeField] private Sprite closedLvlSprite;
    [SerializeField] private Sprite openedLvlSprite;

    [SerializeField] private SceneBehaviour sceneBehaviour;

    private string maxLevelKey = "MaxLevel";
    private string currentLevelKey = "CurrentLevel";

    private readonly string _soundKey = "SoundKey";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(maxLevelKey))
        {
            PlayerPrefs.SetInt(maxLevelKey, 1);
        }
        int btnInd = 1;
        foreach (Image buttonImg in listOfButtonImages)
        {
            if (btnInd > PlayerPrefs.GetInt(maxLevelKey, 1000))
                buttonImg.sprite = closedLvlSprite;

            else if (btnInd != PlayerPrefs.GetInt(currentLevelKey, 0))
                buttonImg.sprite = openedLvlSprite;

            btnInd++;
        }

        int lockInd = 1;
        foreach (GameObject _lock in locksList)
        {
            if (lockInd > PlayerPrefs.GetInt(maxLevelKey, 1000) - 1)
                _lock.SetActive(true);

            //else if (lockInd != PlayerPrefs.GetInt(currentLevelKey, 0) - 1)
              else
                _lock.SetActive(false);

            lockInd++;
        }
    }

    public void SetCurrentLevel(int level)
    {
        if (level <= PlayerPrefs.GetInt(maxLevelKey, 1000))
        {
            PlayerPrefs.SetInt(currentLevelKey, level);
            Debug.Log("currentLevelKey " + PlayerPrefs.GetInt(currentLevelKey, 0));
            sceneBehaviour.LoadSceneByName("MainGameScene");
        }
    }


    /// <summary>
    /// Check level availability by level number.
    /// </summary>
    /// <param name="lvl">Level number.</param>
    public void CheckLevelAvailability(int lvl)
    {
        if (!PlayerPrefs.HasKey(_soundKey))
        {
            PlayerPrefs.SetInt(_soundKey, 1);
        }

        if (!PlayerPrefs.HasKey(currentLevelKey))
        {
            PlayerPrefs.SetInt(currentLevelKey, 1);
        }

        if (!PlayerPrefs.HasKey(maxLevelKey))
        {
            PlayerPrefs.SetInt(maxLevelKey, 1);
        }
        else if (lvl <= PlayerPrefs.GetInt(maxLevelKey, 1000))
        {
            PlayerPrefs.SetInt(currentLevelKey, lvl);
            Debug.Log("currentLevelKey " + PlayerPrefs.GetInt(currentLevelKey, 0));

            int btnInd = 1;
            foreach (Image buttonImg in listOfButtonImages)
            {
                if (btnInd > PlayerPrefs.GetInt(maxLevelKey, 1000))
                    buttonImg.sprite = closedLvlSprite;

                else if (btnInd != lvl)
                    buttonImg.sprite = openedLvlSprite;

                btnInd++;
            }

            if (asseptSound != null && PlayerPrefs.GetInt(_soundKey, 1) != 0f)
            {
                asseptSound.Play();
            }
        }
        else
        {
            if (declineSound != null && PlayerPrefs.GetInt(_soundKey, 1) != 0f)
            {
                declineSound.Play();
            }
        }
    }
}
