using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneController : MonoBehaviour
{
    [Header("Player")]
    [Space]
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private List<Transform> playerPositions = new List<Transform>(3);

    [SerializeField]
    private float playerShiftSpeed = 5.0f;

    private ShiftPlayerTo shiftPlayerTo = ShiftPlayerTo.TopPosition;

    private Vector3 playerBigScale = new Vector3(1.7f, 1.7f, 1f);

    private Vector3 playerDefaultScale = new Vector3(1f, 1f, 1f);

    [Header("Text panel")]
    [Space]
    [SerializeField]
    private GameObject textPanel;

    private RectTransform textPanelRect;

    [Header("Buttons")]
    [Space]
    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button skipButton;

    private int dialogueNumber = 0;

    private DialogueManager dialogueManager;

    private KennysShipController shipController;

    private enum ShiftPlayerTo
    {
        TopPosition = 0,
        CenterPosition = 1,
        BottomPosition = 2
    }

    private void Awake()
    {
        //nextButton.onClick.AddListener(delegate { dialogueManager.PlayDialogue(dialogueNumber);
        //    Resize();
        //});     
        
        nextButton.onClick.AddListener(delegate { dialogueManager.PlayDialogue(dialogueNumber);});
    }

    private void Start()
    {
        shipController = player.GetComponent<KennysShipController>();
        shipController.IsMoveAllowed = false;

        dialogueManager = GetComponent<DialogueManager>();
        dialogueManager.PlayDialogue(dialogueNumber);
        dialogueNumber++;

        textPanelRect = textPanel.GetComponent<RectTransform>();

        //Resize(player.transform, playerBigScale, 1f);
    }

    private void Update()
    {
        if (!shipController.IsMoveAllowed && player.transform.position != playerPositions[(int)shiftPlayerTo].position)
        {
            ShiftPlayerToPosition(playerPositions[(int)shiftPlayerTo].position);
        }

        Debug.Log("Text panel sizeDelta " + textPanelRect.sizeDelta);
    }
    private void ShiftPlayerToPosition(Vector3 position)
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, position, playerShiftSpeed * Time.deltaTime);
    }

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

    /// <summary>
    /// Resize GameObject sizeDelta.
    /// </summary>
    /// <param name="rectTransform">Target GameObject RectTransform</param>
    /// <param name="newSizeDeltaY">New sizeDelta</param>
    /// <param name="time">Scaling coroutine time</param>
    private void Resize(RectTransform rectTransform, float newSizeDeltaY, float time)
    {
        if (_resizeCoroutine != null)
            StopCoroutine(_resizeCoroutine);
        _resizeCoroutine = ResizeCoroutine(rectTransform, newSizeDeltaY, time);
        StartCoroutine(_resizeCoroutine);
    }

    private IEnumerator ResizeCoroutine(RectTransform rectTransform, float newSizeDeltaY, float time)
    {
        float Timer = 0;
        Vector2 Base = rectTransform.sizeDelta;
        while (Timer < time)
        {
            transform.localScale = Vector2.Lerp(Base, new Vector2(0f, newSizeDeltaY), Timer / time);
            yield return null; // задержка цикла до следующего кадра
            Timer += Time.deltaTime;
        }
        rectTransform.sizeDelta = new Vector2(0f, newSizeDeltaY);
        _resizeCoroutine = null;
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
