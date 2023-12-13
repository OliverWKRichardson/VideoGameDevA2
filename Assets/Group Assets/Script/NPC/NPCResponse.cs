using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCResponse : MonoBehaviour
{
    // Stored values from the response lines
    [HideInInspector] public int NPCPath;
    [HideInInspector] public string NPCState;
    [HideInInspector] public string NPCSetState;
    [HideInInspector] public string NPCReward;
    [HideInInspector] public string NPCTake;

    // Reset values
    public void ResetValues()
    {
        NPCPath = 0;
        NPCState = null;
        NPCSetState = null;
        NPCReward = null;
        NPCTake = null;
    }
}
