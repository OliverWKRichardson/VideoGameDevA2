using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    // Controller for the UI
    [SerializeField] DialogueController dialogueController;

    // Text file that contains the dialogue script
    [SerializeField] TextAsset dialogueFile;

    // lines extracted from the dialogueFile
    [HideInInspector] public string[] lines;

    // Name of the npc
    public string NpcName;

    // Number of states (bool vals used to toggle dialogue responses)
    [SerializeField] int noOfStates;

    // array of states
    [HideInInspector] public bool[] states;

    void Awake()
    {
        // Split the dialogueFile by line
        lines = dialogueFile.text.Split('\n');
        states = new bool[noOfStates];
    }

    // Start dialogue with this npc
    public void startDialogue()
    {
        dialogueController.startDialogue(NpcName, this);
    }
}
