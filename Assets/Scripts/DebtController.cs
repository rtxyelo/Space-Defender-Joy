using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DebtController : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttonsList = new(2);

    [SerializeField]
    private List<Image> buttonsImagesList = new(2);

    [SerializeField]
    private GameObject tryToPayDebtPanel;

    [SerializeField]
    private GameObject debtPayedPanel;

    [SerializeField]
    private SceneBehaviour sceneManager;

    [SerializeField]
    private Sprite accessSprite;
    

    [SerializeField]
    private GameObject locksObject;

    private readonly string moneyCountKey = "MoneyCount";
    private readonly string debtPayKey = "DebtPay";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        if (!PlayerPrefs.HasKey(debtPayKey))
            PlayerPrefs.SetInt(debtPayKey, 0);

        bool debtPayCheck = CheckDebtPayed();

        buttonsList[0].onClick.AddListener(delegate
        {
            if (!CheckDebtPayed())
                TryToPayDebt();
            else
            {
                sceneManager.LoadSceneByName("ShopScene");
            }
        });

        buttonsList[1].onClick.AddListener(delegate
        {
            if (!CheckDebtPayed())
                TryToPayDebt();
            else
            {
                sceneManager.LoadSceneByName("LevelsScene");
            }
        });
    }

    private bool CheckDebtPayed()
    {
        if (PlayerPrefs.GetInt(debtPayKey, 0) == 1)
        {
            buttonsImagesList[0].sprite = accessSprite;
            buttonsImagesList[1].sprite = accessSprite;
            locksObject.SetActive(false);
            return true;
        }
        else 
            return false;
    }

    private void TryToPayDebt()
    {
        tryToPayDebtPanel.SetActive(true);
        Button yesButton = tryToPayDebtPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        Button noButton = tryToPayDebtPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>();

        yesButton.onClick.AddListener(delegate
        {
            if (PlayerPrefs.GetInt(moneyCountKey, 0) >= 5000)
            {
                int money = PlayerPrefs.GetInt(moneyCountKey, 0) - 5000;
                PlayerPrefs.SetInt(moneyCountKey, money);
                PlayerPrefs.SetInt(debtPayKey, 1);
                debtPayedPanel.SetActive(true);
                buttonsImagesList[0].sprite = accessSprite;
                buttonsImagesList[1].sprite = accessSprite;
                locksObject.SetActive(false);
                tryToPayDebtPanel.SetActive(false);
            }
        });

        noButton.onClick.AddListener(delegate
        {
            tryToPayDebtPanel.SetActive(false);
        });
    }
}
