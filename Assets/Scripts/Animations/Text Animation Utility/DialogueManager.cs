using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private const int DIALOGUES_COUNT = 22;

    [SerializeField]
    private List<TMP_Text> textBoxes = new List<TMP_Text>(DIALOGUES_COUNT);

    [SerializeField]
    private AudioClip typingClip;

    [SerializeField]
    private AudioSourceGroup audioSourceGroup;

    [TextArea]
    public List<string> dialogues = new List<string>(DIALOGUES_COUNT);

    private DialogueVertexAnimator dialogueVertexAnimator;
    
    private Coroutine typeRoutine = null;

    private Action onFinishAction;

    private TutorialSceneController tutorialSceneController;

    private void Awake()
    {
        tutorialSceneController = GetComponent<TutorialSceneController>();
    }

    private void PlayDialogue(string message, bool _isPlaySound)
    {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, onFinishAction, _isPlaySound));
    }

    public void PlayDialogue(int dialogueIndex, bool _isPlaySound)
    {
        if (dialogueIndex < DIALOGUES_COUNT)
        {
            if (dialogueIndex > 0)
            {
                for (int i = dialogueIndex - 1; i >= 0; i--)
                    textBoxes[i].transform.parent.gameObject.SetActive(false);
            }

            textBoxes[dialogueIndex].transform.parent.gameObject.SetActive(true);
            dialogueVertexAnimator = new DialogueVertexAnimator(textBoxes[dialogueIndex], audioSourceGroup);

            onFinishAction = () =>
            {
                tutorialSceneController.NextButtnoStatus = true;
            };

            PlayDialogue(dialogues[dialogueIndex], _isPlaySound);
        }
    }

    public void HideTextPanel(int dialogueIndex)
    {
        textBoxes[dialogueIndex].transform.parent.gameObject.SetActive(false);
    }

    public void ShowTextPanel(int dialogueIndex)
    {
        textBoxes[dialogueIndex].transform.parent.gameObject.SetActive(true);
    }
}
