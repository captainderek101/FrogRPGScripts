using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public Text charName;
    public Text dialogue;
    public GameObject dialogueBox;

    bool typing = false;
    bool buffered = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if(dialogueBox.activeSelf)
        {
            return;
        }
        dialogueBox.SetActive(true);
        Debug.Log("Starting conversation with "+dialogue.name);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        charName.text = dialogue.name;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (typing)
        {
            buffered = true;
        }
        else
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }
    }

    IEnumerator TypeSentence (string sentence)
    {
        typing = true;
        dialogue.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogue.text += letter;
            yield return new WaitForFixedUpdate();
            if(buffered)
            {
                dialogue.text = sentence;
                break;
            }
        }
        typing = false;
        buffered = false;
    }

    public void EndDialogue()
    {
        if(dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(false);
            Debug.Log("End of conversation.");
        }
        GameManager.manager.StartBattle(0);
    }
}