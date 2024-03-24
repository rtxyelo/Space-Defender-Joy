using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.SplashScreen;


public class ShopButtonsStatus : MonoBehaviour
{
    [Tooltip("List of shop buttons prices texts.")]
    [SerializeField]
    private List<TMP_Text> shopButtonsPricesTexts = new(5);

    [Tooltip("List of shop buttons prices coins.")]
    [SerializeField]
    private List<Image> shopButtonsPricesCoins = new(5);

    [Tooltip("List of shop buttons texts.")]
    [SerializeField]
    private List<TMP_Text> shopButtonsTexts = new(6);

    private ShopBehaviour shopBehaviour;

    private readonly string ownedShipsKey = "OwnedShips";

    private readonly string currentShipKey = "CurrentShip";

    private void Awake()
    {
        shopBehaviour = GetComponent<ShopBehaviour>();
        shopBehaviour.ShipEquipped.AddListener(UpdateButtonsStatus);
    }

    private void Start()
    {
        Debug.Log("Current ship " + PlayerPrefs.GetInt(currentShipKey, 1));
        UpdateButtonsStatus(PlayerPrefs.GetInt(currentShipKey, 1));

        ShipType shipTypeIndex = (ShipType)IndexToShipType(PlayerPrefs.GetInt(currentShipKey, 1));

        Debug.Log("ShipType " + shipTypeIndex);
        Debug.Log("ShipType " + (int)shipTypeIndex);
    }

    public int IndexToShipType(int index)
    {
        switch (index)
        {
            case 1: return 1;
            case 2: return 2;
            case 3: return 4;
            case 4: return 8;
            case 5: return 16;
            case 6: return 32;
            default: return 0;
        }
    }

    private void UpdateButtonsStatus(int shipIndex)
    {
        for (int i = 0; i < shopButtonsTexts.Count; i++)
        {
            bool isShipOwned = shopBehaviour.IsShipOwned((ShipType)IndexToShipType(i + 1), PlayerPrefs.GetInt(ownedShipsKey, 1));

            Debug.Log($"Ship {i + 1} owned status is " + isShipOwned);

            if (i + 1 == shipIndex)
            {
                shopButtonsTexts[i].text = "EQUIPPED";

                if (i > 0 && isShipOwned)
                {
                    shopButtonsPricesCoins[i - 1].enabled = false;
                    shopButtonsPricesTexts[i - 1].text = "";
                }
            }
            else if (isShipOwned && shopButtonsTexts[i].text == "EQUIPPED")
            {
                shopButtonsTexts[i].text = "EQUIP";
            }
        }
    }
}
