using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("Levels duration in seconds")]
    [SerializeField]
    private List<float> levelsDurationList = new(10);

    [SerializeField]
    private List<float> enemySpawnSpeed = new(10);

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private List<GameObject> playersList = new(6);

    [SerializeField]
    private TMP_Text currentLevelText;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject losePanel;

    [SerializeField]
    private TMP_Text winScoreText;

    [SerializeField]
    private AudioBehaviour audioBehaviour;

    private readonly string endlessModeKey = "EndlessMode";

    private readonly string currentLevelKey = "CurrentLevel";

    private readonly string maxLevelKey = "MaxLevel";

    private readonly string currentShipKey = "CurrentShip";

    private readonly string moneyCountKey = "MoneyCount";

    private bool _isEndlessMode;

    private float elapsedTime = 0f;

    private int currentLevel;

    private int currentShip;

    private ShipController currentShipController;

    public ShipController ShipController { get { return currentShipController; } }

    private bool isShipDestroyed = false;

    private bool isShipShoted = false;

    private bool gunBoostIsActive = false;

    Dictionary<EnemyType, int> listOfEnemies = new Dictionary<EnemyType, int>();

    private BonusesController bonusesController;

    private void Start()
    {
        isShipDestroyed = false;
        isShipShoted = false;
        gunBoostIsActive = false;
        elapsedTime = 0f;

        if (!PlayerPrefs.HasKey(currentLevelKey))
            PlayerPrefs.SetInt(currentLevelKey, 1);

        if (!PlayerPrefs.HasKey(currentShipKey))
            PlayerPrefs.SetInt(currentShipKey, 1);

        if (!PlayerPrefs.HasKey(endlessModeKey))
            PlayerPrefs.SetInt(endlessModeKey, Convert.ToInt32(true));

        _isEndlessMode = Convert.ToBoolean(PlayerPrefs.GetInt(endlessModeKey, 1));

        if (_isEndlessMode)
        {
            currentLevel = 1;
            currentLevelText.enabled = false;
        }
        else
            currentLevel = PlayerPrefs.GetInt(currentLevelKey, 1);

        currentShip = PlayerPrefs.GetInt(currentShipKey, 1);

        Debug.Log("currentShip " + currentShip);

        playersList[currentShip - 1].SetActive(true);

        currentShipController = playersList[currentShip - 1].GetComponent<ShipController>();

        if (playersList[currentShip - 1].TryGetComponent(out ShipController controller))
        {
            controller.DeadEventHandler.AddListener(ShipDestroyed);
            controller.ShotEventHandler.AddListener(ShipShoted);
        }

        enemySpawner.IsSpawnAllow = true;

        bonusesController = FindObjectOfType<BonusesController>();
        if (bonusesController != null)
            bonusesController.GunBoostEvent.AddListener(GunBoostIsActive);

        listOfEnemies = LevelComplexity(currentLevel);
        enemySpawner.CollectEnemyListByLevelComplexity(listOfEnemies);
        enemySpawner.SpawnTime = enemySpawnSpeed[currentLevel];
    }

    private void Update()
    {
        if (!_isEndlessMode)
        {
            elapsedTime += Time.deltaTime;

            if (isShipDestroyed)
            {
                enemySpawner.IsSpawnAllow = false;
                DeactiveteGunBoostBonus();
                bonusesController.ResetBonuses();
                Debug.Log("Game Lose!");
                GameLose();
            }
            else if (elapsedTime >= levelsDurationList[currentLevel - 1] && !isShipDestroyed)
            {
                enemySpawner.IsSpawnAllow = false;
                DeactiveteGunBoostBonus();
                bonusesController.ResetBonuses();
                playersList[currentShip - 1].SetActive(false);
                Debug.Log("Game Win!");
                GameWin();
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= levelsDurationList[currentLevel - 1])
            {
                if (currentLevel >= 10)
                    currentLevel = 10;
                else
                    currentLevel++;
               
                enemySpawner.SpawnTime = enemySpawnSpeed[currentLevel - 1];
                enemySpawner.IsSpawnAllow = false;

                listOfEnemies = LevelComplexity(currentLevel);
                enemySpawner.CollectEnemyListByLevelComplexity(listOfEnemies);
                
                enemySpawner.IsSpawnAllow = true;

                DeactiveteGunBoostBonus();
                bonusesController.ResetBonuses();

                elapsedTime = 0;

                Debug.Log("NEW WAVE (based on levels)");
            }

            if (isShipDestroyed)
            {
                enemySpawner.IsSpawnAllow = false;
                DeactiveteGunBoostBonus();
                bonusesController.ResetBonuses();
                Debug.Log("Game Over!");
                GameLose();
            }
        }

        if (isShipShoted)
        {
            DeactiveteGunBoostBonus();
            isShipShoted = false;
        }
    }

    private void GameWin()
    {
        if (!winPanel.activeSelf)
        {
            audioBehaviour.GameWin();
            if (PlayerPrefs.GetInt(currentLevelKey, 1) + 1 > PlayerPrefs.GetInt(maxLevelKey, 1))
            {
                PlayerPrefs.SetInt(maxLevelKey, PlayerPrefs.GetInt(maxLevelKey, 1) + 1);
                Debug.Log("Max Level Key " + PlayerPrefs.GetInt(maxLevelKey, 0));
            }

            winScoreText.text = "Money: " + PlayerPrefs.GetInt(moneyCountKey, 0).ToString();
            winPanel.SetActive(true);
        }
    }

    private void GameLose()
    {
        if (!losePanel.activeSelf)
        {
            audioBehaviour.GameLose();
            losePanel.SetActive(true);
        }
    }

    public void GunBoostIsActive()
    {   
        if (!gunBoostIsActive)
        {
            currentShipController.ShootingSpeed += 2;
            gunBoostIsActive = true;
        }
    }

    public void PlayNextLevel()
    {
        if (PlayerPrefs.GetInt(currentLevelKey, 1) != 10)
            PlayerPrefs.SetInt(currentLevelKey, PlayerPrefs.GetInt(currentLevelKey, 1) + 1);
        else
            PlayerPrefs.SetInt(currentLevelKey, 1);
    }

    private void DeactiveteGunBoostBonus()
    {
        if (gunBoostIsActive)
        {
            currentShipController.ShootingSpeed -= 2;
            gunBoostIsActive = false;
        }
    }

    private void ShipDestroyed()
    {
        isShipDestroyed = true;
    }

    private void ShipShoted()
    {
        isShipShoted = true;
    }

    private Dictionary<EnemyType, int> LevelComplexity(int level)
    {
        Dictionary<EnemyType, int> listOfEnemies = new Dictionary<EnemyType, int>();

        switch (level)
        {
            case 1:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 1);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 4);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 0);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 0);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break; 
                }
            case 2:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 5);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 1);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 0);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 3:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 4);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 2);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 0);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 4:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 1);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 1);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 3);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 0);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 5:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 2);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 1);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 3);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 5);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 0);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 6:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 2);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 2);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 1);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 7:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 2);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 1);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 4);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 3);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 0);
                    break;
                }
            case 8:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 2);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 0);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 4);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 4);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 1);
                    break;
                }
            case 9:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 2);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 1);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 0);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 3);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 3);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 3);
                    break;
                }
            case 10:
                {
                    listOfEnemies.Add(EnemyType.AsteroidOne, 1);
                    listOfEnemies.Add(EnemyType.AsteroidTwo, 2);
                    listOfEnemies.Add(EnemyType.ScoutEnemy, 0);
                    listOfEnemies.Add(EnemyType.RaiderEnemy, 2);
                    listOfEnemies.Add(EnemyType.DestroyerEnemy, 3);
                    listOfEnemies.Add(EnemyType.DreadnoughtEnemy, 5);
                    break;
                }

        }

        return listOfEnemies;
    }

}

public enum EnemyType
{
    None = -1,
    AsteroidOne,
    AsteroidTwo,
    ScoutEnemy,
    RaiderEnemy,
    DestroyerEnemy,
    DreadnoughtEnemy,
}