using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopBehaviour : MonoBehaviour
{
    [Tooltip("List of ship select/buy buttons.")]
    [SerializeField]
    private List<Button> buttons = new(6);

    [Tooltip("List of ships prices.")]
    [SerializeField]
    private List<int> shipsPrices = new(5) { 5000, 10000, 20000, 50000, 200000 };

    [SerializeField]
    private GameObject buyPanel;

    [SerializeField]
    private Button agreeBuyButton; 
    
    [SerializeField]
    private Button disagreeBuyButton;

    private readonly string ownedShipsKey = "OwnedShips";

    private readonly string currentShipKey = "CurrentShip";

    private readonly string moneyCountKey = "MoneyCount";

    public UnityEvent<int> ShipEquipped;

    private void Awake()
    {
        ShipEquipped = new UnityEvent<int>();

        if (!PlayerPrefs.HasKey(ownedShipsKey))
            PlayerPrefs.SetInt(ownedShipsKey, 1);

        for (int i = 0; i < buttons.Count; i++)
        {
            int buttonIndex = i + 1;
            buttons[i].onClick.AddListener(delegate {

                //ToggleAllButtons(buttons, false);
                CheckUsability(buttonIndex);

            });
        }
    }

    public bool IsShipOwned(ShipType shipType, int ownedShips)
    {
        return ((ownedShips & (int)shipType) == (int)shipType);
    }

    private void CheckUsability(int ship)
    {
        int ownedShips = PlayerPrefs.GetInt(ownedShipsKey, 1);

        bool isShipOwned = IsShipOwned((ShipType)IndexToShipType(ship), ownedShips);

        Debug.Log($"Ship {ship} owned status is " +  isShipOwned);

        if (isShipOwned)
        {
            Equip(ship);
        }
        else
        {
            buyPanel.SetActive(true);

            agreeBuyButton.onClick.RemoveAllListeners();
            agreeBuyButton.onClick.AddListener(() => BuyAgree((ShipType)IndexToShipType(ship), ship, ownedShips));

            disagreeBuyButton.onClick.AddListener(BuyDisagree);
        }
    }

    private void Equip(int shipIndex)
    {
        // For text change to "Equipped"
        ShipEquipped?.Invoke(shipIndex);

        PlayerPrefs.SetInt(currentShipKey, shipIndex);

        //ToggleAllButtons(buttons, true);
    }

    private void Update()
    {
        Debug.Log("Current money " + PlayerPrefs.GetInt(moneyCountKey, 0));
    }

    private void BuyAgree(ShipType ship, int shipIndex, int ownedShips)
    {
        if (shipsPrices[shipIndex - 2] <= PlayerPrefs.GetInt(moneyCountKey, 0))
        {
            Debug.Log("Ship PRICE " + shipsPrices[shipIndex - 2]);
            Debug.Log("Current money " + PlayerPrefs.GetInt(moneyCountKey, 0));

            int moneyCount = PlayerPrefs.GetInt(moneyCountKey, 0) - shipsPrices[shipIndex - 2];

            Debug.Log("Result money " + moneyCount);

            PlayerPrefs.SetInt(moneyCountKey, moneyCount);

            PlayerPrefs.SetInt(currentShipKey, shipIndex);

            ownedShips |= (int)ship;

            PlayerPrefs.SetInt(ownedShipsKey, ownedShips);

            //ToggleAllButtons(buttons, true);
            
            ShipEquipped?.Invoke(shipIndex);
            
            buyPanel.SetActive(false);
        }
    }

    private void BuyDisagree()
    {
        //ToggleAllButtons(buttons, true);
        buyPanel.SetActive(false);
    }

    private void ToggleAllButtons(List<Button> buttonsList, bool toggleOn=true)
    {
        if (toggleOn)
            buttonsList.ForEach(button => button.enabled = true);
        else
            buttonsList.ForEach(button => button.enabled = false);
    }

    private void OnDestroy()
    {
        ShipEquipped.RemoveAllListeners();
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
}

[Flags]
public enum ShipType
{
    // Ships 654321
    // Mask  111111
    None = 0,
    ShipOne = 1 << 0,
    ShipTwo = 1 << 1,
    ShipThree = 1 << 2,
    ShipFour = 1 << 3,
    ShipFive = 1 << 4,
    ShipSix = 1 << 5,
}
