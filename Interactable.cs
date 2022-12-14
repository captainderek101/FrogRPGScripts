using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interactable : MonoBehaviour
{
    public Dialogue[] dialogue;
    private bool dialogueTriggered = false;
    private DialogueManager manager;
    [SerializeField]
    private int enemyType = 0;

    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
        manager.dialogueEndEvent += EndDialogue;
    }

    private void EndDialogue()
    {
        dialogueTriggered = false;
    }

    public void TriggerDialogue()
    {
        dialogueTriggered = true;
        manager.curBattle = enemyType;
        manager.StartDialogue(dialogue[0]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
            TriggerDialogue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && dialogueTriggered)
            manager.EndDialogue();
    }
}
