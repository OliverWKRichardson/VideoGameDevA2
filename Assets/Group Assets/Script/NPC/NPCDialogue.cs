using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] DialogueController dialogueController;

    [SerializeField] TextAsset dialogueFile;

    [HideInInspector] public string[] lines;

    public string NpcName;

    [SerializeField] int noOfStates;

    [HideInInspector] public bool[] states;

    void Awake()
    {
        lines = dialogueFile.text.Split('\n');
        states = new bool[noOfStates];
    }

    public void startDialogue()
    {
        dialogueController.startDialogue(NpcName, this);
    }
}
