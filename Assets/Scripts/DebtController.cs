using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebtController : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttonsList = new(2);

    [SerializeField]
    private List<Button> buttonsImagesList = new(2);

    [SerializeField]
    private GameObject tryToPayDebtPanel;

    [SerializeField]
    private GameObject debtPayedPanel;

    [SerializeField]
    private SceneBehaviour sceneManager;

    private readonly string moneyCountKey = "MoneyCount";
    private readonly string debtPayKey = "DebtPay";

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        if (!PlayerPrefs.HasKey(debtPayKey))
            PlayerPrefs.SetInt(debtPayKey, 0);

        bool debtStatus = CheckDebtPayed();

        buttonsList[0].onClick.AddListener(delegate
        {
            if (!debtStatus)
                TryToPayDebt();
            else
            {
                sceneManager.LoadSceneByName("ShopScene");
            }
        });

        buttonsList[1].onClick.AddListener(delegate
        {
            if (!debtStatus)
                TryToPayDebt();
            else
            {
                sceneManager.LoadSceneByName("CampaignScene");
            }
        });
    }

    private bool CheckDebtPayed()
    {
        if (PlayerPrefs.GetInt(debtPayKey, 0) == 1)
            return true;
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
                PlayerPrefs.SetInt(moneyCountKey, PlayerPrefs.GetInt(moneyCountKey, 0) - 5000);
                PlayerPrefs.SetInt(debtPayKey, 1);
                debtPayedPanel.SetActive(true);
            }
        });

        noButton.onClick.AddListener(delegate
        {
            tryToPayDebtPanel.SetActive(false);
        });
    }
}
