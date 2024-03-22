using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneController : MonoBehaviour
{
    #region Fields
    [Header("Player")]
    [Space]
    [SerializeField]
    private List<GameObject> players = new(6);

    [SerializeField]
    private List<Transform> playerPositions = new(3);

    [SerializeField]
    private float playerShiftSpeed = 5.0f;

    private ShiftPlayerTo shiftPlayerTo = ShiftPlayerTo.TopPosition;

    private bool isShift = false;

    private int currentShip = 0;

    private Vector3 playerBigScale = new(1.7f, 1.7f, 1f);

    private Vector3 playerDefaultScale = new (1f, 1f, 1f);

    [Header("Buttons")]
    [Space]
    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button takeKeysButton;

    [SerializeField]
    private Button skipButton;    
    
    [SerializeField]
    private Image nextButtonImage;

    [SerializeField]
    private Image skipButtonImage;


    [Header("Misc objects")]
    [Space]
    [SerializeField]
    private GameObject tutorialPlayerControlIcon;

    [SerializeField]
    private GameObject tutorialTextIcon;

    [SerializeField]
    private TutorialEnemySpawner tutorialEnemySpawner;

    [SerializeField]
    private BonusesController bonusesController;

    [SerializeField]
    private GameObject keyAnimatingObj;

    [SerializeField]
    private GameObject bonusesObj;

    public int DialogueNumber { get; private set; } = 0;

    private DialogueManager dialogueManager;

    private ShipController kennyShipController;

    private BackgroundPlanetSpawner backgroundPlanetSpawner;

    private TutorialAsteroidSpawner tutorialAsteroidSpawner;
    
    private bool isBeenCalled = false;

    private bool shipCanMove = false;

    private bool playMode = false;

    private bool isPlaySound = true;

    private bool nextButtonStatus = true;

    public bool NextButtnoStatus { get => nextButtonStatus; set => nextButtonStatus = value; }

    private enum ShiftPlayerTo
    {
        TopPosition = 0,
        CenterPosition = 1,
        BottomPosition = 2
    }

    private readonly string moneyCountKey = "MoneyCount";

    #endregion

    #region Private Methods

    private void Awake()
    {
        nextButton.onClick.AddListener(delegate
        {
            isBeenCalled = false;
            DialogueNumber++;
            dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);
            nextButtonStatus = false;
            Debug.Log("CLICK NEXT BUTTON!!");
        });

        skipButton.onClick.AddListener(delegate { SkipButtonClick(); });
    }

    // todo: rewrite this func
    private void SkipButtonClick()
    {
        isBeenCalled = false;
        DialogueNumber = 21;
        backgroundPlanetSpawner.IsSpawnAllow = false;
        tutorialPlayerControlIcon.SetActive(false);

        foreach (var item in players)
        {
            item.SetActive(false);
        }

        tutorialAsteroidSpawner.SetAsteroidsCount(0);
        tutorialAsteroidSpawner.AsteroidDestroyHandler();
        tutorialAsteroidSpawner.IsSpawnAllow = false;

        dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);

        tutorialEnemySpawner.DeactivateEnemies();
        kennyShipController.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(moneyCountKey))
            PlayerPrefs.SetInt(moneyCountKey, 0);

        kennyShipController = players[0].GetComponent<ShipController>();

        kennyShipController.IsMoveAllow = false;
        kennyShipController.IsShootingAllow = false;

        dialogueManager = GetComponent<DialogueManager>();
        dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);

        backgroundPlanetSpawner = FindObjectOfType<BackgroundPlanetSpawner>();
        tutorialAsteroidSpawner = FindObjectOfType<TutorialAsteroidSpawner>();
    }

    private void Update()
    {
        //nextButton.enabled = nextButtonStatus;

        CheckCurrentDialogueNumber();

        if (isShift)
        {
            kennyShipController.IsMoveAllow = false;
            ShiftPlayerToPosition(playerPositions[(int)shiftPlayerTo].position, currentShip);
        }

        if (players[currentShip].transform.position == playerPositions[(int)shiftPlayerTo].position)
        {
            if (shipCanMove)
            {
                kennyShipController.IsMoveAllow = true;
                shipCanMove = false;
            }
            isShift = false;
        }
        
    }

    private void ShiftPlayerToPosition(Vector3 position, int shipIndex)
    {
        players[shipIndex].transform.position = Vector3.MoveTowards(players[shipIndex].transform.position, position, playerShiftSpeed * Time.deltaTime);
    }

    private void CheckCurrentDialogueNumber()
    {
        Debug.Log("Current dialogue number: " + DialogueNumber);

        // Ship control demonstrate
        if (!isBeenCalled && DialogueNumber == 2)
        {
            isBeenCalled = true;

            tutorialPlayerControlIcon.SetActive(true);

            Resize(players[currentShip].transform, playerDefaultScale, 0.7f);

            shipCanMove = true;

            ShipInfo shipInfo = GetShipInfo(kennyShipController);
            Debug.Log("Ship info " + shipInfo.SerializeShipInfo());
        }


        // Asteroid flybys
        else if (DialogueNumber == 3 && !playMode)
        {
            if (!isBeenCalled)
            {
                isBeenCalled = true;

                Resize(players[currentShip].transform, playerDefaultScale, 0.7f);
                isShift = true;
                shiftPlayerTo = ShiftPlayerTo.CenterPosition;

                backgroundPlanetSpawner.IsSpawnAllow = false;

                tutorialPlayerControlIcon.SetActive(false);

                nextButton.enabled = false;
            }

            if (players[currentShip].transform.localScale == playerDefaultScale && !isShift)
            {
                shipCanMove = true;
                playMode = true;
                isPlaySound = false;
                nextButton.enabled = true;
            }
        }

        // Enter asteroids flybys play mode
        else if (DialogueNumber == 4 && playMode)
        {
            isBeenCalled = false;

            playMode = false;

            tutorialAsteroidSpawner.IsSpawnAllow = true;

            skipButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            dialogueManager.HideTextPanel(DialogueNumber);
            isPlaySound = true;

            isShift = true;
            shiftPlayerTo = ShiftPlayerTo.BottomPosition;

            shipCanMove = true;
        }
        
        // Exit asteroids flybys play mode
        else if (!isBeenCalled && DialogueNumber == 4 && !playMode && !tutorialAsteroidSpawner.IsSpawnAllow)
        {
            isBeenCalled = true;

            backgroundPlanetSpawner.IsSpawnAllow = true;

            skipButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            dialogueManager.ShowTextPanel(DialogueNumber);
            dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);

            Resize(players[currentShip].transform, playerBigScale, 0.7f);
            isShift = true;
            shiftPlayerTo = ShiftPlayerTo.TopPosition;
        }
        

        // Shooting asteroids
        else if (DialogueNumber == 6 && !playMode)
        {
            if (!isBeenCalled)
            {
                isBeenCalled = true;

                Resize(players[currentShip].transform, playerDefaultScale, 1f);
                isShift = true;
                shiftPlayerTo = ShiftPlayerTo.CenterPosition;

                backgroundPlanetSpawner.IsSpawnAllow = false;

                nextButton.enabled = false;
            }
            if (players[currentShip].transform.localScale == playerDefaultScale && !isShift)
            {
                kennyShipController.IsShootingAllow = true;
                shipCanMove = true;
                playMode = true;
                isPlaySound = false;
                nextButton.enabled = true;
            }
        }

        // Enter shooting asteroids play mode
        else if (DialogueNumber == 7 && playMode)
        {
            isBeenCalled = false;

            playMode = false;

            tutorialAsteroidSpawner.IsSpawnAllow = true;

            skipButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);

            dialogueManager.HideTextPanel(DialogueNumber);
            isPlaySound = true;

            isShift = true;
            shiftPlayerTo = ShiftPlayerTo.BottomPosition;

            shipCanMove = true;
        }

        // Exit shooting asteroids play mode
        else if (!isBeenCalled && DialogueNumber == 7 && !playMode && !tutorialAsteroidSpawner.IsSpawnAllow)
        {
            isBeenCalled = true;

            kennyShipController.IsShootingAllow = false;

            backgroundPlanetSpawner.IsSpawnAllow = true;

            skipButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);

            dialogueManager.ShowTextPanel(DialogueNumber);
            dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);

            Resize(players[currentShip].transform, playerBigScale, 0.7f);
            isShift = true;
            shiftPlayerTo = ShiftPlayerTo.TopPosition;
        }

        // Demonstrate types of spaceships
        else if (!isBeenCalled && DialogueNumber >= 9 && DialogueNumber <= 13)
        {
            isBeenCalled = true;

            tutorialAsteroidSpawner.SetAsteroidsCount(100);
            tutorialAsteroidSpawner.IsSpawnAllow = true;

            players[currentShip].transform.localScale = playerDefaultScale;
            shiftPlayerTo = ShiftPlayerTo.CenterPosition;
            players[currentShip].transform.position = playerPositions[(int)shiftPlayerTo].position;

            players[currentShip].gameObject.SetActive(false);
            currentShip++;
            players[currentShip].gameObject.SetActive(true);
        }

        else if (!isBeenCalled && DialogueNumber == 14)
        {
            isBeenCalled = true;

            players[currentShip].gameObject.SetActive(false);
            currentShip = 0;
            players[currentShip].gameObject.SetActive(true);

            shipCanMove = true;

            bonusesController.TutorialShieldBonusOn(currentShip);
        }

        else if (!isBeenCalled && DialogueNumber == 15)
        {
            isBeenCalled = true;
            bonusesController.TutorialDeathRayBonusOn(currentShip);
        }

        // Bonuses off
        else if (!isBeenCalled && DialogueNumber == 16)
        {
            isBeenCalled = true;

            bonusesObj.SetActive(false);

            tutorialAsteroidSpawner.SetAsteroidsCount(0);
            tutorialAsteroidSpawner.AsteroidDestroyHandler();
            tutorialAsteroidSpawner.IsSpawnAllow = false;

            isShift = true;
            shiftPlayerTo = ShiftPlayerTo.CenterPosition;

            shipCanMove = false;
            kennyShipController.IsMoveAllow = false;
        }

        // Scouts is coming
        else if (!isBeenCalled && DialogueNumber == 17)
        {
            isBeenCalled = true;
            backgroundPlanetSpawner.IsSpawnAllow = false;
            tutorialEnemySpawner.IsSpawnAllow = true;
        }

        // 5 minutes later text ON
        else if (!isBeenCalled && DialogueNumber == 18)
        {
            isBeenCalled = true;
            StartCoroutine(WaitCoroutine(3f));
        }

        // final dead!
        else if (!isBeenCalled && DialogueNumber == 19)
        {
            isBeenCalled = true;

            tutorialEnemySpawner.ActivateEnemies();
            kennyShipController.gameObject.SetActive(true);

            tutorialTextIcon.SetActive(true);

            nextButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);

            kennyShipController.SetIsDead(DamageType.Scout);
        }

        // debt
        else if (!isBeenCalled && DialogueNumber == 20)
        {
            isBeenCalled = true;

            tutorialEnemySpawner.DeactivateEnemies();
            kennyShipController.gameObject.SetActive(false);
        }

        // take key
        else if (!isBeenCalled && DialogueNumber == 21)
        {
            isBeenCalled = true;

            nextButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
            takeKeysButton.gameObject.SetActive(true);

            keyAnimatingObj.SetActive(true);
        }


    }

    private IEnumerator WaitCoroutine(float time)
    {
        tutorialEnemySpawner.DeactivateEnemies();
        kennyShipController.gameObject.SetActive(false);

        nextButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        tutorialTextIcon.SetActive(false);

        yield return new WaitForSeconds(time);
        
        DialogueNumber++;

        dialogueManager.PlayDialogue(DialogueNumber, isPlaySound);

        isBeenCalled = false;
    }

    private ShipInfo GetShipInfo<T> (T controller) where T: PlayerShipBase
    {
        ShipInfo shipInfo = new ShipInfo(controller.Maneuverability,
                                         controller.Damage,
                                         controller.Durability,
                                         controller.ShootingSpeed,
                                         controller.HealthPoints,
                                         controller.IsDead,
                                         controller.IsMoveAllow,
                                         controller.IsShootingAllow);
        return shipInfo;
    }

    #endregion

    #region Resize Methods

    private IEnumerator _resizeCoroutine;

    /// <summary>
    /// Resize GameObject scale.
    /// </summary>
    /// <param name="transform">Target GameObject Transform</param>
    /// <param name="newScale">New scale</param>
    /// <param name="time">Scaling coroutine time</param>
    private void Resize(Transform transform, Vector3 newScale, float time)
    {
        if (_resizeCoroutine != null)
            StopCoroutine(_resizeCoroutine);
        _resizeCoroutine = ResizeCoroutine(transform, newScale, time);
        StartCoroutine(_resizeCoroutine);
    }

    private IEnumerator ResizeCoroutine(Transform transform, Vector3 newScale, float time)
    {
        float Timer = 0;
        Vector3 Base = transform.localScale;
        while (Timer < time)
        {
            transform.localScale = Vector3.Lerp(Base, newScale, Timer / time);
            yield return null; // задержка цикла до следующего кадра
            Timer += Time.deltaTime;
        }
        transform.localScale = newScale;
        _resizeCoroutine = null;
    }

    #endregion
}
