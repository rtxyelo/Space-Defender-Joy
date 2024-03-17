using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textBox;

    [SerializeField]
    private AudioClip typingClip;

    [SerializeField]
    private AudioSourceGroup audioSourceGroup;

    [TextArea]
    public List<string> dialogues = new List<string>(14);

    private DialogueVertexAnimator dialogueVertexAnimator;
    
    private Coroutine typeRoutine = null;
    
    private void Awake() 
    {
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox, audioSourceGroup);
    }

    private void PlayDialogue(string message)
    {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, typingClip, null));
    }

    public void PlayDialogue(int dialogueIndex)
    {
        PlayDialogue(dialogues[dialogueIndex]);
    }
}
