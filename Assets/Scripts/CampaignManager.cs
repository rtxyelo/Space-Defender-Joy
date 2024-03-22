using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    [Tooltip("Levels duration in seconds")]
    [SerializeField]
    private List<float> levelsDurationList = new(10);

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private List<GameObject> playersList = new(6);

    private readonly string currentLevelKey = "CurrentLevel";

    private readonly string currentShipKey = "CurrentShip";

    private int currentLevel;

    private int currentShip;

    private ShipController currentShipController;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(currentLevelKey))
            PlayerPrefs.SetInt(currentLevelKey, 1);

        if (!PlayerPrefs.HasKey(currentShipKey))
            PlayerPrefs.SetInt(currentShipKey, 1);

        currentLevel = PlayerPrefs.GetInt(currentLevelKey, 1);

        currentShip = PlayerPrefs.GetInt(currentShipKey, 1);

        playersList[currentShip].SetActive(true);

        currentShipController = playersList[currentShip].GetComponent<ShipController>();

        if (playersList[currentShip].TryGetComponent(out ShipController controller))
        {
            controller.AddListener(ShipDestroyed);
        }
    }

    private void Update()
    {
        if (Time.unscaledTime < levelsDurationList[currentLevel - 1])
        {
            enemySpawner.IsSpawnAllow = true;
        }
        else
        {
            enemySpawner.IsSpawnAllow = false;
            Debug.Log("Game Over !!");
        }
    }

    private void ShipDestroyed()
    {

    }
}
