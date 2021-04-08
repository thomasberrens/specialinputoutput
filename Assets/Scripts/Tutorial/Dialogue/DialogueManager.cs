using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    private DialogueTrigger _dialogueTrigger;
    public bool isTyping;

    public UnityEvent startPlayableTutorial;
    public UnityEvent spawnEnemy;

    [SerializeField] private bool TutorialMode = true;
    [SerializeField] private GameObject _continueButton;

    // Use this for initialization
    void Start () {
        sentences = new Queue<string>();
        _dialogueTrigger = GetComponent<DialogueTrigger>();
        _dialogueTrigger.TriggerDialogue();
        _continueButton = GameObject.FindWithTag(Values.ContinueButtonTag);
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (TutorialMode)
        {
            if (sentences.Count == 5)
            {
                startPlayableTutorial?.Invoke();
                Debug.Log("Starting playable tutorial");
                Destroy(_continueButton);
            }

            if (sentences.Count == 1)
            {
                spawnEnemy?.Invoke();
                Debug.Log("Time to spawn a enemy");
            }
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            
            isTyping = true;
            Debug.Log("Started typing");
            yield return null;
        }
        
        Debug.Log("Stopped typing");
        isTyping = false;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

}